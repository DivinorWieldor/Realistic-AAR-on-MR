using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Character movement script
public class CharacterMovement : MonoBehaviour
{
    public Camera playerCamera;

    public float speed = 5f; // The movement speed of the character
    private float pitch = 0f;
    public float maxPitch = 90f; // Maximum
    public float rotationSpeed = 100f; // The rotation speed of the character

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor to the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Cursor.lockState == CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // Get the horizontal and vertical input values
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction based on the input values
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Move the character in the movement direction
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.Self);

        #region Camera_Movement_Mouse
        // Get the mouse movement values
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the character based on the mouse X movement (Yaw)
        transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);

        // Calculate new pitch value and clamp it to prevent flipping
        pitch -= mouseY * rotationSpeed * Time.deltaTime; // Subtracting to invert the vertical axis
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        // Apply the pitch (vertical rotation) directly to the camera
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
        #endregion Camera_Movement_Mouse

        // Check if the user pressed 'e' to move the camera up
        if (Input.GetKey(KeyCode.E))
        {
            Camera.main.transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        }

        // Check if the user pressed 'q' to move the camera down
        if (Input.GetKey(KeyCode.Q))
        {
            Camera.main.transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        }
    }
}
