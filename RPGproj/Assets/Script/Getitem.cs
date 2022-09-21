using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Getitem : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    GameObject player;

    private void Update()
    {
        gameObject.transform.position = player.transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if(collision.CompareTag("Item") && Input.GetKey(KeyCode.Z))
        {
            InventoryMgr.GetInstance().GainItem(collision.transform.GetComponent<Prefab_item>().item);
            Destroy(collision.gameObject); // �ֿ� ������ �ı�
        }

        // ���� ����
        if (collision.transform.CompareTag("Ground") && GameMgr.GetInstance().PIsJump == 2)
        {
            animator.SetInteger("IsJump", 0);
            GameMgr.GetInstance().PIsJump = 0;
        }
    }

    // �ٴ� üũ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            if (GameMgr.GetInstance().PIsJump == 1)
            {
                animator.SetInteger("IsJump", 0);
                GameMgr.GetInstance().PIsJump = 0;
            }

            GameMgr.GetInstance().PIsGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            GameMgr.GetInstance().PIsGround = false;
        }
    }
}
