using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    GameObject SpawnMonster;

    public GameObject SpawnedMonster;

    private void Start()
    {
        Spawn();
        StartCoroutine(CSapwnMonster());
    }
    void Update()
    {

    }

    void Spawn()
    {
        SpawnedMonster = Instantiate(SpawnMonster,new Vector3(transform.position.x,transform.position.y),Quaternion.identity);
    }

    IEnumerator CSapwnMonster()
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);
            if (SpawnedMonster == null)
            {
                Spawn();
            }
        }
    }
}
