using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GameObject _basicMobPrefab;
    public GameObject _wheelMobPrefab;
    public GameObject _jamesPlayerPrefab;
    public GameObject _joyStickPrefab;

    // Start is called before the first frame update

    private GameObject _basicMonster;
    private GameObject _wheelMonster;
    private GameObject _jamesPlayer;
    private GameObject _joyStick;
    void Start()
    {
        _basicMonster = GameObject.Instantiate(_basicMobPrefab); //붕어빵 찍어주세요
        _wheelMonster =  GameObject.Instantiate(_wheelMobPrefab); //실제로 하이러키에 소환.
        _jamesPlayer = GameObject.Instantiate(_jamesPlayerPrefab); 
        _joyStick = GameObject.Instantiate(_joyStickPrefab);

        GameObject go = new GameObject() { name = "@Monsters" }; //@Monsters라는 빈오브젝트를 하이러키에 하나 생성
        //Monsters에다가 앞으로 풀링하여 등장시킬 몬스터들을 넣으려는데
        //계층구조를 가지고 있는 컴포넌트는 transform이다
        _basicMonster.transform.parent = go.transform;
        _wheelMonster.transform.parent = go.transform;
        _jamesPlayer.transform.parent = go.transform;
        //이렇게 @Monsters를 부모로 삼아준다.

        //실행했을때 (Clone) 이 붙는게 마음에 안든다면
        _jamesPlayer.name = _jamesPlayerPrefab.name;
        _basicMonster.name = _basicMobPrefab.name;
        _wheelMonster.name= _wheelMobPrefab.name;
        //이렇게 프리팹의 이름을 가져오면 된다
        _joyStick.name = "@UI_Joystick";

        _jamesPlayer.AddComponent<PlayerController>();

        Camera.main.GetComponent<CameraController>().Target = _jamesPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
