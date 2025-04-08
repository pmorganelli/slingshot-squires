using UnityEngine;

public class PlayerAnimation: MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;
    Vector2 lastMoveDir = Vector2.down; // Default idle direction

    void Update()
    {
        // Get WASD or arrow key input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Optional: prevent diagonal speed boost
        if (movement.sqrMagnitude > 1)
            movement.Normalize();

        // Update animator parameters
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);

        // Store the last direction (for idle facing)
        if (movement != Vector2.zero)
            lastMoveDir = movement;
    }

    void FixedUpdate()
    {
        // Apply movement using Rigidbody2D
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
