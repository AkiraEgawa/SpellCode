using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 8f;
    public float jumpForce = 16f;

    private float horizontalInput;
    private bool isFacingRight = true;
    private bool jumpHeld;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Update()
    {
        float x = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x += 1f;
        }

        if (Gamepad.current != null)
        {
            float stick = Gamepad.current.leftStick.x.ReadValue();
            if (Mathf.Abs(stick) > 0.1f) x = stick;
        }

        horizontalInput = x;

        bool jumpNow =
            (Keyboard.current != null && Keyboard.current.spaceKey.isPressed) ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.isPressed);

        bool jumpPressed = jumpNow && !jumpHeld;
        bool jumpReleased = !jumpNow && jumpHeld;
        jumpHeld = jumpNow;

        if (jumpPressed && IsGrounded())
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (jumpReleased && rb.linearVelocity.y > 0f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
        Flip();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}

