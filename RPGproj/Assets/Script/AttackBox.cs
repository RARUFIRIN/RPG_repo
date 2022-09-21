using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    float SkillTime; // �����ð�
    Vector2 vec; // �̵� ����
    Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        SkillTime = SkillMgr.GetInstance().GetSkillTime();
        vec = SkillMgr.GetInstance().GetSkillVec();
        rigid.velocity = vec;
        StartCoroutine(EndSkill());
    }


    IEnumerator EndSkill()
    {
        yield return new WaitForSeconds(SkillTime);
        Destroy(gameObject);
    }
}
