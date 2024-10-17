using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager //Spawn과 DeSpawn을 관리해주는 매니저
{
    public PlayerController Player { get; private set; } 
    public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
    public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();

    public T Spawn<T>(int templateID = 0) where T : BaseController //오브젝트를 스폰하기
    {
        System.Type type = typeof(T); //무슨타입을 스폰하길 원하는지

        if (type == typeof(PlayerController)) //타입 : 플레이어
        {
            // TODO : Data
            GameObject go = Managers.Resource.Instantiate("James.prefab", pooling: true); //게임오브젝트에 제인스프리팹 넣어주고 풀링했다고 체크 //키값을 전달하여 어드레서블에서 오브젝트를 구해옴
            go.name = "Player"; 

            PlayerController pc = go.GetOrAddComponent<PlayerController>(); //생성한 플레이어에게 컴포넌트 붙혀줌 Get하는이유???
            Player = pc; //기억해둠

            return pc as T;
        }
        else if (type == typeof(MonsterController)) //타입 : 몬스터
        {
            string name = (templateID == 0 ? "BasicMonster" : "WheelMonster"); //랜덤하게 들어온 두 id중 하나를 판별 -> name 뽑아옴
            GameObject go = Managers.Resource.Instantiate(name + ".prefab", pooling: true); //키값을 전달하여 어드레서블에서 오브젝트를 구해옴

            MonsterController mc = go.GetOrAddComponent<MonsterController>(); //컴포넌트 붙혀줌 Get하는이유 ???
            Monsters.Add(mc); //해쉬에 밀어넣어줌

            return mc as T;
        }

        return null;
    }

    public void Despawn<T>(T obj) where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            // ?
        }
        else if (type == typeof(MonsterController))
        {
            Monsters.Remove(obj as MonsterController);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(ProjectileController))
        {
            Projectiles.Remove(obj as ProjectileController);
            Managers.Resource.Destroy(obj.gameObject);
        }
    }
}
