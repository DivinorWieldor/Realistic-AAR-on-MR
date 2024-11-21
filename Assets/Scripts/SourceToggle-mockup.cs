using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceToggleMockup : MonoBehaviour
{
    public GameObject Source1;
    public GameObject Source2;
    public GameObject Source3;
    public GameObject Source4;
    public GameObject Source5;
    public GameObject Source6;

    [Range(0, 5)]
    public int option = 0;

    public bool randomize = false;

    // Update is called once per frame
    void Update() {
        if(randomize) {
            option = Random.Range(0, 6);
            randomize = false;

            Source1.SetActive(false);
            Source2.SetActive(false);
            Source3.SetActive(false);
            Source4.SetActive(false);
            Source5.SetActive(false);
            Source6.SetActive(false);
        }

        switch (option) {
            case 0:
                Source1.SetActive(true);
                Source2.SetActive(false);
                Source3.SetActive(false);
                Source4.SetActive(false);
                break;
            case 1:
                Source1.SetActive(false);
                Source2.SetActive(true);
                Source3.SetActive(false);
                Source4.SetActive(false);
                Source5.SetActive(false);
                Source6.SetActive(false);
                break;
            case 2:
                Source1.SetActive(false);
                Source2.SetActive(false);
                Source3.SetActive(true);
                Source4.SetActive(false);
                Source5.SetActive(false);
                Source6.SetActive(false);
                break;
            case 3:
                Source1.SetActive(false);
                Source2.SetActive(false);
                Source3.SetActive(false);
                Source4.SetActive(true);
                Source5.SetActive(false);
                Source6.SetActive(false);
                break;
            case 4:
                Source1.SetActive(false);
                Source2.SetActive(false);
                Source3.SetActive(false);
                Source4.SetActive(false);
                Source5.SetActive(true);
                Source6.SetActive(false);
                break;
            case 5:
                Source1.SetActive(false);
                Source2.SetActive(false);
                Source3.SetActive(false);
                Source4.SetActive(false);
                Source5.SetActive(false);
                Source6.SetActive(true);
                break;

        }
    }
}
