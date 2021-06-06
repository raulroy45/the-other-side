using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDestroy : MonoBehaviour
{
    public GameObject[] objectsToBeDestroyed;

    void OnTriggerEnter2D(Collider2D other)
    {
        for (int i = 0; i < objectsToBeDestroyed.Length; i++)
        {
            objectsToBeDestroyed[i].SetActive(false);
        }
    }
}
