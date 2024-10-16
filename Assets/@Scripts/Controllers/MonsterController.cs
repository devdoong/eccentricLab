using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : CreatureController
{
    public override bool Init()
    {
        if (base.Init())
            return false;

        Object_Type = Define.ObjectType.Monster;
        return true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerController pc = Managers.Object.Player;

        if (pc == null)
            return;

        Vector3 dir = pc.transform.position - transform.position; //타겟위치 - 나의 위치  = 상대방으로 가기 위한 방향이 나옴
        Vector3 newPos = transform.position + dir.normalized * Time.deltaTime * _speed; //나의 위치 + 방향을 정규화 * 시간 * 스피드
        GetComponent<Rigidbody2D>().MovePosition(newPos);

        GetComponent<SpriteRenderer>().flipX = dir.x < 0; //왼쪽을보냐 마냐. 뒤집어주기
    }



    private void OnCollisionEnter2D(Collision2D collision) //충돌 발생하면
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>(); //이게 정상적으로 들고 와진다면
        if (target == null) return; //안들고 와진다면
        
        if(_coDotDamage != null) StopCoroutine(_coDotDamage); //해당 코루틴 정지

        _coDotDamage = StartCoroutine(CostartDotDamage(target));
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>();
        if (target == null) return;

        if(_coDotDamage != null) StopCoroutine(_coDotDamage);

        _coDotDamage = null;
    }

    Coroutine _coDotDamage;

    public IEnumerator CostartDotDamage(PlayerController target)
    {
        while (true)
        {
            target.OnDamaged(this, 2);
            yield return new WaitForSeconds(0.1f);
        }
    }



    protected override void OnDead()
    {
        base.OnDead();

        if (_coDotDamage != null) StopCoroutine(_coDotDamage);
        _coDotDamage = null;

        Managers.Object.Despawn(this);
    }
}
