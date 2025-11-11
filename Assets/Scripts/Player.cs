using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 playerPosition;
    [Header("Mouse Look")]
    [SerializeField] float mouseSensitivity = 100f;
    bool cursorLocked = true;
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = transform.position;
        // Lock the cursor for FPS-style mouse look
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

        // Yaw rotation (rotate around the Y axis) from mouse horizontal movement
        // "Mouse X" from the Input Manager is used here. Multiply by sensitivity and Time.deltaTime
        // to keep the rotation frame-rate independent.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // Allow unlocking the cursor with Escape (useful during development)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLocked = !cursorLocked;
            Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !cursorLocked;
        }
    }

}
