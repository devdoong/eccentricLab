// Created on: 2024-10-22
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


    bool _init = false;
    virtual public bool Init()
    {
        if (_init)
            return false; 

        _init = true;
        return true; 
    }
    //

    void Update()
    {
        UpdateController();
    }

    public virtual void UpdateController()
    {

    }
}
