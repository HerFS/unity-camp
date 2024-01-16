using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]

public class PlayerController : MonoBehaviour
{
    Animator animator; // animation
    Rigidbody rb;

    [Header("Movement")]
    [SerializeField] Vector3 moveDirection;
    public float walkSpeed;
    [SerializeField] float runSpeed;
    bool isRun;

    [Header("Jump")]
    [SerializeField] Transform nowTransform;
    [SerializeField] float height;
    [SerializeField] float posY;
    [SerializeField] float jumpGravity;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpTime;
    [SerializeField] bool isJumping;

    RaycastHit slopeHit;
    [Header("Slope")]
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform frontSlopeCheck;
    [SerializeField] int groundLayer;
    [SerializeField] float slopeRayDistance;
    [SerializeField] float angle;
    [SerializeField] float maxSlopeAngle;
    [SerializeField] bool isOnSlope;
    [SerializeField] bool isGrounded;
    [SerializeField] Vector3 velocity;
    [SerializeField] Vector3 gravity;
    Vector3 frontSlopeCheckPos = new Vector3(0f, 0f, 0f);

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        groundLayer = 1 << LayerMask.NameToLayer("Ground");

        // jump data define
        nowTransform = transform;
        posY = transform.position.y;
        jumpGravity = 9.81f;
        jumpTime = 0.0f;
        isJumping = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 boxSize = new Vector3(transform.lossyScale.x / 2, 0.1f, transform.lossyScale.z / 2);
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
    }
    private void Update()
    {
        FreezeConstraints();
    }

    private void FixedUpdate()
    {
        Move();

        if (isJumping)
        {
            Jump();
        }
    }

    void Move()
    {
        bool hasControl = (moveDirection != Vector3.zero);
        float animationPlayerMoveValue = isRun ? 1f : 0f;

        isOnSlope = IsOnSlope();
        isGrounded = IsGrounded();
        gravity = Vector3.down * Mathf.Abs(rb.velocity.y);

        animator.SetBool("isMove", false);

        //CheckSlopePos();

        if (hasControl)
        {
            if (isGrounded && isOnSlope)
            {
                gravity = Vector3.zero;
            }

            LookAt();
            velocity = CalculateNextFrameGroundAngle(RunCheck()) <= maxSlopeAngle ? AdjustDirectionToSlope(moveDirection) : Vector3.zero;
            rb.velocity = velocity * RunCheck() + gravity;

            animator.SetBool("isMove", true);

            animator.SetFloat("Velocity", animationPlayerMoveValue);
        }
    }

    void Jump()
    {
        // "y(오브젝트의 높이) = a(중력가속도) * x(점프 시간) + b(초기 점프 속도)" 에서
        // 적분하여 y = (-a/2) * x^2 + (b * x)공식을 얻는다.
        // 변화된 높이 height를 기존 높이 posY에 더한다.
        height = (jumpTime * jumpTime * (-jumpGravity) / 2) + (jumpTime * jumpForce);
        nowTransform.position = new Vector3(nowTransform.position.x, posY + height, nowTransform.position.z);
        jumpTime += Time.fixedDeltaTime;

        //if (isOnSlope)
        //{
        //    isJumping = false;
        //    jumpTime = 0.0f;
        //}

        if (height < 0.0f || isOnSlope)
        {
            isJumping = false;
            jumpTime = 0.0f;
            nowTransform.position = new Vector3(nowTransform.position.x, posY, nowTransform.position.z);
        }
    }

    void LookAt()
    {
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    float RunCheck()
    {
        return isRun ? runSpeed : walkSpeed;
    }

    void FreezeConstraints()
    {
        if (moveDirection == Vector3.zero && isOnSlope && isGrounded)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
        else if (moveDirection == Vector3.zero)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    #region Slope
    private bool IsOnSlope()
    {
        Ray ray = new Ray(frontSlopeCheck.position, Vector3.down);
        if (Physics.Raycast(ray, out slopeHit, slopeRayDistance, groundLayer))
        {
            angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            Debug.DrawLine(slopeHit.point, slopeHit.point + slopeHit.normal, Color.blue);

            return angle != 0f && angle <= maxSlopeAngle;
        }
        return false;
    }

    private Vector3 AdjustDirectionToSlope(Vector3 Direction)
    {
        Vector3 adjustVelocityDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
        return adjustVelocityDirection;
    }

    public bool IsGrounded()
    {
        Vector3 boxSize = new Vector3(transform.lossyScale.x / 2, 0.1f, transform.lossyScale.z / 2);
        return Physics.CheckBox(groundCheck.position, boxSize, Quaternion.identity, groundLayer);
    }

    private float CalculateNextFrameGroundAngle(float moveSpeed)
    {
        Vector3 nextFramePlayerPosition = frontSlopeCheck.position + moveDirection * moveSpeed * Time.fixedDeltaTime; // next frame location information
        if (Physics.Raycast(nextFramePlayerPosition, Vector3.down, out RaycastHit hitInfo, slopeRayDistance, groundLayer))
        {
            return Vector3.Angle(Vector3.up, hitInfo.normal);
        }
        return 0f;
    }

    //private void CheckSlopePos() // fix me
    //{
    //    frontSlopeCheckPos.y = 0.5f;

    //    //Debug.Log("transform" + transform.TransformDirection(transform.position));
    //    //Debug.Log("frontslope" + frontSlopeCheckPos);
    //    //Debug.Log("slopeHit" + slopeHit.point);
    //    SlopeCheckPosFollowToPlayer();
    //    if (slopeHit.point.y > transform.position.y && isOnSlope)
    //    {
    //        frontSlopeCheckPos.x = transform.position.x;
    //        frontSlopeCheckPos.z = transform.position.z;
    //        Debug.Log(moveDirection);
    //        // slopeCheck.position.z = 0.5
    //        // (if slope -> ground 도달하면 == isSlopeOn false 일때) slopeCheck.position.z = 0 
    //    }
    //    else
    //    {
    //        // slopeCheck position.z = 0
    //        frontSlopeCheckPos.x = transform.position.x;
    //        frontSlopeCheckPos.z = transform.position.z;
    //    }
    //}

    //private void SlopeCheckPosFollowToPlayer() // fix me
    //{
    //    if (moveDirection.z == 1f && !isOnSlope)
    //    {
    //        frontSlopeCheckPos.x = transform.position.x;
    //        frontSlopeCheckPos.z = transform.position.z + 0.3f;
    //    }
    //    else if (moveDirection.z == -1f && !isOnSlope)
    //    {
    //        frontSlopeCheckPos.x = transform.position.x;
    //        frontSlopeCheckPos.z = transform.position.z - 0.3f;
    //    }
    //    else if (moveDirection.x == 1f && !isOnSlope)
    //    {
    //        frontSlopeCheckPos.x = transform.position.x + 0.3f;
    //        frontSlopeCheckPos.z = transform.position.z;
    //    }
    //    else if (moveDirection.x == -1f && !isOnSlope)
    //    {
    //        frontSlopeCheckPos.x = transform.position.x - 0.3f;
    //        frontSlopeCheckPos.z = transform.position.z;
    //    }
    //    else if (isOnSlope)
    //    {
    //        frontSlopeCheckPos.x = transform.position.x;
    //        frontSlopeCheckPos.z = transform.position.z;
    //    }
    //}

    #endregion


    #region INPUT_EVENTS
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input != null)
        {
            moveDirection = new Vector3(input.x, 0f, input.y).normalized;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log($"{context.phase}");
        if (context.performed && !isJumping /*&& !isOnSlope*/)
        {
            isJumping = true;
            posY = transform.position.y;
        }
    }
    #endregion
}