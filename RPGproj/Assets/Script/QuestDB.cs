using System.Collections;
using System.Collections.Generic;

public class QuestDB
{
    public string questName;
    public int[] NPCid;

    public QuestDB(string name, int[] npcid)
    {
        questName = name;
        NPCid = npcid;
    }

}
