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
    // 퀘스트매니저 싱글톤 //
    private QuestMgr() { }
    private static QuestMgr instance = null;
    public static QuestMgr GetInstance()
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
        TalkData = new Dictionary<int, string[]>();
        RegistData();

        questList = new Dictionary<int, QuestDB>();     // 퀘스트 목록
        QuestClearCheck = new Dictionary<int, bool>();  // 퀘스트 조건 체크
        RegistQuest();
    }
    // 대화 매니징
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
        // 일반
        TalkData.Add(1000 + 20, new string[] { "튜토리얼은 끝이야 포탈을 통해 다음 지역으로 가 봐" });
        TalkData.Add(2000 + 10, new string[] { "아직 튜토리얼을 마치지 않았군" , "튜토리얼을 끝마치고 오도록 하시게" });
        TalkData.Add(2000 + 11, new string[] { "아직 튜토리얼을 마치지 않았군" , "튜토리얼을 끝마치고 오도록 하시게" });
        TalkData.Add(2000 + 12, new string[] { "아직 튜토리얼을 마치지 않았군" , "튜토리얼을 끝마치고 오도록 하시게" });
        TalkData.Add(2000 + 13, new string[] { "아직 튜토리얼을 마치지 않았군" , "튜토리얼을 끝마치고 오도록 하시게" });


        // 퀘스트용
        TalkData.Add(1000 + 10, new string[] { "나는 튜토리얼을 담당하는 튜토리얼맨이야.", "방향키로 이동할 수 있고 A키가 공격이야.",
                                         "알고있겠지만 G키로 상호작용을 할 수 있어.", "그럼 시험삼아 슬라임과 늑대를 잡고 사과와 맥주를 3개씩 얻어와줘." });
        TalkData.Add(1000 + 11, new string[] { "아직 구하지 못한거야?" , "구해야 할 건 사과랑 맥주 3개야"});
        TalkData.Add(1000 + 12, new string[] { "어디보자.." , "잘 가져왔구나! 어렵진 않았을거야.", "장비를 줄테니까 다음 지역으로 가 봐"});
        TalkData.Add(1000 + 13, new string[] { });
    }



    public void Action(GameObject scanNpc)
    {
        ScanedNPC = scanNpc;
        QuestNPC npcdata = ScanedNPC.GetComponent<QuestNPC>();
        Talk(npcdata.NPC_id);

        TalkBox.SetActive(IsAction);

    }

    void Talk(int NPCid) // 엔피씨 아이디 받아옴
    {
        QuestCheck(NPCid + QuestId + QuestNow);
        if (QuestClearCheck.ContainsKey(NPCid + QuestId + QuestNow) && QuestClearCheck[NPCid + QuestId + QuestNow])
        {
            QuestNow++;
        }
        int questidx = GetQuest(NPCid);    // 퀘스트용 인덱스
        string TalkData = GetData(NPCid + questidx, talkIdx);  // 퀘스트 진행도에 따라 대화 출력
        
        if (TalkData == null)    // 대화내용이 끝나면 대화 끝
        {
            IsAction = false;
            talkIdx = 0;        // 인덱스 초기화
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
        if (idx == TalkData[id].Length) // 대화 마지막에 다다르면 null 반환
            return null;
        else
            return TalkData[id][idx];
    }


    // 퀘스트 매니징
    public int QuestId;    // 퀘스트 아이디
    public int QuestNow;   // 퀘스트 진행도

    Dictionary<int, QuestDB> questList;
    Dictionary<int, bool> QuestClearCheck;

    void RegistQuest()
    {
        questList.Add(10, new QuestDB("튜토리얼", new int[] { 1000 , 1000 , 1000 , 1000 }));
        QuestClearCheck.Add(1011, false);


        questList.Add(20, new QuestDB("보스 잡기", new int[] { 2000 }));
    }


    int GetQuest(int id)
    {
        return QuestId + QuestNow;
    }

    void ProgressQuest(int id)
    {
        if (!QuestClearCheck.ContainsKey(id + QuestId + QuestNow) && id == questList[QuestId].NPCid[QuestNow])       // 해당 인덱스에 퀘스트가 없을경우 다음대화로 넘어간 후 퀘스트 확인자 되돌림
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
