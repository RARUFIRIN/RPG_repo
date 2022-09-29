using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadObject : MonoBehaviour
{
    GameObject Player;
    [SerializeField]
    SpriteRenderer spriterenderer;
    [SerializeField]
    Sprite bg;
    [SerializeField]
    GameObject PortalLeft;
    [SerializeField]
    GameObject PortalRight;

    private void Awake()
    {
        spriterenderer.sprite = bg;
        Player = GameObject.Find("Player");
        SpawnScenePoint();
        StartCoroutine(GameMgr.GetInstance().FadeInStart());

        StartCoroutine(DestroyObj());
    }
    
    void SpawnScenePoint()
    {
        if(GameMgr.GetInstance().CurScene == -1)
        {
            Player.transform.position = new Vector3(0, 0, 0);
        }
        else if(GameMgr.GetInstance().CurScene < SceneManager.GetActiveScene().buildIndex)
        {
            if (PortalLeft != null)
                Player.transform.position = PortalLeft.transform.position;
            else
                Debug.Log("포탈이 없습니다.");
        }
        else if(GameMgr.GetInstance().CurScene > SceneManager.GetActiveScene().buildIndex)
        {
            if (PortalRight != null)
                Player.transform.position = PortalRight.transform.position;
            else
                Debug.Log("포탈이 없습니다.");
        }

        GameMgr.GetInstance().CurScene = SceneManager.GetActiveScene().buildIndex;
    }


    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
    
}
