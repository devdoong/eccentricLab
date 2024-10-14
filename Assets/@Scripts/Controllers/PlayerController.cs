using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Vector2 _moveDir = Vector2.zero;
    float _speed = 5.0f;

    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set { _moveDir = value.normalized; }
    }

    
    void Start()
    {
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged; //●4.구독
    }

    void OnDestroy()
    {
        if(Managers.Game != null)
        {
            Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        }
    }

    void HandleOnMoveDirChanged(Vector2 dir) //●5. 구독한 함수를 보면 dir 파라미터에 Invoke(_moveDir)으로 받은 값이 들어갈것
    {
        _moveDir = dir;
    }

    void Update()
    {
        MovePlayer();

    }



    void MovePlayer()
    {
        //_moveDir = Managers.Game.MoveDir;

        Vector3 dir = _moveDir * _speed * Time.deltaTime;
        transform.position += dir;
    }
}
