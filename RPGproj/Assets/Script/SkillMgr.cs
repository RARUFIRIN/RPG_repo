using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMgr : MonoBehaviour
{
    // 스킬매니저 싱글톤 //
    private SkillMgr() { }
    private static SkillMgr instance = null;
    public static SkillMgr GetInstance()
    {
        if (instance == null)
        {

            Debug.LogError("오브젝트를 찾을 수 없습니다.");
        }
        return instance;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (this != instance)
            {
                Destroy(gameObject);
            }
        }
    }

    float SkillTime;
    Vector2 SkillVelocity;
    [SerializeField]
    Item NAttack;
    [SerializeField]
    Item SAttack;
    [SerializeField]
    Item Jump;
    

    public Item SetSkill(float _f, Vector2 _vec, int _skillNum)
    {
        SkillTime = _f;             // 스킬 유지시간
        SkillVelocity = _vec;       // 스킬 움직임
        if (_skillNum == 0)         // 스킬 구분
            return NAttack;
        else if (_skillNum == 1)
            return SAttack;
        else if (_skillNum == 2)
            return Jump;
        else
            return null;

    }
    public float GetSkillTime()
    {
        return SkillTime;
    }
    public Vector2 GetSkillVelocity()
    {
        return SkillVelocity;
    }
}
