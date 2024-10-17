using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float SpeedHorizontal = 50.0f;
    public float SpeedVertical_x = 50.0f;
    public float SpeedVertical_z = 50.0f;

    public bool RandomizeSpeed = false;
    public bool Reverse = false;

    [Range(0, 1)]
    public int SpinX = 0;
    [Range(0, 1)]
    public int SpinY = 1;
    [Range(0, 1)]
    public int SpinZ = 0;

    // Update is called once per frame
    void Update() {
        if(RandomizeSpeed) {
            SpeedHorizontal = Random.Range(0.5f, 200.0f);
            RandomizeSpeed = false;
        }

        if (Reverse) {
            transform.Rotate(Vector3.up, SpinY * - 1 * SpeedHorizontal * Time.deltaTime);
            transform.Rotate(Vector3.left, SpinX * -1 * SpeedVertical_x * Time.deltaTime);
            transform.Rotate(Vector3.back, SpinZ * -1 * SpeedVertical_z * Time.deltaTime);
        }
        else {
            transform.Rotate(Vector3.up, SpinY * SpeedHorizontal * Time.deltaTime);
            transform.Rotate(Vector3.left, SpinX * SpeedVertical_x * Time.deltaTime);
            transform.Rotate(Vector3.back, SpinZ * SpeedVertical_z * Time.deltaTime);
        }
        
    }
}
