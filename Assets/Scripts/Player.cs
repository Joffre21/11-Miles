using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 playerPosition;
    [Header("Mouse Look")]
    [SerializeField] float mouseSensitivity = 100f;
    bool cursorLocked = true;
    [Header("Yaw Limits")]
    [Tooltip("Minimum allowed yaw in degrees (negative = left of world forward)")]
    [SerializeField] float minYaw = -90f;
    [Tooltip("Maximum allowed yaw in degrees (positive = right of world forward)")]
    [SerializeField] float maxYaw = 90f;

    [Tooltip("Optional: camera/head pivot to apply pitch to (not required for yaw-only limiting)")]
    [SerializeField] Transform headTransform;

    [Header("Pitch Limits")]
    [Tooltip("Minimum pitch (look down) in degrees, negative values look down depending on your setup")]
    [SerializeField] float minPitch = -40f;
    [Tooltip("Maximum pitch (look up) in degrees")]
    [SerializeField] float maxPitch = 40f;
    [Tooltip("Invert vertical mouse input")]
    [SerializeField] bool invertY = false;

    // runtime accumulated angles
    float currentYaw;
    float currentPitch;
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = transform.position;
        // Lock the cursor for FPS-style mouse look
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize currentYaw from the transform's world Y rotation (convert to signed -180..180)
        currentYaw = transform.eulerAngles.y;
        if (currentYaw > 180f) currentYaw -= 360f;

        // If there's a head/camera pivot, initialize pitch too (local X)
        if (headTransform != null)
        {
            currentPitch = headTransform.localEulerAngles.x;
            if (currentPitch > 180f) currentPitch -= 360f;
        }
    }

    // Update is called once per frame
    void Update()
    {

        // Read mouse deltas
        float dx = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float dy = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        // Apply invert option
        if (invertY) dy = -dy;

        // Accumulate
        currentYaw += dx;
        currentPitch += dy;

        // Clamp yaw so the player cannot look behind
        currentYaw = Mathf.Clamp(currentYaw, minYaw, maxYaw);

        // Clamp pitch if headTransform is assigned
        if (headTransform != null)
        {
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
            headTransform.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);
        }

        

        // Allow unlocking the cursor with Escape (useful during development)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLocked = !cursorLocked;
            Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !cursorLocked;
        }
    }

    void LateUpdate()
    {
        // Apply yaw to player body
        transform.rotation = Quaternion.Euler(0f, currentYaw, 0f);
    }

}
