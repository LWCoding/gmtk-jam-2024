using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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

    void Update()
    {
        // Get input from the player
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Set the movement vector based on input
        _movement = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
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
