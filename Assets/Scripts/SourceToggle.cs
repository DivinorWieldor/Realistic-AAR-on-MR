using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceToggle : MonoBehaviour
{
    //decrease volume when player close

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        gameObject.SetActive(false);
    }

}
