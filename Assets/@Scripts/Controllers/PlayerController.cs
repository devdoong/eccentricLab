using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : CreatureController
{

    Vector2 _moveDir = Vector2.zero;

    float EnvCollectDist { get; set; } = 1.0f;
    
    private PlayerController()
    {
        _speed = 3.0f;
    }

    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set { _moveDir = value.normalized; }
    }

    
    void Start()
    {
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged; //●4.구독
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

    }



    void MovePlayer()
    {
        //_moveDir = Managers.Game.MoveDir;

        Vector3 dir = _moveDir * _speed * Time.deltaTime;
        transform.position += dir;
        Debug.Log(dir);
        if (dir.x < 0) GetComponent<SpriteRenderer>().flipX = true;
        else if (dir.x > 0) GetComponent<SpriteRenderer>().flipX = false;
        
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

}
