using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class Car : MonoBehaviour
{
    [SerializeField] string targetSceneName = "Start Menu";
    [SerializeField] private float spinSpeed = 200f;
    [SerializeField] private Vector3 targetPosition = new Vector3(17.536f, 4.6207f, -0.16f);
    [SerializeField] private float returnSpeed = 5f;
    private Collider carCollider;
    private Transform steeringWheel;
    void Awake()
    {
        steeringWheel = transform.Find("CICADA_HI/BODY/inside/steering_wheel");
        carCollider = transform.Find("CICADA_COLLIDER").GetComponent<Collider>();
    }
    void Update()
    {
        // Example: Check if carCollider is overlapping any colliders with the "Crasher" tag
        Collider[] hits = Physics.OverlapBox(
            carCollider.bounds.center,
            carCollider.bounds.extents,
            carCollider.transform.rotation
        );

        foreach (var hit in hits)
        {
            if (hit != carCollider && hit.CompareTag("Crasher"))
            {
                SceneManager.LoadScene(targetSceneName);
                break;
            }
        }
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput == 0 && Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            returnToInitialPosition();
        }
    }
    public void UpdateCarTransform(float moveAmount)
    {
        transform.position += new Vector3(moveAmount, 0f, 0f);
        RotateSteeringWheel(moveAmount);
    }

    void RotateSteeringWheel(float moveAmount)
    {
        steeringWheel.Rotate(0f, 0f, moveAmount * spinSpeed);
        transform.Rotate(0f, moveAmount, 0f);
    }

    void returnToInitialPosition()
    {
        transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                returnSpeed * Time.deltaTime
            );
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.identity,
            returnSpeed * Time.deltaTime
        );
        Vector3 currentEuler = steeringWheel.localEulerAngles;
        if (currentEuler.z > 180f) currentEuler.z -= 360f;

        float newZ = Mathf.Lerp(currentEuler.z, 0f, returnSpeed * Time.deltaTime);
        steeringWheel.localRotation = Quaternion.Euler(currentEuler.x, currentEuler.y, newZ);
    }
}
