using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    float DamagedCool = 2;
    bool CanDamaged;
    int IsAttack;               // 공격 상태값
    int AttackPattern;
    int IsMove;                 // 움직임 상태값
    public float MaxHP = 1000;
    public float HP = 1000;              // HP
    [SerializeField]
    SpriteRenderer HPgage;
    [SerializeField]
    GameObject AttackN;
    [SerializeField]
    GameObject AttackP;

    float Speed = 1.5f;         // 몬스터 이동속도
    MonsterState State;         // 몬스터 상태값
    bool IsDying = false;
    
    float RayDist = 10.0f;       // 몬스터 인식범위

    Vector2 dir = Vector2.right;    // 레이캐스트용 벡터

    int DropExp;                // 주는 경험치

    [SerializeField]
    GameObject DropItem_Prefab;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        StartCoroutine(MoveTime());
        State = MonsterState.IDLE;
        DropExp = 400;
    }
    private void Update()
    {
        Debug.Log(State);
        HPControl();

        if(HP <= 0)
        {
            State = MonsterState.Die;
        }
        switch (State)
        {
            case MonsterState.IDLE:
                {
                    MoveState();
                    RayCast();
                }
                break;
            case MonsterState.Trace:
                {
                    Trace();
                }
                break;
            case MonsterState.Damaged:
                {

                }
                break;
            case MonsterState.Die:
                {
                    if(IsDying == false)
                    StartCoroutine(DieMotion());
                }
                break;
            default:
                break;
        }

    }

    enum MonsterState
    {
        IDLE,
        Trace,
        Damaged,
        Die,
    }


    void MoveState()
    {
        switch (IsMove)
        {
            case 0:
                {
                    animator.SetInteger("IsWalk", 0);
                    rigid.velocity -= new Vector2(rigid.velocity.x, rigid.velocity.y);
                }
                break;
            case 1:
                {
                    Move(-1);
                }
                break;
            case 2:
                {
                    Move(1);
                }
                break;
        }
    }

    void ExpDrop()
    {
        GameMgr.GetInstance().PDropEXP = DropExp;
        GameMgr.GetInstance().PIsKilled = true;
    }

    IEnumerator MoveTime()
    {
        yield return new WaitForSeconds(3.0f);
        IsMove = Random.Range(0, 3); // 0 idle 1 left 2 right
        StartCoroutine(MoveTime());
    }
    IEnumerator NormalAttack()
    {
        IsAttack = 1;
        yield return new WaitForSeconds(1.0f);
        AttackN.SetActive(true);
        animator.SetInteger("IsAttack", 2);
        yield return new WaitForSeconds(0.2f);
        IsAttack = 0;
        AttackN.SetActive(false);
        animator.SetInteger("IsAttack", 0);
    }

    IEnumerator PowerAttack()
    {
        IsAttack = 1;
        yield return new WaitForSeconds(3.0f);
        AttackP.SetActive(true);
        animator.SetInteger("IsAttack", 2);
        yield return new WaitForSeconds(0.2f);
        IsAttack = 0;
        AttackP.SetActive(false);
        animator.SetInteger("IsAttack", 0);
    }

    IEnumerator DieMotion()
    {
        IsDying = true;
        animator.SetInteger("IsDie", 1);
        yield return new WaitForSeconds(0.5f);
        ExpDrop();
        Instantiate(DropItem_Prefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        Destroy(gameObject);
    }
    IEnumerator DamagedCoolDown()
    {
        CanDamaged = false;

        int Count = 0;
        while (Count < 10)
        {

            if (Count % 2 == 0)
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                spriteRenderer.color = new Color32(255, 255, 255, 180);

            yield return new WaitForSeconds(DamagedCool / 10);
            Count++;
        }

        spriteRenderer.color = new Color32(255, 255, 255, 255);

        CanDamaged = true;

    }

    void Damaged()
    {
        if (CanDamaged)
        {
            StartCoroutine(DamagedCoolDown());
            HP -= GameMgr.GetInstance().PAttackDamage;
        }
    }
    void HPControl() // 체력 바
    {
        if (HP > 0)
        {
            HPgage.transform.position = new Vector3((100 - HP / MaxHP * 100) * -0.6975f / 100, 2) + this.transform.position;
            HPgage.transform.localScale = new Vector3(0.5f * (HP / MaxHP), 0.1f, 1);
        }
        else
        {
            HPgage.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("AttackBox") && State != MonsterState.Damaged)
        {
            if(collision.CompareTag("Player"))
            Damaged();
        }
    }
    void Move(int _i /* -1 left 1 right*/)
    {
        rigid.velocity -= new Vector2(rigid.velocity.x, 0);
        animator.SetInteger("IsWalk", 1);
        rigid.velocity += new Vector2(Speed * _i, 0);
        if (_i == -1)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    void Attack(float _f /* -1 Left 1 Right */)
    {
        if (IsAttack == 0)
        {
            if (AttackPattern == 2)
            {
                AttackPattern = 0;
                StartCoroutine(PowerAttack());
                animator.SetInteger("IsAttack", 1);
            }
            else
            {
                AttackPattern++;
                StartCoroutine(NormalAttack());
                animator.SetInteger("IsAttack", 1);
            }
        }
    }

    void RayCast()
    {
        Vector2 MosterPos = new Vector2(transform.position.x, transform.position.y);

        Debug.DrawRay(MosterPos - new Vector2(RayDist / 2, -0.5f),dir * RayDist, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(MosterPos - new Vector2(RayDist / 2 , - 0.5f), dir, RayDist, LayerMask.GetMask("Player"));
        if (hit.collider != null && hit.transform.CompareTag("Player"))
        {
            
            State = MonsterState.Trace;
            Debug.Log(hit);
        }
    }
    void Trace()
    {
        if(Vector3.Distance(transform.position, GameMgr.GetInstance().PPlayerPos) < RayDist * 2)
        {
            if (Vector3.Distance(transform.position, GameMgr.GetInstance().PPlayerPos) > 2.0f && IsAttack == 0)
            {
                if (GameMgr.GetInstance().PPlayerPos.x > transform.position.x)
                {
                    Move(1);
                }

                if (GameMgr.GetInstance().PPlayerPos.x < transform.position.x)
                {
                    Move(-1);
                }
            }
            if(Vector3.Distance(transform.position, GameMgr.GetInstance().PPlayerPos) < 2.0f)
            {
                if (GameMgr.GetInstance().PPlayerPos.x > transform.position.x)
                {
                    Attack(1);
                }

                if (GameMgr.GetInstance().PPlayerPos.x < transform.position.x)
                {
                    Attack(-1);
                }
            }
        }
        else
        {
            State = MonsterState.IDLE;
        }
    }
}
