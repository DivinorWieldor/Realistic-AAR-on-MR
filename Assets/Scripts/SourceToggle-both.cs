using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceToggleBoth : MonoBehaviour
{
    public GameObject targetObject;

    private void OnTriggerEnter(Collider other)
    {
        if (targetObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Triggered");
            gameObject.SetActive(false);
        }
    }
}
