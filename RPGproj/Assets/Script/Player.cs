using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject Attack_Box_Prefab;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    Animator animator;
    // jumpstate
    float jump_pre;
    float jump_col;

    float Attack_Speed = 0.25f;
    int FlipX;

    float DamagedCool = 2;
    public bool  CanDamaged = true;

    public int CanAttack;

    GameObject ScanedObj;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jump_pre = gameObject.transform.position.y;
        jump_col = gameObject.transform.position.y;
    }

    private void Start()
    {
        GameMgr.GetInstance().PAttackDamage = 20;
        
    }
    void Update()
    {
        GameMgr.GetInstance().PPlayerPos = transform.position;
        if (!QuestMgr.GetInstance().GetIsAction())
        {
            Move();
            Attack();
        }
        RayCast();
        JumpState();
    }

    void Move()
    {

        // 오른쪽 움직임
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigid.velocity += new Vector2(5 * GameMgr.GetInstance().PSpeed, 0);
            if (!Input.GetKey(KeyCode.LeftArrow))
            {
                spriteRenderer.flipX = false;
                FlipX = 1;
            }
            animator.SetInteger("IsWalking", 1);
        }
        // 왼쪽 움직임
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            rigid.velocity += new Vector2( -5 * GameMgr.GetInstance().PSpeed, 0);
            if (!Input.GetKey(KeyCode.RightArrow))
            {
                spriteRenderer.flipX = true;
                FlipX = -1;
            }
            animator.SetInteger("IsWalking", 1);
        }
        // 좌우 이동 애니메이션 예외처리
        if ((!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)))
        {
            animator.SetInteger("IsWalking", 0);
            rigid.velocity -= new Vector2(rigid.velocity.x, 0);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
        
        // 아래 엎드리기
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (GameMgr.GetInstance().PIsGround == true)
            {
                animator.SetInteger("IsDown", 1);
            }
        }
        else if (!Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetInteger("IsDown", 0);
        }
     

        if(Input.GetKey(KeyCode.G))
        {
            GameMgr.GetInstance().PTryInter = true;
        }
        else if(Input.GetKeyUp(KeyCode.G))
        {
            GameMgr.GetInstance().PTryInter = false;
        }


        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && GameMgr.GetInstance().PIsJump == 0)
        {
            rigid.AddForce(new Vector2(0, GameMgr.GetInstance().PJump));
            animator.SetInteger("IsJump", 1);
            GameMgr.GetInstance().PIsJump = 1;
        }

        RimitSpeed();
    }

    void JumpState() // 점프 > 하강
    {
        jump_pre = jump_col;
        jump_col = gameObject.transform.position.y;
        if(GameMgr.GetInstance().PIsGround == false && jump_pre - jump_col > 0.01)
        {
            animator.SetInteger("IsJump", 2);
            GameMgr.GetInstance().PIsJump = 2;
        }

        // 점프 시 속도 제한
        if(rigid.velocity.y > 11.0f)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 11.0f);
        }
    }

    void Attack()
    {
        if (CanAttack == 0 && Input.GetKeyDown(KeyCode.A))
        {
            SkillMgr.GetInstance().SetSkill(0.5f, new Vector2(0, 0));

            if (spriteRenderer.flipX == false)
                Instantiate(Attack_Box_Prefab, gameObject.transform.position + new Vector3(0.7f, 0.5f, 0), Quaternion.identity);
            else
                Instantiate(Attack_Box_Prefab, gameObject.transform.position + new Vector3(-0.7f, 0.5f, 0), Quaternion.identity);

            animator.SetInteger("IsAttack", 1);
            CanAttack = 1;
            StartCoroutine(Attackable());
        }
    }

    IEnumerator Attackable()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetInteger("IsAttack", 2);
        CanAttack = 2;
        yield return new WaitForSeconds(Attack_Speed);
        animator.SetInteger("IsAttack", 0);
        CanAttack = 0;
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
    void RimitSpeed()
    {
        if(rigid.velocity.x > 5.0f)
        {
            rigid.velocity -= new Vector2(rigid.velocity.x - 5.0f, 0);
        }
        if (rigid.velocity.x < -5.0f)
        {
            rigid.velocity -= new Vector2(rigid.velocity.x + 5.0f, 0);
        }
    }

    void Damaged(int _damage)
    {
        StartCoroutine(DamagedCoolDown());
        GameMgr.GetInstance().PHP -= _damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("AttackBox") && CanDamaged == true)
        {
            if(collision.CompareTag("Slime"))
            {
                Damaged(20);
            }
        }
    }

    void RayCast()
    {
        Vector2 Pos = new Vector2(transform.position.x, transform.position.y);
        Debug.DrawRay(Pos - new Vector2(0, -0.5f), Vector2.right * FlipX, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(Pos - new Vector2(0, -0.5f), Vector2.right * FlipX, 1, LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            ScanedObj = hit.collider.gameObject;
            if (Input.GetKeyDown(KeyCode.G))
            {
                QuestMgr.GetInstance().Action(ScanedObj);
            }
        }
        else
        {
            ScanedObj = null;
        }
    }

}
