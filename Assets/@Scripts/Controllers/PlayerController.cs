using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Vector2 _moveDir = Vector2.zero;
    float _speed = 5.0f;

    //방법1-3 _moveDir을 외부에서도 (조이스틱에서도) 값을 변경할 수 있게 만들어줘야할것임 => C#프로퍼티를 사용
    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set { _moveDir = value.normalized; }
    }

    
    // Start is called before the first frame upd
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateInput();
        MovePlayer();

    }

    //Device Simulator 에선 먹통
    void UpdateInput()
    {
        Vector2 moveDir = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            moveDir.y += 1;
        if (Input.GetKey(KeyCode.S))
            moveDir.y -= 1;
        if (Input.GetKey(KeyCode.A))
            moveDir.x -= 1;
        if (Input.GetKey(KeyCode.D))
            moveDir.x += 1;

        _moveDir = moveDir.normalized;

    }

    void MovePlayer()
    {
        _moveDir = Managers.MoveDir; //방법 2-3
        Vector3 dir = _moveDir * _speed * Time.deltaTime;
        transform.position += dir;
    }
}
