using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Vector2 _moveDir = Vector2.zero;
    float _speed = 5.0f;
    // Start is called before the first frame upd
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        MovePlayer();

    }

    //Device Simulator ø°º± ∏‘≈Î
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
        Vector3 dir = _moveDir * _speed * Time.deltaTime;
        transform.position += dir;
    }
}
