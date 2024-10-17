using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRegionTurnOff : MonoBehaviour
{
    Collider m_ObjectCollider;
    public GameObject thisObj;

    // Start is called before the first frame update
    void Start()
    {
        m_ObjectCollider = GetComponent<Collider>();

    }

    private void OnTriggerEnter(Collider other)
    {
        thisObj.SetActive(false);
    }
}
