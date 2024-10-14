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
        if (_resources.TryGetValue(key, out Object resource))
            return resource as T;

        return null;
    }

    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
    {
        GameObject prefab = Load<GameObject>($"{key}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {key}");
            return null;
        }

        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }
    public void Destroy(GameObject go)
    {
        if (go = null) return;


        Object.Destroy(go);
    }
    #region addressable
    Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();

    /*1.어떠한 키값을 받아서 로드를 해줄거임 */
    //제너릭문법 : 어떤 타입이든 허용함 하지만 그것이 UnityEngine.Object을 상속받아야함. 근데 모든 오브젝트들은 상속을 받음.
    //이 함수에서는 외부로부터 어떠한 타입이든 유연하게 받아오겠다는 밑작업.
    public void LoadAsync<T>(string key, Action<T> callback =null) where T : UnityEngine.Object
    { //결국 이 함수에서 callback이라는거는 너가 리턴을 하여 함수를 마쳤더라도 나중에 찾으면 콜백해줘
        if (_resources.TryGetValue(key, out Object resource)) //로드한적이 있다 (캐시확인)
         //out : 참조값을 받아옴. 사용하게 되면 함수 밖에 있는 변수를 그대로 변경할수있다.
        {
            callback?.Invoke(resource as T); //니가 찾는 리소스 줄게.
            return;
        }

        //로드한적이 없다
        /*2. 원하는 키를 리턴해줌*/var asyncOperation=Addressables.LoadAssetAsync<T>(key); //비동기방식. 가져왔을때 콜백함수를 받아 처리함
        /*//받아왔다면 뭘해줄지 연결할 수 있음*/asyncOperation.Completed += (op) => 
        {//asyncOperation.Completed += (op) => : 비동기 작업이 완료된 작업물(대표적으로 상태와 결과)를 op에 전달받음
            /*3딕셔너리에 add 하겠다. 키값과 결과물인 오브젝트를*/_resources.Add(key, op.Result); //
            /*4*/callback?.Invoke(op.Result); 
            
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

