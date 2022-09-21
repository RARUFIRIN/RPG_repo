using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_item : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;  // ������ ������ ������
    [SerializeField]
    int count;              // ������ ������ ����


    private void Destruction()
    {
        for(int i = 0; i < count; i++)      // ī��Ʈ ��ŭ ������ ����
        {
            Instantiate(itemPrefab, itemPrefab.transform.position, Quaternion.identity);
        }
        Destroy(gameObject);                // ������ ��� �� ������Ʈ ����
    }
}
