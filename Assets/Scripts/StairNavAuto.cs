using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairNavAuto : MonoBehaviour
{
    public GameObject player;
    GameObject[] sources;
    GameObject[] indicators;

    void Start()
    {
        sources = GameObject.FindGameObjectsWithTag("soundSource"); //gets ACTIVE objects!
        indicators = GameObject.FindGameObjectsWithTag("abx");
        Debug.Log("Found " + indicators.Length + " indicators");

        foreach (GameObject source in sources) {
            if (source.activeInHierarchy) {
                if (source.transform.position.y > player.transform.position.y) {
                    /*AudioSource audioSource = source.GetComponent<AudioSource>();
                    if (audioSource != null)
                        audioSource.pitch = 1.5f; // sound is above, set pitch higher
                    */
                    AudioSource audioSource = player.GetComponent<AudioSource>();
                    audioSource.pitch = 1.5f; // sound is above, set pitch higher
                    audioSource.volume = 0.05f;

                    //stair indicator going up should be activated
                    foreach (GameObject indicator in indicators) {
                        //dumb solution for now: just activate the higher one since there is only one
                        //TODO: find the closest one and activate each floor's indicator as the player goes up

                        if (indicator.transform.position.y > player.transform.position.y) { 
                            audioSource = indicator.GetComponentInChildren<AudioSource>();
                            audioSource.volume = 1.0f;
                        }

                    }
                }
                //same shit but check if below
                else if (source.transform.position.y < player.transform.position.y) {
                    /*AudioSource audioSource = source.GetComponent<AudioSource>();
                    if (audioSource != null)
                        audioSource.pitch = 0.5f; // sound is below, set pitch lower
                    */
                    AudioSource audioSource = player.GetComponent<AudioSource>();
                    audioSource.pitch = 0.5f; // sound is above, set pitch higher
                    audioSource.volume = 0.05f;

                    //stair indicator going down should be activated
                    foreach (GameObject indicator in indicators)
                    {
                        //dumb solution for now: just activate the higher one since there is only one
                        //TODO: find the closest one and activate each floor's indicator as the player goes up

                        if (indicator.transform.position.y < player.transform.position.y) { 
                            audioSource = indicator.GetComponentInChildren<AudioSource>();
                            audioSource.volume = 1.0f;
                        }
                    }
                }
                //else, source is at same height. No need to modify.
                //TODO: reset source pitch when player goes up!

                //for now this only supports one active sound source
                //plus sources active in both up and down will only confuse the listener
                //we need to keep it simple
                break;
            }
        }
    }

    void Update()
    {
        foreach (GameObject source in sources) {
            if (Mathf.Abs(source.transform.position.y - player.transform.position.y) < 0.5) {
                /*AudioSource audioSource = source.GetComponent<AudioSource>();
                audioSource.pitch = 1.0f; // sound is at same height, reset pitch
                */

                AudioSource audioSource = player.GetComponent<AudioSource>();
                audioSource.pitch = 1.0f; // sound is above, set pitch higher
                audioSource.volume = 0;
            }
            break;
        }
    }
}
