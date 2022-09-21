using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDistroy : MonoBehaviour
{
    void Awake()
    {
        for (int i = 0; i < Object.FindObjectsOfType<DontDistroy>().Length; i++)
        {
            if (Object.FindObjectsOfType<DontDistroy>()[i] != this)
            {
                if (Object.FindObjectsOfType<DontDistroy>()[i].name == gameObject.name)
                {
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
