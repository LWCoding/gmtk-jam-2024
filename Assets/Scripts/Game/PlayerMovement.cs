using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Object Assignments")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rb;
    [Header("Player Properties")]
    public float MoveSpeed;
    public float Acceleration;
    public float Deceleration;

    private Vector2 _movement;
    private Vector2 _velocity;

    private void Update()
    {
        LookTowardsMouse();
        CalculateMovementVector();
    }

    /// <summary>
    /// Make this character look towards the mouse (top-down 2D perspective).
    /// </summary>
    public void LookTowardsMouse()
    {
        // Make the player point towards the mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        direction.z = 0f; // Ignore the z-axis since it's a 2D game
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _spriteRenderer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    /// <summary>
    /// Based on player inputs, calculate and store movement vector data into
    /// the movement variable read by FixedUpdate.
    /// </summary>
    public void CalculateMovementVector()
    {
        // Get input from the player
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Set the movement vector based on input
        _movement = new Vector2(moveX, moveY).normalized;
    }

    private void FixedUpdate()
    {
        // Calculate target velocity based on input and moveSpeed
        Vector2 targetVelocity = _movement * MoveSpeed;

        // Accelerate towards the target velocity
        _velocity = Vector2.MoveTowards(_velocity, targetVelocity,
            (_movement.magnitude > 0 ? Acceleration : Deceleration) * Time.fixedDeltaTime);

        // Apply the current velocity to the player
        _rb.velocity = _velocity;
    }

}
