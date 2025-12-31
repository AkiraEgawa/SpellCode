using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 8f;
    public float jumpForce = 16f;
    public int maxJumps = 2;

    public float wallSlideSpeed = 2f;
    public float wallJumpForceX = 12f;
    public float wallJumpForceY = 16f;
    public float wallJumpLockTime = 0.15f;

    private float horizontalInput;
    private bool isFacingRight = true;
    private bool jumpHeld;
    private int jumpsLeft;

    private bool isWallSliding;
    private float wallJumpLockTimer;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    public float wallCheckRadius = 0.2f;

    void Awake()
    {
        jumpsLeft = maxJumps;
    }

    void Update()
    {
        if (IsGrounded() && rb.linearVelocity.y <= 0.01f) 
        {
            jumpsLeft = maxJumps;
        }

        float x = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) 
            {
                x -= 1f;
            }
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) 
            {
                x += 1f;
            }
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

        bool touchingWall = IsTouchingWall();
        bool grounded = IsGrounded();

        isWallSliding = touchingWall && !grounded && rb.linearVelocity.y < 0f && Mathf.Abs(horizontalInput) > 0.01f;

        if (jumpPressed)
        {
            if (isWallSliding)
            {
                int wallDir = isFacingRight ? 1 : -1;
                rb.linearVelocity = new Vector2(-wallDir * wallJumpForceX, wallJumpForceY);
                wallJumpLockTimer = wallJumpLockTime;
                jumpsLeft = Mathf.Max(jumpsLeft, maxJumps - 1);
            }
            else if (jumpsLeft > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpsLeft--;
            }
        }

        if (jumpReleased && rb.linearVelocity.y > 0f) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (wallJumpLockTimer > 0f)
        {
            wallJumpLockTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (isWallSliding)
        {
            float y = Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, y);
        }

        if (wallJumpLockTimer <= 0f) 
        {
            rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
        }

        Flip();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsTouchingWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
    }

    private void Flip()
    {
        if (isWallSliding) 
        {
            return;
        }

        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}


