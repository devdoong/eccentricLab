using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using static Define;

public class BaseController : MonoBehaviour
{

    public ObjectType Object_Type {  get; protected set; }

    
    void Awake()
    {
        Init();
    }


    //자식들이 override하여 초기화 작업을 하였는지를 확인해 줄 수 있음. //ui작업때 유용할 수 있음.
    bool _init = false;

    virtual public bool Init()
    {
        if (_init)
            return false; //이미 초기화 했다면 false 리턴

        _init = true;
        return true; 
    }
    //
}
