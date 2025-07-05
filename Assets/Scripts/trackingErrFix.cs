using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackingErrFix : MonoBehaviour
{
    public Transform trackedCamera;     // The camera being tracked (child)
    public float maxAllowedDistance = 3f;

    private Vector3 lastGoodWorldPos;
    private Quaternion lastGoodWorldRot;
    private bool isFirstValid = false;

    /////////////////////////////////
    public Vector3 LastGoodWorldPos
    {
        get => lastGoodWorldPos;
        set => lastGoodWorldPos = value;
    }

    public Quaternion LastGoodWorldRot
    {
        get => lastGoodWorldRot;
        set => lastGoodWorldRot = value;
    }
    /////////////////////////////////

    private void Start()
    {
        lastGoodWorldPos = trackedCamera.position;
    }

    void Update()
    {
        if (trackedCamera == null) return;

        Vector3 currentPos = trackedCamera.position;

        // Detect bad data
        bool isNaN = float.IsNaN(currentPos.x) || float.IsNaN(currentPos.y) || float.IsNaN(currentPos.z);
        bool isInfinity = float.IsInfinity(currentPos.x) || float.IsInfinity(currentPos.y) || float.IsInfinity(currentPos.z);
        bool isTooFar = isFirstValid && Vector3.Distance(currentPos, lastGoodWorldPos) > maxAllowedDistance;

        if (isNaN || isInfinity || isTooFar)
        {
            Debug.LogWarning("Tracking error detected. Shifting parent to restore camera position.");

            // Optional: restore orientation by adjusting rotation of the parent
            Quaternion deltaRot = lastGoodWorldRot * Quaternion.Inverse(trackedCamera.rotation);
            gameObject.transform.rotation = deltaRot * gameObject.transform.rotation; // not sure about this

            // Compute how far off the camera is, and move the parent to correct it
            Vector3 delta = lastGoodWorldPos - trackedCamera.position;
            gameObject.transform.position += delta; // not sure about this
        }
        else
        {
            lastGoodWorldPos = trackedCamera.position;
            lastGoodWorldRot = trackedCamera.rotation;
            isFirstValid = true;
        }
    }
}
