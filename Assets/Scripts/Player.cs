using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
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


    [Header("Speed Progression")]
    [SerializeField] float speed = 10f;
    [SerializeField] float speedIncreasePerSecond = 2f;
    [SerializeField] float maxSpeed = 25f;
    [SerializeField] string targetSceneName = "Start Menu";
    private bool triggeredSceneChange = false;

    [Header("Turn/Strafe Speed")]
    [SerializeField] float turnSpeed = 10f;
    [SerializeField] private Vector3 targetPosition = new Vector3(16.988f, 5.994f, 0.312f);
    [SerializeField] private float returnSpeed = 5f;

    public float CurrentSpeed => speed;

    [SerializeField] Car car;

    private GameManager gm;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
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
        if (gm == null || !gm.GameStarted)
            return;

        // Speed up over time
        speed += speedIncreasePerSecond * Time.deltaTime;

        // Check for scene change
        if (!triggeredSceneChange && speed >= maxSpeed)
        {
            triggeredSceneChange = true;
            SceneManager.LoadScene(targetSceneName);
            return;
        }

        // Handle left/right movement (A/D or Left/Right Arrow)
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 (A/Left), 1 (D/Right)
        if (horizontalInput != 0f)
        {
            // Move left/right in world X axis, scaled by turnSpeed
            float moveAmount = horizontalInput * turnSpeed * Time.deltaTime;
            transform.position += new Vector3(moveAmount, 0f, 0f);
            if (car != null)
            {
                car.UpdateCarTransform(moveAmount);
            }
        }
        else
            returnToInitialPosition();

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
        if (gm == null || !gm.GameStarted)
            return;
        // Apply yaw to player body
        transform.rotation = Quaternion.Euler(0f, currentYaw, 0f);
    }

    void returnToInitialPosition()
    {
        if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                returnSpeed * Time.deltaTime
            );
        }
    }
}
