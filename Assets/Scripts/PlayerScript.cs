using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerScript : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Units per second")]
    public float moveSpeed = 5f;

    // Cached components
    Rigidbody2D rb2D;
    CapsuleCollider2D capCollider;

    void Awake()
    {
        // Grab required components
        rb2D         = GetComponent<Rigidbody2D>();
        capCollider  = GetComponent<CapsuleCollider2D>();

        // Zero out gravity (top-down game)
        rb2D.gravityScale = 0f;

        // Freeze rotation so capsule never tips
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Interpolate Rigidbody for smooth movement
        rb2D.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        // Read raw input (A/D, W/S or arrow keys)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Build a direction vector
        Vector2 dir = new Vector2(h, v);

        // Normalize so diagonal isn't faster
        if (dir.sqrMagnitude > 1f)
            dir.Normalize();

        // Set Rigidbody velocity
        rb2D.linearVelocity = dir * moveSpeed;
    }

    void OnDisable()
    {
        // Stop any leftover sliding if script gets disabled
        if (rb2D != null) rb2D.linearVelocity = Vector2.zero;
    }
}