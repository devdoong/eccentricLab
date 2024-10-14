using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    

    // Start is called before the first frame update

    private GameObject _basicMonster;
    private GameObject _wheelMonster;
    private GameObject _jamesPlayer;
    private GameObject _joyStick;
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
    void StartLoaded()
    {
        GameObject Player = Managers.Resource.Load<GameObject>("James.prefab");


        GameObject go = new GameObject() { name = "@Monsters" }; //@Monsters라는 빈오브젝트를 하이러키에 하나 생성
        //Monsters에다가 앞으로 풀링하여 등장시킬 몬스터들을 넣으려는데
        //계층구조를 가지고 있는 컴포넌트는 transform이다
        _basicMonster.transform.parent = go.transform;
        //_wheelMonster.transform.parent = go.transform;
        _jamesPlayer.transform.parent = go.transform;
        //이렇게 @Monsters를 부모로 삼아준다.

        //실행했을때 (Clone) 이 붙는게 마음에 안든다면
        //_jamesPlayer.name = _jamesPlayerPrefab.name;
        //_basicMonster.name = _basicMobPrefab.name;
        //_wheelMonster.name = _wheelMobPrefab.name;
        //이렇게 프리팹의 이름을 가져오면 된다
        _joyStick.name = "@UI_Joystick";

        _jamesPlayer.AddComponent<PlayerController>();

        Camera.main.GetComponent<CameraController>().Target = _jamesPlayer;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

  

    //어찌저찌 마무리를 쳐두고
    //gpt api 도우미 npc =>
    //도우미 내가 지금 좀 약해서 보스를 못잡는것같아. <- 나의 레벨, 아이템상황 -> 어 당신은 지금 이스테이지의 보스를 잡기에는 레벨이 부족합니다.
    // 내가 지금 좀 약해서 보스를 못잡는것같아.
    // ai <- 스테이지1에서는 3레벨과 아이템은 전부다 레어등급이 갖춰져야해. 이걸 활용해서 유저ㅗ가 질문하면 답변해줘.
    //+여기저기 널린기능. 
}
