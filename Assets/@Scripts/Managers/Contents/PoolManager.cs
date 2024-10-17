using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

class Pool
{
    GameObject _prefab; //원본 프리팹 담아줌
    IObjectPool<GameObject> _pool; //unity pooling interface

    Transform _root; //찐 root //하이러키에 형체가 보여질 _root
    Transform Root //get할 Root
    {
        get
        {
            if (_root == null) //root 형체 없으면
            {
                GameObject go = new GameObject() { name = $"{_prefab.name}Root" }; //오브젝트에 이름하나 붙혀서 생성
                _root = go.transform; //_root에 박아줌
                
            }

            return _root;
        }
    }

    public Pool(GameObject prefab) //풀링해줄테니 일단 오브젝트부터 내놔라.
    {
        _prefab = prefab; //오브젝트에 넣어주고
        _pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy); //최소한 create해주는 func는 만들어야함
        
    }
    GameObject OnCreate() //원본 찍어냄
    {
        GameObject go = GameObject.Instantiate(_prefab);
        go.name = _prefab.name; //지저분한 clone 지우기
        go.transform.parent = Root; //Root의 자식으로 들어가기
        return go;
    }
    void OnGet(GameObject go)
    {
        go.SetActive(true);
    }
    void OnRelease(GameObject go)
    {
        go.SetActive(false);
    }
    void OnDestroy(GameObject go)
    {
        Managers.Resource.Destroy(go);
        //GameObject.Destroy(go);
    }
    public void Push(GameObject go)
    {
        _pool.Release(go); 
    }
    public GameObject Pop()
    {
        return _pool.Get();
    }
}
public class PoolManager 
{
    Dictionary<string, Pool> _pools = new Dictionary<string, Pool>(); //키밸류 pools
    
    public GameObject Pop(GameObject prefab)
    {
        if (_pools.ContainsKey(prefab.name) == false) //키값 아예 없으면
            CreatePool(prefab);//
        return _pools[prefab.name].Pop();
    }

    public bool Push(GameObject go)
    {
        if(_pools.ContainsKey(go.name)==false) 
            return false;

        _pools[go.name].Push(go);
        return true;
    }

    void CreatePool(GameObject prefab)
    {
        Pool pool = new Pool(prefab);//풀객체에 프리팹 넣어줌 생성자를 살펴봐야함 //알아서 OnCreate된다.
        _pools.Add(prefab.name, pool);  //딕셔너리에 Add
    }
}
