using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    //리스폰 주기
    //몬스터 최대 개수
    //스톱
    float _spawnInterval = 0.2f;
    int _maxMonsterCount = 100;
    Coroutine _coUpdateSpawningPool;
    void Start() 
    {
        _coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool()); //코루틴 시작
    }

    IEnumerator CoUpdateSpawningPool()
    {
        while(true)
        {
            TrySpawn();
            yield return new WaitForSeconds(_spawnInterval); //2.0초마다
        }
    }

    void TrySpawn()
    {
        int monsterCount = Managers.Object.Monsters.Count; //현재 찍어준 몬스터
        if(monsterCount >= _maxMonsterCount )
        {
            return; //100마리 이상이면
        }


        //TEMP : DATA ID
        MonsterController mc = Managers.Object.Spawn<MonsterController>(Random.Range(0,2));
        mc.transform.position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
