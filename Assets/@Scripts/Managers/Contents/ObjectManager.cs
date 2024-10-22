// Created on: 2024-10-23
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager //Spawn�� DeSpawn�� �������ִ� �Ŵ���
{
    public PlayerController Player { get; private set; } 
    public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
    public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();
    public HashSet<GemController> Gems { get; } = new HashSet<GemController>();

    public T Spawn<T>(Vector3 position,int templateID = 0) where T : BaseController //������Ʈ�� �����ϱ�
    {
        System.Type type = typeof(T); //����Ÿ���� �����ϱ� ���ϴ���
        #region �÷��̾� ����
        if (type == typeof(PlayerController)) //Ÿ�� : �÷��̾�
        {
            // TODO : Data
            GameObject go = Managers.Resource.Instantiate("James.prefab", pooling: true); //���ӿ�����Ʈ�� ���ӽ������� �־��ְ� Ǯ���� ���� �����ΰ� üũ //Ű���� �����Ͽ� ��巹������� ������Ʈ�� ���ؿ�
            go.name = "Player"; 
            go.transform.position = position;

            PlayerController pc = go.GetOrAddComponent<PlayerController>(); //������ �÷��̾�� ������Ʈ ������ Get�ϴ�����???
            Player = pc; //����ص�

            return pc as T;
        }

        #endregion
        #region ���� ����
        else if (type == typeof(MonsterController)) //Ÿ�� : ����
        {
            string name = (templateID == 0 ? "BasicMonster" : "WheelMonster"); //�����ϰ� ���� �� id�� �ϳ��� �Ǻ� -> name �̾ƿ�
            GameObject go = Managers.Resource.Instantiate(name + ".prefab", pooling: true); //Ű���� �����Ͽ� ��巹������� ������Ʈ�� ���ؿ�
            go.transform.position = position;

            MonsterController mc = go.GetOrAddComponent<MonsterController>(); //������Ʈ ������ Get�ϴ����� ???
            Monsters.Add(mc); //�ؽ��� �о�־���

            return mc as T;
        }

        else if (type == typeof(GemController))
        {
            GameObject go = Managers.Resource.Instantiate(Define.EXP_GEM_PREFAB, pooling: true);
            go.transform.position = position;

            GemController gc = go.GetOrAddComponent<GemController>();
            Gems.Add(gc);
            

            return gc as T; 
        }
        #endregion
        return null;
    }

    public void Despawn<T>(T obj) where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            // ?
        }
        else if (type == typeof(MonsterController)) //���Ͱ� �����Ÿ�
        {
            Monsters.Remove(obj as MonsterController);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(ProjectileController))
        {
            Projectiles.Remove(obj as ProjectileController);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(GemController))
        {
            Gems.Remove(obj as GemController);
            Managers.Resource.Destroy(obj.gameObject);
        }
    }
}
