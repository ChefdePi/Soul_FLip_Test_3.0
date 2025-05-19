using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Drag your player GameObject here")]
    public Transform target;

    [Tooltip("How quickly the camera catches up; higher = snappier")]
    public float smoothing = 5f;

    private Vector2 xyOffset;
    private float fixedZ;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollow: no target assigned!");
            enabled = false;
            return;
        }

        // Lock in your original camera Z-depth
        fixedZ = transform.position.z;

        // Calculate only the XY offset
        Vector3 delta = transform.position - target.position;
        xyOffset = new Vector2(delta.x, delta.y);

        // Snap instantly to the correct spot so you start centered
        transform.position = new Vector3(
            target.position.x + xyOffset.x,
            target.position.y + xyOffset.y,
            fixedZ
        );
    }

    void LateUpdate()
    {
        // Build the smooth target position
        Vector3 desired = new Vector3(
            target.position.x + xyOffset.x,
            target.position.y + xyOffset.y,
            fixedZ
        );

        // Lerp only if you’re off by more than a hair
        transform.position = Vector3.Lerp(
            transform.position,
            desired,
            smoothing * Time.deltaTime
        );
    }
}