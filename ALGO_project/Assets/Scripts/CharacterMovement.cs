using UnityEngine;
using UnityEngine.InputSystem; // 새 Input System

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;       // 기본 이동 속도
    public float jumpForce = 10f;      // 점프 힘
    public float slideFriction = 5f;   // 착지 후 감속 속도
    public float slideDuration = 0.3f; // 착지 후 입력 잠금 시간

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGroundedLastFrame;
    private float moveInput;
    private bool isSliding; // 착지 후 미끄러짐 상태

    [Header("바닥 체크")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 바닥 체크
        wasGroundedLastFrame = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 땅 위 + 미끄러짐 중이 아닐 때만 이동 입력 받기
        if (isGrounded && !isSliding)
        {
            moveInput = 0f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                moveInput = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                moveInput = 1f;

            if (moveInput != 0)
                spriteRenderer.flipX = moveInput < 0;
        }

        // 점프 (공중에서는 방향 조절 불가)
        if ((Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        if (isGrounded && !isSliding)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
        else if (isSliding)
        {
            // 감속 처리
            float velocityX = Mathf.MoveTowards(rb.linearVelocity.x, 0f, slideFriction * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector2(velocityX, rb.linearVelocity.y);
        }

        // 착지 순간 미끄러짐 시작
        if (!wasGroundedLastFrame && isGrounded)
        {
            StartCoroutine(SlideAfterLanding());
        }
    }

    private System.Collections.IEnumerator SlideAfterLanding()
    {
        isSliding = true;

        // 입력 잠금 시간
        yield return new WaitForSeconds(slideDuration);

        isSliding = false;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
