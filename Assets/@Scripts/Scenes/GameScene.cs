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
                Managers.Resource.LoadAllAsync<TextAsset>("Data", (key3, count3, total3) =>
                {
                    if (count3 == total3)
                    {
                        StartLoaded();
                    }
                });
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

        //Data Test
        Managers.Data.Init();
        foreach(var playerData in Managers.Data.PlayerDic.Values)
        {
            Debug.Log($"Level : {playerData.level}, HP : {playerData.maxHp}");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
