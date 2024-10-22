using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public PlayerController Player { get { return Managers.Object?.Player; } }
    

    #region 재화
    public int Gold { get; set; }
    public int Gem { get; set; }
    #endregion


    #region 플레이어 이동
    Vector2 _moveDir;
    public event Action<Vector2> OnMoveDirChanged; //구독 신청 받습니다.
                                                   //Action<Vector2> = void를 리턴 파라미터는 Vector2를 받음 (델리게이트라 생각하면 된다.)
    public Vector2 MoveDir //UI_JoyStick에서 조이스틱 드래그가 발생 할 때 마다 호출
    {
        get { return _moveDir; }
        set { 
            _moveDir = value; //●2.value값을 넣어준다
            OnMoveDirChanged?.Invoke(_moveDir); 
            //●3.구독자들에게 새로 등록된 _movedir(방향)을 리턴해줌 즉 구독자들의 함수의 파라미터에 _moveDir을 준다.
            //정확히 OnMoveDirChanged로 구독자들의 함수를 실행시켜주고 -> 너러블을 이용해 _moveDir을 구독자들에게 전달한다.
            //OnMoveDirChanged로 구독 함수들을 실행시키고 구독자들에게 _moveDir값을 전달함
            //[goto PlayerConroller]
            
        }
    }
    #endregion
}
