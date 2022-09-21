using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Portal : MonoBehaviour
{

    [SerializeField]
    int TargetScene;

    public bool NowChange = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (GameMgr.GetInstance().PTryInter == true && NowChange == false)
            {
                NowChange = true;
                StartCoroutine(GameMgr.GetInstance().FadeOutStart(TargetScene));
            }
        }
    }


}
