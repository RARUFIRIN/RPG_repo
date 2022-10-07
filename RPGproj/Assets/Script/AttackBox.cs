using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    [SerializeField]
    float SkillTime; // 유지시간
    Vector2 vec; // 이동 방향
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
