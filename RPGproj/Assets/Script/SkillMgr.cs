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
    bool NAttackCool = true;
    [SerializeField]
    Item SAttack;
    bool SAttackCool = true;
    [SerializeField]
    Item Jump;
    bool JumpCool = true;


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

    public bool GetSkillCoolDown(Item item)
    {
        switch (item.itemidx)
        {
            case 500:
                {
                    return NAttackCool;
                }
            case 501:
                {
                    return SAttackCool;
                }
            case 502:
                {
                    return JumpCool;
                }
            default:
                {
                    return false;
                }
        }
    }

    public void SetSkillCoolDown(Item item, bool _b)
    {
        switch (item.itemidx)
        {
            case 500:
                {
                    NAttackCool = _b;
                    break;
                }
            case 501:
                {
                    SAttackCool = _b;
                    break;
                }
            case 502:
                {
                    JumpCool = _b;
                    break;
                }
            default:
                break;
        }
    }

    public int returnDmg(string str)
    {
        if(str == "SAttack")
        {
            return SAttack.AttackDamage * GameMgr.GetInstance().PAttackDamage;
        }
        else if(str == "NAttack")
        {
            return NAttack.AttackDamage * GameMgr.GetInstance().PAttackDamage;
        }
        else
        {
            return 0;
        }

    }
}
