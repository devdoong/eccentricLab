using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    
    static Managers s_instance;
    static bool s_init = false;


    //매니저들의 허브가 완성된다.
    //아래 처럼 해줬을때 어떠한 스크립트에서 ResourceManager ResourceManager = Managers.Resource.~~~ 으로 모두 접근가능하다.
    #region Contents
    GameManager _game = new GameManager();
    ObjectManager _object = new ObjectManager();
    PoolManager _pool = new PoolManager();
    public static GameManager Game { get { return Instance?._game; } } 
    public static ObjectManager Object {  get { return Instance?._object; } }
    public static PoolManager Pool {  get  { return Instance?._pool; } }
    #endregion

    #region Core
    DataManager _data = new DataManager();
    ResourceManager _resource = new ResourceManager();
    SceneManager _scene = new SceneManager();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();

    public static DataManager Data {  get { return Instance?._data; } }
    public static ResourceManager Resource {  get { return Instance?._resource; } }
    public static SceneManager Scene {  get { return Instance?._scene; } }
    public static SoundManager Sound { get { return Instance?._sound; } }
    public static UIManager UI {  get { return Instance?._ui; } }

    #endregion

    public static Managers Instance //프로퍼티 문법
    {
        get
        {
            if (s_init == false) //처음에 한번만 만들어주기 위함
            {
                s_init = true;//없으면 true

                GameObject go = GameObject.Find("@Managers"); //@Managers라는 게임 오브젝트를 찾아줌(하이러키에서 찾아줄것이다)
                if (go == null) //존재하지 않는다면.
                {
                    go = new GameObject { name = "@Managers" }; //@Managers라는 오브젝트를 하나 생성해주고
                    go.AddComponent<Managers>(); //Managers라는 스크립트 컴포넌트(현재 스크립트)를 붙혀준다.
                }

                DontDestroyOnLoad(go); //잘못건드는일이 없도록 보호작업(씬끼리 이동하게되면 삭제되는 경우를 방지하기 위함.)
                s_instance = go.GetComponent<Managers>(); //@Managers오브젝트가 이제 생성이 됐으면 그 오브젝트에 달린 Managers 스크립트 컴포넌트를 s_instance에 입혀줌
            }

            return s_instance; //Managers 컴포넌트 스크립트를 불러올수 있게되었음.

            //★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★
            //이렇게하면 언제 어디서든 Mangers.Game 또는 Managers.Resource를 해주게되면
            //Managers.Resource 까지는 s_instance를 return 받게될것이다.
            //s_instance의 리턴값은 Managers class(스크립트)로 봐도 무방하고
            //이어서 Managers.Resource._resource 즉 _resource는 ResourceManager.cs를 까볼 수 있도록 해주는것
            //말로 쭉 한방에 설명하면
            //Managers 스크립트에 들어가면 @Manager 오브젝트에 딸린 컴포넌트인 Manager.cs 컴포넌트를 참조하여
            //_resource에 접근하면 ResourceManager에 들어있는 데이터들을 만질수 있다.
            //그냥 더 쉽게해서 Manager들어가서 -> ResouceManager에 있는 데이터를 직접 참조할 수 있다.
        }
    }
}
