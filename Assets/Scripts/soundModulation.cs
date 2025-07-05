using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class soundModulation : MonoBehaviour
{
    GameObject player; // the dynamic object we are tracking
    private float minLowPass = -10; // the lowest the audio will be modulated to
    private Vector3 soundPos; // the position of the items this is attached to
    public AudioMixer audioMixer; // the audio mixer we are modulating

    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        soundPos = transform.position;
        player = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            /* This section modulates the sound if it has a direct sight to the player. If behind the player, sound is dampened to show that it is behind */
            Vector3 direction = player.transform.position - soundPos;
            Ray ray = new Ray(soundPos, direction);

            if (Physics.Raycast(ray, out hit, direction.magnitude)) {
                /*float currentAttenVolume; // if we don't want the audio to jump but lerp instead
                audioMixer.GetFloat("AttenVolume", out currentAttenVolume);*/

                if (hit.collider.gameObject == player) 
                {
                    /*float targetVal = CalculateLowPass(player);
                    audioMixer.SetFloat("AttenVolume", Mathf.Lerp(currentAttenVolume, targetVal, 0.01f));*/
                    audioMixer.SetFloat("AttenVolume", CalculateLowPass(player));
                }
                else // lerp from current AttenVolume towards 0
                {
                    /*if (currentAttenVolume < 0)
                        audioMixer.SetFloat("AttenVolume", Mathf.Lerp(currentAttenVolume, 0, 0.001f));*/
                    audioMixer.SetFloat("AttenVolume", 0);

                    /* A major issue with lerping directly is that the user may think it's their fault
                     * If so, they will think that they moved around too much and might get the wrong idea
                     * So it needs to be clear to them that the change happened because of something on the
                     * system's end, not theirs.
                     * And so, if the audio has a perceivable jump, then the user will be more likely to
                     * associate the change with the system, not themselves.
                     */
                }
            }

            /* This section modulates the sound based on player height. If behind the player is at a low elevation, sound is high pitched to show it is above */
            // TODO: Should we even implement this? We already know up-down is hard to perceive? This test just measures navigational performance?
            //          but at the same time, what we're doing is novel because we're doing it in a 3D space, not just 2D
            //          but also also, they don't know original sound, meaning this isn't as helpful as the papers.
            //          idk, maybe if this shows lower cognitive load it's still good enough...

            /*float currentPitch; // if we don't want the audio to jump but lerp instead
            audioMixer.GetFloat("MasterPitch", out currentPitch);
            if (soundPos.y - player.transform.position.y > 1) audioMixer.SetFloat("MasterPitch", 1.05f);
            else if (soundPos.y - player.transform.position.y < -1) audioMixer.SetFloat("MasterPitch", 0.95f);
            else audioMixer.SetFloat("MasterPitch", Mathf.Lerp(currentPitch, 1, 0.001f));*/
        }
    }

    float CalculateLowPass(GameObject object1)
    {
        Vector3 directionToPlayer = transform.position - object1.transform.position;
        Vector3 forward = object1.transform.forward;

        float azimuthAngle = Vector3.SignedAngle(forward, directionToPlayer, Vector3.up);

        if (azimuthAngle >= 90 && azimuthAngle <= 180) // audio to the right
        {
            float t = (azimuthAngle - 90) / 90.0f; // Normalize the angle to a range of 0 to 1
            return Mathf.Lerp(0, minLowPass, t);
        }
        else if (azimuthAngle <= -90 && azimuthAngle >= -180) // audio to the left
        {
            float t = (azimuthAngle + 180) / 90.0f; // Normalize the angle to a range of 0 to 1
            return Mathf.Lerp(minLowPass, 0, t);

        }
        else // target is ahead
        {
            return 0;
        }
    }
}

