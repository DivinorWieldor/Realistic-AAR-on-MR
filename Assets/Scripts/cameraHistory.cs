using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraHistory : MonoBehaviour
{
    [System.Serializable]
    public class PoseRecord
    {
        public float time;
        public Vector3 position;
        public Quaternion rotation;

        public PoseRecord(float t, Vector3 pos, Quaternion rot)
        {
            time = t;
            position = pos;
            rotation = rot;
        }
    }

    public Transform trackedCamera;
    public trackingErrFix trackingErrFixScript;
    public float historyDuration = 30f; // seconds
    public KeyCode restoreKey = KeyCode.Z;
    public float restoreTimeOffset = 5f; // how far back to go

    private List<PoseRecord> history = new List<PoseRecord>();

    void Update()
    {
        if (trackedCamera == null) return;

        float now = Time.time;

        // Record current pose
        history.Add(new PoseRecord(now, trackedCamera.position, trackedCamera.rotation));

        // Remove old poses
        while (history.Count > 0 && now - history[0].time > historyDuration)
        {
            history.RemoveAt(0);
        }

        // Manual restore
        if (Input.GetKeyDown(restoreKey))
        {
            trackingErrFixScript.enabled = false;
            float targetTime = now - restoreTimeOffset;
            PoseRecord closest = null;

            foreach (var record in history)
            {
                if (record.time <= targetTime)
                {
                    closest = record;
                }
                else
                {
                    break;
                }
            }

            if (closest != null)
            {
                Debug.Log($"Restoring camera to pose from {now - closest.time:F1} seconds ago");

                // Calculate delta and apply to parent
                // Extract Y-axis rotation (yaw) from both quaternions
                float targetYaw = closest.rotation.eulerAngles.y;
                float currentYaw = trackedCamera.rotation.eulerAngles.y;

                // Compute yaw difference
                float yawDelta = Mathf.DeltaAngle(currentYaw, targetYaw);

                // Apply Y rotation delta to the parent
                gameObject.transform.Rotate(Vector3.up, yawDelta, Space.World);

                Vector3 posDelta = closest.position - trackedCamera.position;
                gameObject.transform.position += posDelta;
            }
            else
            {
                Debug.LogWarning("No suitable pose in history to restore to.");
            }

            trackingErrFixScript.LastGoodWorldPos = trackedCamera.position;
            trackingErrFixScript.LastGoodWorldRot = trackedCamera.rotation;
            trackingErrFixScript.enabled = true;
        }
    }
}
