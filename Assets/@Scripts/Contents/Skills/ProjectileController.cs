using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : SkillController
{
    CreatureController _owner;
    Vector3 _moveDir;
    float _speed = 10.0f;
    float _lifeTime = 10.0f;
    Transform target = null;

    public override bool Init()
    {
        base.Init ();
        StartDestroy(_lifeTime);

        return true;
    }

    public void SetInfo(int templateID, CreatureController owner, Vector3 moveDir)
    { //이 객체를 만든쪽에서 만들어진 객체에 대한 상세 정보를 나타내는 함수
        if (Managers.Data.SkillDic.TryGetValue(templateID, out Data.SkillData data) == false)
        {
            Debug.LogError("ProjectileController SetInfo Failed");
            return;
        }

        _owner = owner;
        _moveDir = moveDir;
        SkillData = data;

        //TODO : Data Parsing
    }

    public override void UpdateController()
    {
        base.UpdateController();

        transform.position +=_moveDir * _speed * Time.deltaTime;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.gameObject.GetComponent<MonsterController>();
        if (mc.IsValid() == false)
            return;

        if(this.IsValid() == false) //이미 풀링된걸 또 풀링하지 않기위해
            return;

        mc.OnDamaged(_owner, SkillData.damage);

        StopDestroy();

        Managers.Object.Despawn(this);
    }

    

}
