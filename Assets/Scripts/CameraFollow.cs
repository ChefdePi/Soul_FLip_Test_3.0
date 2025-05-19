using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Drag your player GameObject here")]
    public Transform target;

    [Tooltip("How quickly the camera catches up; higher = snappier")]
    public float smoothing = 5f;

    // The offset from the target (so camera stays at correct Z)
    Vector3 offset;

    void Start()
    {
        // Compute initial offset based on your scene setup:
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // Where the camera *wants* to go
        Vector3 desired = target.position + offset;

        // Smoothly interpolate from current to desired
        transform.position = Vector3.Lerp(
            transform.position,
            desired,
            smoothing * Time.deltaTime
        );
    }
}
