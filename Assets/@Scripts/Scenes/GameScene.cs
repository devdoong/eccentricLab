using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    
    void Start()
    {
        Managers.Resource.LoadAllAsync<GameObject>("Prefabs", (key, count, total) =>
        {
            Debug.Log($"{key}: {count}/{total}");

            if (count == total)
            {
                StartLoaded();
            }
        });
    }

    SpawningPool spawning_pool;
    void StartLoaded()
    {
         
        var james_player = Managers.Object.Spawn<PlayerController>(); //PlayerController 리턴

        spawning_pool = gameObject.AddComponent<SpawningPool>();//Start함수 실행되면서 스포닝 시작

        var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Joystick";

        //var map = Managers.Resource.Instantiate("Map.prefab");
        //map.name = "@Map";
        Camera.main.GetComponent<CameraController>().Target = james_player.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
