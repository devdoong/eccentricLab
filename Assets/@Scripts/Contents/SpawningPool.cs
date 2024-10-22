// Created on: 2024-10-23
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    //������ �ֱ�
    //���� �ִ� ����
    //����
    float _spawnInterval = 0.2f;
    int _maxMonsterCount = 100;
    Coroutine _coUpdateSpawningPool;
    void Start() 
    {
        _coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool()); //�ڷ�ƾ ����
    }

    IEnumerator CoUpdateSpawningPool()
    {
        while(true)
        {
            TrySpawn();
            yield return new WaitForSeconds(_spawnInterval); //2.0�ʸ���
        }
    }

    void TrySpawn()
    {
        int monsterCount = Managers.Object.Monsters.Count; //���� ����� ����
        if(monsterCount >= _maxMonsterCount )
        {
            return; //100���� �̻��̸�
        }


        //TEMP : DATA ID
        Vector3 randPos = new Vector2 (Random.Range(-5,5), Random.Range(-5,5));
        MonsterController mc = Managers.Object.Spawn<MonsterController>(randPos,Random.Range(0,2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
