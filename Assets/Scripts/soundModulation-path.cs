using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class soundModulationPath : MonoBehaviour
{
    private GameObject player; // the dynamic object we are tracking
    private float minLowPass = -10; // the lowest the audio will be modulated to
    private Vector3 soundPos; // the position of the items this is attached to
    public AudioMixer audioMixer; // the audio mixer we are modulating

    [SerializeField]
    private LineRenderer Path;
    [SerializeField]
    private float PathHeightOffset = 1.25f;
    [SerializeField]
    private float PathUpdateSpeed = 0.25f;

    private NavMeshTriangulation Triangulation;
    private Coroutine DrawPathCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        Triangulation = NavMesh.CalculateTriangulation();
        soundPos = transform.position;
        player = GameObject.FindWithTag("MainCamera");

        if(DrawPathCoroutine != null)
            StopCoroutine(DrawPathCoroutine);

        DrawPathCoroutine = StartCoroutine(DrawPathToSound());
    }

    private IEnumerator DrawPathToSound()
    {
        WaitForSeconds wait = new WaitForSeconds(PathUpdateSpeed);
        NavMeshPath path = new NavMeshPath();

        while(true)
        {
            if(NavMesh.CalculatePath(player.transform.position, transform.position, NavMesh.AllAreas, path))
            {
                Path.positionCount = path.corners.Length;
                for (int i = 0; i < path.corners.Length; i++)
                    Path.SetPosition(i, path.corners[i] + Vector3.up * PathHeightOffset);
            }
            else
                Debug.LogWarning("Failed to calculate path from player to sound source.");

            yield return wait;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            /* modulates the sound if it has a direct sight to the player. If behind the player, sound is dampened to enhance the head occlusion effect */
            audioMixer.SetFloat("AttenVolume", CalculateLowPass_path(player, Path));
            

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

    float CalculateLowPass_path(GameObject object1, LineRenderer Path)
    {
        Vector3 targetPos = Path.GetPosition(1);
        Vector3 directionToPlayer = targetPos - object1.transform.position;
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

