using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : CreatureController
{
    public override bool Init()
    {
        if (base.Init()) //이미 초기 설정을 해줬다면?
            return false;

        Object_Type = Define.ObjectType.Monster; //나의 타입을 베이스에 기록해둠
        return true;//초기작업 완료
    }

    void FixedUpdate()
    {
        #region 몬스터가 계속 플레이어에게 다가가게 만드는 part
        PlayerController pc = Managers.Object.Player;

        if (pc == null)
            return;

        Vector3 dir = pc.transform.position - transform.position; //타겟위치 - 나의 위치  = 상대방으로 가기 위한 방향이 나옴
        Vector3 newPos = transform.position + dir.normalized * Time.deltaTime * _speed; //나의 위치 + 방향을 정규화 * 시간 * 스피드
        GetComponent<Rigidbody2D>().MovePosition(newPos);

        GetComponent<SpriteRenderer>().flipX = dir.x < 0; //왼쪽을보냐 마냐. 뒤집어주기
        #endregion
    }

    Coroutine _coDotDamage;

    private void OnCollisionEnter2D(Collision2D collision) //몬스터가 충돌이 발생했는데
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>(); //부딪힌 대상이 플레이어이길 바라며 PlayerController를 불러와봤는데
        if (target == null) return;

        //들고와진다면 + 
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
