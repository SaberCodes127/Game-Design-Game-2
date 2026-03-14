using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    bool isFlipped = false;
    bool isOnWall;
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float maxJumps = 2;
    float jumps = 0;
    float dir = 0f;
    float horizontalMovement;
    float maxFallSpeed = 10f;

    [Header("Jumping")]
    public float jumpForce = 10f;

    [Header("GroundCheck & WallCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isFacingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        IsTouchingWall();
        Debug.Log(isOnWall);
        if (!Direction() && !isFlipped)
        {
            sprite.flipX = true;
            isFlipped = true;
            wallCheckPos.position = new Vector3(wallCheckPos.position.x-0.6f, wallCheckPos.position.y, wallCheckPos.position.z);
        } else if (Direction() && isFlipped)
        {
            sprite.flipX = false;
            isFlipped = false;
            wallCheckPos.position = new Vector3(wallCheckPos.position.x+0.6f, wallCheckPos.position.y, wallCheckPos.position.z);
        }

        Moving();
    }


    private bool Direction()    //false == facing left & true == facing right
    {
        if (dir < 0)
        {
            return false;
        } else if(dir > 0)
        {
            return true;
        }
        return !isFlipped;

        FlipSprite();
    }

    public void Move(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>().x;
    }

    public void Moving () {
        if (!isOnWall)
        {
            horizontalMovement = dir;
        } else
        {
            horizontalMovement = 0f;
        }
        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(IsGrounded() || (jumps < maxJumps)){
            if (context.performed)
            {
                jumps++;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y *0.5f);
            }
        }
    }

    private bool IsGrounded(){
        if(Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumps = 0;
            return true;
        }
        return false;
    }

    private void IsTouchingWall(){
        if(Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, groundLayer))
        {
            if(!isOnWall && jumps > 0) {
                jumps--;
            }
            if (rb.linearVelocity.y < 0f)
            {
                maxFallSpeed = 2f;
            } else
            {
                maxFallSpeed = 8f;
            }
            isOnWall = true;
        } else
        {
            maxFallSpeed = 8f;
            isOnWall = false;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }

    void FlipSprite()
    {
        if (isFacingRight && horizontalMovement < 0f || !isFacingRight && horizontalMovement > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x = -ls.x;
            transform.localScale = ls;
        }
    }
}
