using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//아래 항목들 추가
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Object = UnityEngine.Object;

public class ResourceManager
{
    public T Load<T>(string key) where T : Object
    {
        if (_resources.TryGetValue(key, out Object resource)) //어드레서블 딕셔너리에 키값이 존재하면 오브젝트를 가져오겠다
            return resource as T; //가져온 오브젝트를 리턴

        return null;
    }

    #region Instantiate 리소스가 필요하면 여기
    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false) //키값을 가지고
    {
        GameObject prefab = Load<GameObject>($"{key}"); //어드레서블을 통해 로딩된 오브젝트들의 딕셔너리중 키값을 가지고 오브젝트를 가져옴
        if (prefab == null) 
        {
            Debug.Log($"Failed to load prefab : {key}");
            return null;
        }//깡통이면 예외처리

        if (pooling) //풀링 해줘야할 놈이라면 풀매니저에서 처리
        {
            return Managers.Pool.Pop(prefab);
        }

        GameObject go = Object.Instantiate(prefab, parent); //일단 하이러키부모는 안정해주고 go에 담아서 하이러키에 생성하겠음.
        //go.name = prefab.name; //이름도 지어주겠음 오브젝트매니저로 토스
        return go; //오브젝트 리턴
    }
    #endregion

    #region Destroy()
    public void Destroy(GameObject go) //사망처리
    {
        if (go == null) return;

        if (Managers.Pool.Push(go)) return; //푸쉬성공했으면 여기서 그만

        Object.Destroy(go); //실패했으면 풀링하는놈이 아닐테니까 걍 디스트로이
    }
    #endregion


    #region addressable
    Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();


    /*1.어떠한 키값을 받아서 로드를 해줄거임 */
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    { //결국 이 함수에서 callback이라는거는 너가 리턴을 하여 함수를 마쳤더라도 나중에 찾으면 콜백해줘
        if (_resources.TryGetValue(key, out Object resource)) //로드한적이 있다 (캐시확인)
        {
            callback?.Invoke(resource as T); //니가 찾는 리소스 줄게
            return;
        }

        string loadKey = key; //잼 어드레서블에는 텍스처 부모 아래에 스프라이트를 들고있는데 텍스처는 필요없고 스프라이트만 있으면되는데 걔를 불러오려면
        if (key.Contains(".sprite")) //.sprite를 포함한 키값이라면
            loadKey = $"{key}[{key.Replace(".sprite", "")}]"; //그 아래에있는 이름으로 바꾸겠다.


        var asyncOperation=Addressables.LoadAssetAsync<T>(key); //비동기방식. 가져왔을때 콜백함수를 받아 처리함
        asyncOperation.Completed += (op) => //비동기로 내가 찾는 어떠한 리소스를 가져왔다면 람다를 실행
        {
            _resources.Add(key, op.Result); //딕셔너리에 키값과 결과를 보관
            callback?.Invoke(op.Result); //LoadAsync를 부른놈한테 op.Result를 전달
            //즉 보관도 보관하고 전달도 하고
            
        };
    }

    public void LoadAllAsync<T>(string label, Action<string,int,int> callback ) where T : UnityEngine.Object //라벨에 있는것들 싹다 들고와줌
    {
        var opHandle = Addressables.LoadResourceLocationsAsync(label,typeof(T));
        opHandle.Completed += (op) =>
        {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                LoadAsync<T>(result.PrimaryKey, (obj) =>
                {
                    loadCount++;
                    callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }

}
#endregion

