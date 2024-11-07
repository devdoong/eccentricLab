using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProjectileController : SkillController
{
    CreatureController _owner;
    Vector3 _moveDir;
    float _speed = 10.0f;
    float _lifeTime = 10.0f;
    GameObject target;

    public override bool Init()
    {
        base.Init ();
        StartDestroy(_lifeTime);

        return true;
    }

    private void Awake()
    {
        target = PlayerController.closestMonster;
        print("검색결과: " + PlayerController.closestMonster.name);
        Rotate(PlayerController.closestMonster);
    }

    private void Rotate (GameObject target)
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
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

        // target이 null이 아니면 target을 향해 이동
        if (target != null)
        {
            // 목표물 방향을 계산
            Vector3 direction = (target.transform.position - transform.position).normalized;

            // 속도를 적용하여 이동
            transform.position += direction * _speed * Time.deltaTime;
        }
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
