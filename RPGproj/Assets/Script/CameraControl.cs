using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    GameObject GPlayer;



    float HWidth;
    float HHeight;
    [SerializeField]
    float MinX, MaxX, MinY, MaxY;

    void Start()
    {
        GPlayer = GameObject.Find("Player");
        HWidth = Camera.main.aspect * Camera.main.orthographicSize;
        HHeight = Camera.main.orthographicSize;
    }


    void Update()
    {
        if (QuestMgr.GetInstance().GetIsAction() == false)
            PlayerCamera();

        NPCCamera();
    }

    void PlayerCamera()
    {
        Vector3 LimitPos = new Vector3( Mathf.Clamp(GPlayer.transform.position.x, MinX + HWidth, MaxX - HWidth),
                                        Mathf.Clamp(GPlayer.transform.position.y, MinY + HHeight, MaxY - HHeight),
                                        -10);
        transform.position = Vector3.Lerp(transform.position, LimitPos, Time.deltaTime * 4.0f);
    }

    void NPCCamera()
    {
        if (QuestMgr.GetInstance().GetIsAction() == true)
        {
            transform.position = QuestMgr.GetInstance().GetScanedObj().transform.position - new Vector3(5, -2, 10);
        }
    }
}
