using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Joystick : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    [SerializeField]
    Image _background;
    [SerializeField]
    Image _handler;

    Vector2 _startTouchPosition;
    Vector2 _moveDir; //어떤 방향으로 이동해야하는지

    float _joystickRadius;

    void Start()
    {
        //조이스틱 벡그라운드의 RectTransform에서 sizeDelta(요소의크기) 그중에서 y는 높이 x는 폭일텐데 원이니까 뭘해도 상관없다
        //거기서 /2 로 반지름을 구해줌.
        _joystickRadius = _background.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
    }

    void Update()
    {
        
    }

    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        _background.transform.position = eventData.position; 
        _handler.transform.position = eventData.position;

        _startTouchPosition = eventData.position;

    }

    public void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        _handler.transform.position = _startTouchPosition;
        _moveDir = Vector2.zero;

        Managers.Game.MoveDir = _moveDir;
    }

    public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Vector2 touchDir = (eventData.position - _startTouchPosition); //두점을 뺀걸 벡터로 저장
        //만약 touchDir 벡터가 (3, 4)라고 가정하면, 이 벡터의 크기(길이)는 5입니다 (3^2 + 4^2 = 9 + 16 = 25, 즉 √25 = 5).
        float moveDistance = Mathf.Min(touchDir.magnitude, _joystickRadius); //magnitude=벡터를 거리로 구해줌.//그래서 background의 지름?이랑 이 거리중에서 더 작은거 골라주는거 

        //이 벡터를 normalized하면 크기가 1로 바뀌고 방향만 유지됩니다. 즉, (3, 4)의 단위 벡터는(0.6, 0.8)로 변환됩니다. 이 벡터는 크기는 1이지만, 여전히 같은 방향을 가리키고 있습니다.
        _moveDir = touchDir.normalized; //핸들이 이동할 방향.

        Vector2 newPosition = _startTouchPosition + _moveDir * moveDistance; //방향과 이동할 거리를 곱하고 + 시작점을 더해주면 = 핸들의 위치를 새로 잡음
        _handler.transform.position = newPosition;

        Managers.Game.MoveDir = _moveDir;

    }
}
