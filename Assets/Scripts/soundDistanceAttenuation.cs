using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundDistanceAttenuation : MonoBehaviour
{
    public GameObject target; // the dynamic object we are tracking
    public AudioSource audioSource; // the audio source we are modulating

    [SerializeField] private AnimationCurve distanceAttenuationCurve; // the curve that defines how the sound attenuates with distance
    public float distance; // the distance between the sound and the player
    private float soundLevel;

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);
        soundLevel = distanceAttenuationCurve.Evaluate(distance);
        audioSource.volume = soundLevel;
    }
}
