using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Target;
    void Start()
    {
    }

    void LateUpdate() //다른 Update 즉 다른 오브젝틑들이 모두 Update를 거치고나서 카메라가 찍워줘야함
    {
        if (Target == null)
            return;
        transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, -10);
    }
}
