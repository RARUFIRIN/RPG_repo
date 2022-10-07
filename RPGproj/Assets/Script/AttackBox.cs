using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    [SerializeField]
    float SkillTime; // �����ð�
    Vector2 vec; // �̵� ����
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = GameMgr.GetInstance().PPFlipX;
        rigid = GetComponent<Rigidbody2D>();
        vec = SkillMgr.GetInstance().GetSkillVelocity();
        rigid.velocity = vec;
        StartCoroutine(EndSkill());
        Debug.Log("XX");
    }


    IEnumerator EndSkill()
    {
        yield return new WaitForSeconds(SkillTime);
        Destroy(gameObject);
    }
}
