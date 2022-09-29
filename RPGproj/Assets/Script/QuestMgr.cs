using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class QuestMgr : MonoBehaviour
{
    // ����Ʈ�Ŵ��� �̱��� //
    private QuestMgr() { }
    private static QuestMgr instance = null;
    public static QuestMgr GetInstance()
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
        TalkData = new Dictionary<int, string[]>();
        RegistData();

        questList = new Dictionary<int, QuestDB>();     // ����Ʈ ���
        QuestClearCheck = new Dictionary<int, bool>();  // ����Ʈ ���� üũ
        RegistQuest();
    }
    // ��ȭ �Ŵ�¡
    Dictionary<int, string[]> TalkData;
    bool IsAction = false;

    [SerializeField]
    TextMeshProUGUI TalkText;
    [SerializeField]
    GameObject TalkBox;
    [SerializeField]
    TextMeshProUGUI QuestText;
    [SerializeField]
    GameObject QuestBox;

    GameObject ScanedNPC;
    int talkIdx;
    void RegistData()
    {
        // �Ϲ�
        TalkData.Add(1000 + 20, new string[] { "Ʃ�丮���� ���̾� ��Ż�� ���� ���� �������� �� ��" });
        TalkData.Add(2000 + 10, new string[] { "���� Ʃ�丮���� ��ġ�� �ʾұ�" , "Ʃ�丮���� ����ġ�� ������ �Ͻð�" });
        TalkData.Add(2000 + 11, new string[] { "���� Ʃ�丮���� ��ġ�� �ʾұ�" , "Ʃ�丮���� ����ġ�� ������ �Ͻð�" });
        TalkData.Add(2000 + 12, new string[] { "���� Ʃ�丮���� ��ġ�� �ʾұ�" , "Ʃ�丮���� ����ġ�� ������ �Ͻð�" });
        TalkData.Add(2000 + 13, new string[] { "���� Ʃ�丮���� ��ġ�� �ʾұ�" , "Ʃ�丮���� ����ġ�� ������ �Ͻð�" });


        // ����Ʈ��
        TalkData.Add(1000 + 10, new string[] { "���� Ʃ�丮���� ����ϴ� Ʃ�丮����̾�.", "����Ű�� �̵��� �� �ְ� AŰ�� �����̾�.",
                                         "�˰��ְ����� GŰ�� ��ȣ�ۿ��� �� �� �־�.", "�׷� ������ �����Ӱ� ���븦 ��� ����� ���ָ� 3���� ������." });
        TalkData.Add(1000 + 11, new string[] { "���� ������ ���Ѱž�?" , "���ؾ� �� �� ����� ���� 3����"});
        TalkData.Add(1000 + 12, new string[] { "�����.." , "�� �����Ա���! ����� �ʾ����ž�.", "��� ���״ϱ� ���� �������� �� ��"});
        TalkData.Add(1000 + 13, new string[] { });
    }



    public void Action(GameObject scanNpc)
    {
        ScanedNPC = scanNpc;
        QuestNPC npcdata = ScanedNPC.GetComponent<QuestNPC>();
        Talk(npcdata.NPC_id);

        TalkBox.SetActive(IsAction);

    }

    void Talk(int NPCid) // ���Ǿ� ���̵� �޾ƿ�
    {
        QuestCheck(NPCid + QuestId + QuestNow);
        if (QuestClearCheck.ContainsKey(NPCid + QuestId + QuestNow) && QuestClearCheck[NPCid + QuestId + QuestNow])
        {
            QuestNow++;
        }
        int questidx = GetQuest(NPCid);    // ����Ʈ�� �ε���
        string TalkData = GetData(NPCid + questidx, talkIdx);  // ����Ʈ ���൵�� ���� ��ȭ ���
        
        if (TalkData == null)    // ��ȭ������ ������ ��ȭ ��
        {
            IsAction = false;
            talkIdx = 0;        // �ε��� �ʱ�ȭ
            if (QuestClearCheck.ContainsKey(NPCid + QuestId + QuestNow - 1) && QuestClearCheck[NPCid + QuestId + QuestNow - 1])
            {
                ClearQurest(NPCid + questidx - 1);
                ProgressQuest(NPCid);
                return;
            }
            else
            {
                ProgressQuest(NPCid);
                return;
            }
        }

        TalkText.text = TalkData;

        IsAction = true;
        talkIdx++;
    }
    public string GetData(int id, int idx)
    {
        if (idx == TalkData[id].Length) // ��ȭ �������� �ٴٸ��� null ��ȯ
            return null;
        else
            return TalkData[id][idx];
    }


    // ����Ʈ �Ŵ�¡
    public int QuestId;    // ����Ʈ ���̵�
    public int QuestNow;   // ����Ʈ ���൵

    Dictionary<int, QuestDB> questList;
    Dictionary<int, bool> QuestClearCheck;

    void RegistQuest()
    {
        questList.Add(10, new QuestDB("Ʃ�丮��", new int[] { 1000 , 1000 , 1000 , 1000 }));
        QuestClearCheck.Add(1011, false);


        questList.Add(20, new QuestDB("���� ���", new int[] { 2000 }));
    }


    int GetQuest(int id)
    {
        return QuestId + QuestNow;
    }

    void ProgressQuest(int id)
    {
        if (!QuestClearCheck.ContainsKey(id + QuestId + QuestNow) && id == questList[QuestId].NPCid[QuestNow])       // �ش� �ε����� ����Ʈ�� ������� ������ȭ�� �Ѿ �� ����Ʈ Ȯ���� �ǵ���
        {
            if (TalkData[id + QuestId + QuestNow + 1].Length >= 1)
            {
                QuestNow++;
            }
            else
            {
                NextQuest();
            }
        }

        if(QuestNow == questList[QuestId].NPCid.Length && QuestClearCheck[id + QuestId + QuestNow])
            NextQuest();

    }

    void NextQuest()
    {
        QuestId += 10;
        QuestNow = 0;
    }


    void QuestCheck(int idx)
    {
        switch (idx)
        {
            case (1011):
                {
                    if (InventoryMgr.GetInstance().SearchItem(101, 3) && InventoryMgr.GetInstance().SearchItem(102, 3))
                    {
                        QuestClearCheck[1011] = true;
                    }
                }
                break;

            default:
                {
                    break;
                }
        }
    }

    void ClearQurest(int idx)
    {
        switch (idx)
        {
            case (1011):
                {
                    InventoryMgr.GetInstance().SpendItem(101, 3);
                    InventoryMgr.GetInstance().SpendItem(102, 3);
                    InventoryMgr.GetInstance().GainItem(ItemDB.GetInstance().GetItem(ItemDB.ItemName.LeatherBoots));
                    InventoryMgr.GetInstance().GainItem(ItemDB.GetInstance().GetItem(ItemDB.ItemName.LeatherArmor));
                    InventoryMgr.GetInstance().GainItem(ItemDB.GetInstance().GetItem(ItemDB.ItemName.LeatherHelmet));
                    InventoryMgr.GetInstance().GainItem(ItemDB.GetInstance().GetItem(ItemDB.ItemName.WoodenSword));
                }
                break;

            default:
                break;
        }
    }















    public bool GetIsAction()
    {
        return IsAction;
    }
    public GameObject GetScanedObj()
    {
        return ScanedNPC;
    }

}
