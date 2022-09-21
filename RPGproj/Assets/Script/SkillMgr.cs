using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMgr : MonoBehaviour
{
    // 인벤토리 싱글톤 //
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
    Vector2 SkillVec;
    

    public void SetSkill(float _f, Vector2 _Vec)
    {
        SkillTime = _f;
        SkillVec = _Vec;
    }
    public float GetSkillTime()
    {
        return SkillTime;
    }
    public Vector2 GetSkillVec()
    {
        return SkillVec;
    }
}
