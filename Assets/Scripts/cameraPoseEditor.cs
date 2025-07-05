using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(cameraHistory))]  // Or any manager component
public class cameraPoseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Align Camera to Scene View"))
        {
            cameraHistory poseHistory = (cameraHistory)target;

            if (poseHistory.trackedCamera != null)
            {
                SceneView sceneView = SceneView.lastActiveSceneView;
                if (sceneView != null)
                {
                    poseHistory.trackingErrFixScript.enabled = false; // Disable tracking error fix script

                    Vector3 sceneViewPos = sceneView.camera.transform.position;
                    Quaternion sceneViewRot = sceneView.camera.transform.rotation;

                    // Reposition the parent so the trackedCamera matches scene view
                    Transform camera = poseHistory.trackedCamera;
                    Transform parent = poseHistory.transform;

                    // apply Y-axis rotation correction
                    float targetYaw = sceneViewRot.eulerAngles.y;
                    float currentYaw = camera.rotation.eulerAngles.y;
                    float yawDelta = Mathf.DeltaAngle(currentYaw, targetYaw);
                    parent.Rotate(Vector3.up, yawDelta, Space.World);

                    // apply position correction
                    Vector3 posDelta = sceneViewPos - camera.position;
                    parent.position += posDelta;

                    // update autamic tracking error fix script
                    poseHistory.trackingErrFixScript.LastGoodWorldPos = poseHistory.trackedCamera.position;
                    poseHistory.trackingErrFixScript.LastGoodWorldRot = poseHistory.trackedCamera.rotation;
                    poseHistory.trackingErrFixScript.enabled = true; // Re-enable tracking error fix script

                    Debug.Log("Aligned camera to scene view.");
                }
                else
                {
                    Debug.LogWarning("No active Scene view found.");
                }
            }
            else
            {
                Debug.LogWarning("CameraHistory is missing trackedCamera.");
            }
        }
    }
}
