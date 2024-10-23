// Created on: 2024-10-23
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    
    void Start()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, total) =>
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
        Managers.Data.Init();

        spawning_pool = gameObject.AddComponent<SpawningPool>();//Start�Լ� ����Ǹ鼭 ������ ����

        var james_player = Managers.Object.Spawn<PlayerController>(Vector3.zero); //PlayerController ����


        var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Joystick";

        //var map = Managers.Resource.Instantiate("Map.prefab");
        //map.name = "@Map";

        Camera.main.GetComponent<CameraController>().Target = james_player.gameObject;

        //Data Test
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
