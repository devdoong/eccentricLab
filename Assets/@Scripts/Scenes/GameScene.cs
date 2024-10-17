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
                StartLoaded2();
            }
        });
    }

    SpawningPool spawning_pool;
    void StartLoaded2()
    {

        spawning_pool = gameObject.AddComponent<SpawningPool>();
         
        var james_player = Managers.Object.Spawn<PlayerController>();

        for (int i = 0; i < 10; ++i)
        {
            MonsterController mc1 = Managers.Object.Spawn<MonsterController>(Random.Range(0, 2));
            mc1.transform.position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            MonsterController mc2 = Managers.Object.Spawn<MonsterController>(Random.Range(0, 2));
            mc2.transform.position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

        }
        MonsterController mc = Managers.Object.Spawn<MonsterController>(Random.Range(0, 2));
        mc.transform.position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

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
