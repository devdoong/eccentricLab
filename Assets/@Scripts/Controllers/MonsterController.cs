// Created on: 2024-10-23
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

    void FixedUpdate()
    {
        #region    ?       ÷  ??    ?            part
        PlayerController pc = Managers.Object.Player;

        if (pc == null)
            return;

        Vector3 dir = pc.transform.position - transform.position;                           
        Vector3 newPos = transform.position + dir.normalized * Time.deltaTime * _speed; 
        GetComponent<Rigidbody2D>().MovePosition(newPos);

        GetComponent<SpriteRenderer>().flipX = dir.x < 0; 
        #endregion
    }
    #region override OnDead()
    protected override void OnDead()
    {
        base.OnDead();

        if (_coDotDamage != null) StopCoroutine(_coDotDamage);
        _coDotDamage = null;

        //           exp gem     
        GemController gc = Managers.Object.Spawn<GemController>(transform.position); //잼은 몬스터 죽은 위치에 그대로 나와야 할 것

        Managers.Object.Despawn(this);
    }
    #endregion

    #region       浹 ?       
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>(); 
        if (target.IsValid() == false) return;
        if (this.IsValid() == false) return;


        if (_coDotDamage != null) StopCoroutine(_coDotDamage);   

        _coDotDamage = StartCoroutine(CostartDotDamage(target));    
    }
    #endregion
    #region
    public void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>();
        if (target.IsValid()==false) return;
        if(this.IsValid() == false) return;


        if (_coDotDamage != null) StopCoroutine(_coDotDamage);

        _coDotDamage = null;
    }
    #endregion
    #region
    Coroutine _coDotDamage;
    public IEnumerator CostartDotDamage(PlayerController target) 
    {
        while (true)
        {
            target.OnDamaged(this, 2);
            yield return new WaitForSeconds(0.1f); //0.1 ?        ?      
        }
    }
    #endregion

    



    

}
