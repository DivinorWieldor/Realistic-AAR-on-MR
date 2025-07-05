using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offsetFix : MonoBehaviour
{
    public GameObject playerCam;
    public trackingErrFix trackingErrFixScript;

    private Quaternion initialChildRotation;
    private float initialChildYRotation;

    // Start is called before the first frame update
    void Start()
    {
        initialChildRotation = playerCam.transform.localRotation;
        initialChildYRotation = playerCam.transform.localEulerAngles.y;
        StartCoroutine(ExecuteAfterTime(3));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        // Code to execute after the delay

        float unexpectedYRotation = playerCam.transform.localEulerAngles.y - initialChildYRotation;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y - unexpectedYRotation, 0);

        /*Quaternion unexpectedRotation = playerCam.transform.localRotation * Quaternion.Inverse(initialChildRotation);
        transform.rotation *= Quaternion.Inverse(unexpectedRotation);*/

        gameObject.transform.position = new Vector3(    playerCam.transform.position.x * -1, 
                                                        playerCam.transform.position.y * -1, 
                                                        playerCam.transform.position.z * -1
                                                   );

        Debug.Log("position reset");
        // activate the trackingErrFixScript
        trackingErrFixScript.enabled = true;
    }
}
