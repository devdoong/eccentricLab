using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : CreatureController
{

    Vector2 _moveDir = Vector2.zero;
    private GameObject weaponObject;
    static public GameObject closestMonster;

    float EnvCollectDist { get; set; } = 1.0f;

    public void Start()
    {
        weaponObject = Utils.FindChildWithTag(transform, "Weapon");
    }


    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set { _moveDir = value.normalized; }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _speed = 4.0f;
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged; //●4.구독

        StartProjectile(); //시작하면 코루틴 호출하면서 무한 뺑뱅 돌면서 총알을 쏴줄것이다.

        return true;
    }

    void OnDestroy()
    {
        if(Managers.Game != null)
        {
            Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        }
    }

    void HandleOnMoveDirChanged(Vector2 dir) //●5. 구독한 함수를 보면 dir 파라미터에 Invoke(_moveDir)으로 받은 값이 들어갈것
    {
        _moveDir = dir;
    }

    void Update()
    {
        MovePlayer();
        CollectEnv();
        closestMonster = FindClosestMonster();
        ChangeMonsterColor(closestMonster, Color.red);

    }

    void MovePlayer()
    {
        //_moveDir = Managers.Game.MoveDir;

        Vector3 dir = _moveDir * _speed * Time.deltaTime;
        transform.position += dir;
        Debug.Log(dir);
        if (dir.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            weaponObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (dir.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            weaponObject.GetComponent<SpriteRenderer>().flipX = false;
        }

            /*if (dir.x < 0)
            {
                flipX = true;  // 왼쪽으로 이동 시 스프라이트를 플립
            }
            else if (dir.x > 0)
            {
                _spriteRenderer.flipX = false; // 오른쪽으로 이동 시 원래 방향
            }*/
        }

        void CollectEnv()
    {
        float sqrCollectDist = EnvCollectDist * EnvCollectDist; //제곱

        List<GemController> gems = Managers.Object.Gems.ToList();
        foreach (GemController gem in gems)
        {
            Vector3 dir = gem.transform.position - transform.position;
            if (dir.sqrMagnitude <= EnvCollectDist)
            {
                Managers.Game.Gem += 1;
                Managers.Object.Despawn(gem);
            }
        }

        var findGems = GameObject.Find("@Grid").GetComponent<GridController>().GatherObjects(transform.position, EnvCollectDist + 0.5f);
        Debug.Log($"SearchGems({findGems.Count}) TotalGems({gems.Count})");
    }

    public override void OnDamaged(BaseController attacker, int damage)
    {
        base.OnDamaged(attacker, damage);

        Debug.Log($"체력 : {HP} / {MaxHP}");

        //TEMP
        CreatureController cc = attacker as CreatureController; //attacker를 크리쳐로 형변환 해주지만 그게 실패하면 null로 반환함
        cc?.OnDamaged(this, 10000); //가시
    }

    #region FireBullet
    Coroutine _coFireBullet;

    void StartProjectile()
    {
        if( _coFireBullet != null )
            StopCoroutine(_coFireBullet);

        _coFireBullet = StartCoroutine(CoStartProjectile());
    }

    IEnumerator CoStartProjectile()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f); 

        while (true)
        {
            ProjectileController pc = Managers.Object.Spawn<ProjectileController>(transform.position,1);
            pc.SetInfo(1, this, _moveDir);

            yield return wait;
        }
    }
    #endregion

    public GameObject FindClosestMonster()
    {
        GameObject closestMonster = null;
        float closestDistanceSqr = Mathf.Infinity;

        // 모든 "Monster" 태그를 가진 오브젝트를 배열로 가져옵니다.
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            // 활성화된 오브젝트만 검사합니다.
            if (monster.activeInHierarchy)
            {
                float distanceSqr = (monster.transform.position - transform.position).sqrMagnitude;

                // 더 가까운 오브젝트가 있으면 갱신
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestMonster = monster;
                }
            }
        }

        return closestMonster;
    }

    void ChangeMonsterColor(GameObject monster, Color color)
    {
        Renderer renderer = monster.GetComponent<Renderer>();

        // Renderer 컴포넌트가 있을 때만 색상을 변경합니다.
        if (renderer != null)
        {
            renderer.material.color = color;
        }
        else
        {
            Debug.LogWarning("The closest monster does not have a Renderer component.");
        }
    }
}
