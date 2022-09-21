using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_item : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;  // 생성될 아이템 프리팹
    [SerializeField]
    int count;              // 생성할 아이템 갯수


    private void Destruction()
    {
        for(int i = 0; i < count; i++)      // 카운트 만큼 아이템 생성
        {
            Instantiate(itemPrefab, itemPrefab.transform.position, Quaternion.identity);
        }
        Destroy(gameObject);                // 아이템 드롭 후 오브젝트 삭제
    }
}
