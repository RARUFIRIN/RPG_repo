using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMgr : MonoBehaviour
{
    // ��ų�Ŵ��� �̱��� //
    private SkillMgr() { }
    private static SkillMgr instance = null;
    public static SkillMgr GetInstance()
    {
        if (instance == null)
        {

            Debug.LogError("������Ʈ�� ã�� �� �����ϴ�.");
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
        SkillTime = _f;             // ��ų �����ð�
        SkillVelocity = _vec;       // ��ų ������
        if (_skillNum == 0)         // ��ų ����
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
