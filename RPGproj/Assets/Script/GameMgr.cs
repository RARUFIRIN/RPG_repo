using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    // ���ӸŴ��� �̱��� //
    private GameMgr() { }
    private static GameMgr instance = null;
    public static GameMgr GetInstance()
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
            if(this != instance)
            {
                Destroy(gameObject);
            }
        }
    }

    // �÷��̾� ���°�
    int Dmg;                // �⺻ ������
    int AttackDamage;       // ���� ������

    int Def;                // �⺻ ����
    int Defense;            // ���� ����

    [SerializeField]
    TextMeshProUGUI Text_Attack;
    [SerializeField]
    TextMeshProUGUI Text_Defense;


    float Speed = 1.0f;     // �̵��ӵ�
    float Jump = 400.0f;    // ������
    int isJump = 0;         // 0 = ���� ������ ���� 1 = ������(���) 2 = ������(�ϰ�)
    bool IsGround;          // �ٴڰ� ����ִ��� üũ

    // ��� �߰��ɷ�ġ
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

    // �����ȯ��
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
        if (ExpSlider.value == 0 && ExpFill.activeSelf == true) // �ݺ����� ȣ�� ������ bool ���
        {
            ExpFill.SetActive(false);
        }
        else if (ExpSlider.value > 0 && ExpFill.activeSelf == false)
        {
            ExpFill.SetActive(true);
        }


        if (MaxEXP < EXP)    // ����ġ�� ���� ���� ������
        {
            Level++;
            EXP = EXP - MaxEXP;
            MaxEXP += 100;
        }

        if (IsKilled == true)    // ���͸� ���̸� �����ʿ��� ���� EXP ��ŭ �ø� �� ���� ȣ������� ���� bool IsKilled
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
        if (HPSlider.value == 0 && HPFill.activeSelf == true) // �ݺ����� ȣ�� ������ bool ���
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
        if (MPSlider.value == 0 && MPFill.activeSelf == true) // �ݺ����� ȣ�� ������ bool ���
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

    //���̵� �ƿ�
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

    //���̵� ��
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
    }         // ���� 
    public int PDropEXP
    {
        get { return DropedExp; }
        set { DropedExp = value; }
    }       // ����ġ
    public bool PIsKilled
    {
        get { return IsKilled; }
        set { IsKilled = value; }
    }     // ų ����
    public int PHP
    {
        // HP
        get { return HP; }
        set { HP = value; }
    }            // ü��
    public int PMaxHP
    {
        // HP
        get { return MaxHP; }
        set { MaxHP = value; }
    }         // �ִ� ü��
    public int PMP
    {
        // MP
        get { return MP; }
        set { MP = value; }
    }            // ����
    public int PMaxMP
    {
        // HP
        get { return MaxMP; }
        set { MaxMP = value; }
    }         // �ִ� ����
    public float PSpeed
    {
        // �̵��ӵ�
        get { return Speed; }
        set { Speed = value; }
    }       // �̵��ӵ�
    public float PJump
    {
        // ������
        get { return Jump; }
        set { Jump = value; }
    }        // ������
    public int PIsJump
    {
        // ���� ���°�
        get { return isJump; }
        set { isJump = value; }
    }        // ���� ����
    public bool PIsGround
    {
        // �ٴ� ���� ���°�
        get { return IsGround; }
        set { IsGround = value; }
    }     // �ٴڿ� ��Ҵ��� ����
    public Vector3 PPlayerPos
    {
        // �÷��̾� ������
        get { return PlayerPos; }
        set { PlayerPos = value; }
    } // �÷��̾� ��ġ
    public int PAttackDamage
    {
        // ���ݷ�
        get { return AttackDamage; }
        set { AttackDamage = value; }
    }  // ������

    public int PWeaponDmg
    {
        get { return WeaponDmg; }
        set { WeaponDmg = value; }
    }     // ���� ���ݷ�
    public int PArmorDef
    {
        get { return ArmorDef; }
        set { ArmorDef = value; }
    }      // �� ����
    public int CurScene
    {
        get { return SceneNum; }
        set { SceneNum = value; }
    }
    #endregion
}
