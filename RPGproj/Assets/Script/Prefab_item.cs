using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefab_item : MonoBehaviour
{
    Rigidbody2D rigid;
    public Item item;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        int X = Random.Range(-30, 30);
        rigid.AddForce(new Vector2(X, 100));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Ground"))
        {
            rigid.velocity = new Vector2(0, 0);
        }
    }
}
