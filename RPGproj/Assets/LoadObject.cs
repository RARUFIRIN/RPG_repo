using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadObject : MonoBehaviour
{
    GameObject Player;
    private void Awake()
    {
        Player = GameObject.Find("Player");
        Player.transform.position = new Vector3(0, 0, 0);
        StartCoroutine(GameMgr.GetInstance().FadeInStart());

        StartCoroutine(DestroyObj());
    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(this);
    }
    
}
