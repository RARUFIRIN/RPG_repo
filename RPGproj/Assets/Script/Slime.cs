using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    int IsAttack;               // 공격 상태값
    int IsMove;                 // 움직임 상태값
    public float MaxHP = 100;
    public float HP = 100;              // HP
    [SerializeField]
    SpriteRenderer HPgage;

    float Speed = 1.5f;         // 몬스터 이동속도
    MonsterState State;         // 몬스터 상태값
    bool IsGround = true;
    bool IsDying = false;

    float jump_pre;             // 위치값으로 추락 애니메이션 조절
    float jump_col;
    
    float RayDist = 5.0f;       // 몬스터 인식범위

    Vector2 dir = Vector2.right;    // 레이캐스트용 벡터

    int DropExp;                // 주는 경험치

    [SerializeField]
    GameObject DropItem_Prefab;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        jump_pre = gameObject.transform.position.y;
        jump_col = gameObject.transform.position.y;
    }
    private void Start()
    {
        StartCoroutine(MoveTime());
        State = MonsterState.IDLE;
        DropExp = 20;
    }
    private void Update()
    {
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
                    JumpState();
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

    void JumpState() // 점프 > 하강
    {
        jump_pre = jump_col;
        jump_col = gameObject.transform.position.y;
        if (IsGround == false && jump_pre - jump_col > 0.01)
        {
            animator.SetInteger("IsAttack", 2);
        }

        // 점프 시 속도 제한
        if (rigid.velocity.y > 11.0f)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 11.0f);
        }
    }

    IEnumerator MoveTime()
    {
        yield return new WaitForSeconds(3.0f);
        IsMove = Random.Range(0, 3); // 0 idle 1 left 2 right
        StartCoroutine(MoveTime());
    }
    IEnumerator AttackTime()
    {
        IsAttack = 1;
        yield return new WaitForSeconds(3.0f);
        IsAttack = 0;
    }
    
    IEnumerator ChangeState(float _f, MonsterState _s) // 상태변화 딜레이
    {
        yield return new WaitForSeconds(_f);
        State = _s;
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


    void Damaged(int Dmg)
    {
        State = MonsterState.Damaged;
        animator.SetInteger("IsWalk", 0);
        animator.SetInteger("IsAttack", 0);
        rigid.velocity = Vector3.zero;
        if (GameMgr.GetInstance().PPlayerPos.x < transform.position.x)
        {
            rigid.AddForce(new Vector2(40, 100));
        }
        else
        {
            rigid.AddForce(new Vector2(-40, 100));
        }
        HP -= Dmg;
        StartCoroutine(ChangeState(0.5f, MonsterState.Trace));
    }
    void HPControl() // 체력 바
    {
        if (HP > 0)
        {
            HPgage.transform.position = new Vector3((100 - HP / MaxHP * 100) * -0.6975f / 100, 1) + this.transform.position;
            HPgage.transform.localScale = new Vector3(0.5f * (HP / MaxHP), 0.1f, 1);
        }
        else
        {
            HPgage.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("AttackBox") && State != MonsterState.Damaged)
        {
            Damaged(SkillMgr.GetInstance().returnDmg(collision.tag));
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
            StartCoroutine(AttackTime());
            rigid.AddForce(new Vector2(70 * _f, 200));
            IsGround = false;
            animator.SetInteger("IsAttack", 1);
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
            if (Vector3.Distance(transform.position, GameMgr.GetInstance().PPlayerPos) > 2.5f && IsAttack != 1)
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
            if(Vector3.Distance(transform.position, GameMgr.GetInstance().PPlayerPos) < RayDist / 2)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground") && (IsAttack != 0 || State == MonsterState.Damaged))
        {
            IsAttack = 2;
            IsGround = true;
            animator.SetInteger("IsAttack", 0);

            rigid.velocity = Vector3.zero; // 바닥에 닿으면 정지
        }
    }
}
