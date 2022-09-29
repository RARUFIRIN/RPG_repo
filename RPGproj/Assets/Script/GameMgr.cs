using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    // 게임매니저 싱글톤 //
    private GameMgr() { }
    private static GameMgr instance = null;
    public static GameMgr GetInstance()
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
            if(this != instance)
            {
                Destroy(gameObject);
            }
        }
    }

    // 플레이어 상태값
    int Dmg;                // 기본 데미지
    int AttackDamage;       // 최종 데미지

    int Def;                // 기본 방어력
    int Defense;            // 최종 방어력

    [SerializeField]
    TextMeshProUGUI Text_Attack;
    [SerializeField]
    TextMeshProUGUI Text_Defense;


    float Speed = 1.0f;     // 이동속도
    float Jump = 400.0f;    // 점프력
    int isJump = 0;         // 0 = 점프 가능한 상태 1 = 점프중(상승) 2 = 점프중(하강)
    bool IsGround;          // 바닥과 닿아있는지 체크

    // 장비 추가능력치
    int WeaponDmg;
    int ArmorDef;

    int MaxHP;
    int HP;
    [SerializeField]
    Slider HPSlider;
    [SerializeField]
    GameObject HPFill;
    [SerializeField]
    TextMeshProUGUI Text_HP;
    [SerializeField]
    TextMeshProUGUI Text_MaxHP;

    int MaxMP;
    int MP;
    [SerializeField]
    Slider MPSlider;
    [SerializeField]
    GameObject MPFill;
    [SerializeField]
    TextMeshProUGUI Text_MP;
    [SerializeField]
    TextMeshProUGUI Text_MaxMP;

    int Level;
    int MaxEXP;
    int EXP;
    [SerializeField]
    Slider ExpSlider;
    [SerializeField]
    GameObject ExpFill;
    [SerializeField]
    TextMeshProUGUI Text_EXP;
    [SerializeField]
    TextMeshProUGUI Text_MaxEXP;

    int DropedExp;
    bool IsKilled = false;
    Vector3 PlayerPos;

    // 장면전환용
    bool TryInter;
    [SerializeField]
    GameObject Fade;
    public int SceneNum = -1;

    [SerializeField]
    SpriteRenderer BackGround;
    [SerializeField]
    Sprite BG_1, BG_2;

    [SerializeField]
    GameObject Player;
    [SerializeField]
    Rigidbody2D Player_rigid;


    private void Start()
    {
        MaxHP = 200;    HP = 200;
        MaxMP = 100;    MP = 100;
        MaxEXP = 100;   EXP = 0;    Level = 5;
    }

    private void Update()
    {
        ExpControl(); // EXP
        HPMPControl();
        StateControl();
    }

    void ExpControl()
    {
        Text_EXP.text = EXP.ToString();
        Text_MaxEXP.text = "/ " + MaxEXP.ToString();
        ExpSlider.value = EXP; 
        ExpSlider.maxValue = MaxEXP;
        if (ExpSlider.value == 0 && ExpFill.activeSelf == true) // 반복적인 호출 방지용 bool 사용
        {
            ExpFill.SetActive(false);
        }
        else if (ExpSlider.value > 0 && ExpFill.activeSelf == false)
        {
            ExpFill.SetActive(true);
        }


        if (MaxEXP < EXP)    // 경험치가 가득 차면 레벨업
        {
            Level++;
            EXP = EXP - MaxEXP;
            MaxEXP += 100;
        }

        if (IsKilled == true)    // 몬스터를 죽이면 몬스토쪽에서 보낸 EXP 만큼 올린 후 이후 호출방지를 위한 bool IsKilled
        {
            EXP += DropedExp;
            IsKilled = false;
        }
    }

    void HPMPControl()
    {
        // HP
        Mathf.Clamp(HP, 0, MaxHP);
        Text_HP.text = HP.ToString();
        Text_MaxHP.text = "/ " + MaxHP.ToString();
        HPSlider.value = HP;
        HPSlider.maxValue = MaxHP;
        if (HPSlider.value == 0 && HPFill.activeSelf == true) // 반복적인 호출 방지용 bool 사용
        {
            ExpFill.SetActive(false);
        }
        else if (HPSlider.value > 0 && HPFill.activeSelf == false)
        {
            ExpFill.SetActive(true);
        }



        // MP
        Mathf.Clamp(MP, 0, MaxMP);
        Text_MP.text = MP.ToString();
        Text_MaxMP.text = "/ " + MaxMP.ToString();
        MPSlider.value = MP;
        MPSlider.maxValue = MaxMP;
        if (MPSlider.value == 0 && MPFill.activeSelf == true) // 반복적인 호출 방지용 bool 사용
        {
            ExpFill.SetActive(false);
        }
        else if (MPSlider.value > 0 && MPFill.activeSelf == false)
        {
            ExpFill.SetActive(true);
        }
    }

    void StateControl()
    {
        AttackDamage = WeaponDmg + Dmg;
        Defense = ArmorDef + Def;
        Text_Attack.text = AttackDamage.ToString() + "(" + WeaponDmg.ToString() + ")";

        Dmg = Level * 5 + 20;
        Def = Level * 3;
        Text_Defense.text = Defense.ToString() + "(" + ArmorDef.ToString() + ")";
    }

    //페이드 아웃
    public IEnumerator FadeInStart()
    {
        Fade.SetActive(true);
        for (float f = 1f; f > 0; f -= 0.02f)
        {
            Color c = Fade.GetComponent<Image>().color;
            c.a = f;
            Fade.GetComponent<Image>().color = c;
        }
        yield return new WaitForSeconds(1);
        Fade.SetActive(false);
    }

    //페이드 인
    public IEnumerator FadeOutStart(int i)
    {
        Fade.SetActive(true);
        for (float f = 0f; f < 1; f += 0.02f)
        {
            Color c = Fade.GetComponent<Image>().color;
            c.a = f;
            Fade.GetComponent<Image>().color = c;
            yield return null;
        }
        SceneManager.LoadScene(i);
    }

    public void Respawn(float x, float y)
    {
        Player_rigid.velocity = new Vector2(0, 0);
        Player.transform.position = new Vector2(x, y);
    }

    public bool GetRandomResult(int _per)
    {
        int result = Random.Range(1, 101);
        if(result <= _per)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #region State Get Set 
    public bool PTryInter
    {
        get { return TryInter; }
        set { TryInter = value; }

    }
    public int PLevel
    {
        get { return Level; }
    }         // 레벨 
    public int PDropEXP
    {
        get { return DropedExp; }
        set { DropedExp = value; }
    }       // 경험치
    public bool PIsKilled
    {
        get { return IsKilled; }
        set { IsKilled = value; }
    }     // 킬 여부
    public int PHP
    {
        // HP
        get { return HP; }
        set { HP = value; }
    }            // 체력
    public int PMaxHP
    {
        // HP
        get { return MaxHP; }
        set { MaxHP = value; }
    }         // 최대 체력
    public int PMP
    {
        // MP
        get { return MP; }
        set { MP = value; }
    }            // 마나
    public int PMaxMP
    {
        // HP
        get { return MaxMP; }
        set { MaxMP = value; }
    }         // 최대 마나
    public float PSpeed
    {
        // 이동속도
        get { return Speed; }
        set { Speed = value; }
    }       // 이동속도
    public float PJump
    {
        // 점프력
        get { return Jump; }
        set { Jump = value; }
    }        // 점프력
    public int PIsJump
    {
        // 점프 상태값
        get { return isJump; }
        set { isJump = value; }
    }        // 점프 여부
    public bool PIsGround
    {
        // 바닥 접촉 상태값
        get { return IsGround; }
        set { IsGround = value; }
    }     // 바닥에 닿았는지 여부
    public Vector3 PPlayerPos
    {
        // 플레이어 포지션
        get { return PlayerPos; }
        set { PlayerPos = value; }
    } // 플레이어 위치
    public int PAttackDamage
    {
        // 공격력
        get { return AttackDamage; }
        set { AttackDamage = value; }
    }  // 데미지

    public int PWeaponDmg
    {
        get { return WeaponDmg; }
        set { WeaponDmg = value; }
    }     // 무기 공격력
    public int PArmorDef
    {
        get { return ArmorDef; }
        set { ArmorDef = value; }
    }      // 방어구 방어력
    public int CurScene
    {
        get { return SceneNum; }
        set { SceneNum = value; }
    }
    #endregion
}
