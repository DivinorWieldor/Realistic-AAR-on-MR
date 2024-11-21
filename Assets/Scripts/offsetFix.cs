using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offsetFix : MonoBehaviour
{
    public GameObject playerCam;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExecuteAfterTime(5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        // Code to execute after the delay

        gameObject.transform.position = new Vector3(    playerCam.transform.position.x * -1, 
                                                        playerCam.transform.position.y * -1, 
                                                        playerCam.transform.position.z * -1
                                                   );
        /*gameObject.transform.eulerAngles = new Vector3( playerCam.transform.eulerAngles.x * -1,
                                                        playerCam.transform.eulerAngles.y,
                                                        playerCam.transform.eulerAngles.z
                                                       );*/
    }
}
