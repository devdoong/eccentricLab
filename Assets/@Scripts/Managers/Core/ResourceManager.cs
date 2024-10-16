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

        //풀리ㅇ
        if(pooling)
        {

        }

        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null) return;


        Object.Destroy(go);
    }


    #region addressable
    Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();


    /*1.어떠한 키값을 받아서 로드를 해줄거임 */
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    { //결국 이 함수에서 callback이라는거는 너가 리턴을 하여 함수를 마쳤더라도 나중에 찾으면 콜백해줘
        if (_resources.TryGetValue(key, out Object resource)) //로드한적이 있다 (캐시확인)
                                                              //out : 참조값을 받아옴. 사용하게 되면 함수 밖에 있는 변수를 그대로 변경할수있다.
        {
            callback?.Invoke(resource as T); //니가 찾는 리소스 줄게.
            return;
        }


        var asyncOperation=Addressables.LoadAssetAsync<T>(key); //비동기방식. 가져왔을때 콜백함수를 받아 처리함
        asyncOperation.Completed += (op) => 
        {
            _resources.Add(key, op.Result);
            callback?.Invoke(op.Result); 
            
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

