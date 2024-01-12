using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    public Transform FootRaycastPos;
    public Vector3 moveDirection;
    public float moveSpeed = 4f;
    public float jumpForce = 4f;
    public float runValue = 1f;
    public bool isGrounded;
    public bool isInteraction;

    const float RAY_DISTANCE = 2f;
    RaycastHit slopeHit;
    [Header("slope")]
    public int groundLayer; // 땅 레이어만 체크
    public float maxSlopeAngle;
    public float angle;
    public bool isSlope = false;
    public Vector3 direction;

    protected const float CONVERT_UINT_VALUE = 0.1f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    void FixedUpdate()
    {
        Move();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Physics.Raycast(FootRaycastPos.transform.position, Vector3.down, 0.1f))
        {
            Debug.DrawRay(FootRaycastPos.transform.position, Vector3.down * 0.1f, Color.red);
            isGrounded = true;
        }
        else
        {
            //Debug.DrawRay(FootRaycastPos.transform.position, Vector3.down * 0.1f);
            isGrounded = false;
        }
    }
    void Update()
    {
        isSlope = IsOnSlope();
        FreezePosition();
    }

    void FreezePosition()
    {
        if (moveDirection == Vector3.zero)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
    void Move()
    {
        bool hasControl = (moveDirection != Vector3.zero);

        Vector3 velocity = isSlope ? AdjustDirectionToSlope(moveDirection) : moveDirection;
        Vector3 gravity = isSlope ? Vector3.zero : Vector3.down * Mathf.Abs(rb.velocity.y);

        if (hasControl)
        {
            //float currentMoveSpeed = moveSpeed * CONVERT_UINT_VALUE;
            LookAt();
            //rb.velocity = moveDirection * runValue * moveSpeed + Vector3.up * rb.velocity.y;
            rb.velocity = velocity * moveSpeed + gravity * runValue;
            //transform.rotation = Quaternion.LookRotation(moveDirection);
            //transform.eulerAngles = moveDirection;
            //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }
    void LookAt()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(moveDirection);
            rb.rotation = targetAngle;
        }
    }

    bool IsOnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out slopeHit, RAY_DISTANCE, groundLayer))
        {
            Debug.DrawLine(slopeHit.point, slopeHit.point + slopeHit.normal, Color.blue);
            angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < maxSlopeAngle;
        }

        return false;
    }

    protected Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    //#region SEND_MESSAGE

    //void OnMove(InputValue value)
    //{
    //    Vector2 input = value.Get<Vector2>();
    //    if (input != null)
    //    {
    //        moveDirection = new Vector3(input.x, 0f, input.y);
    //        Debug.Log($"SEND MESSAGE : {input.magnitude}");
    //    }
    //}

    //void OnJump()
    //{
    //    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    //    rb.velocity = Vector3.zero; // 위의 AddForce 의 추가 값을 없애줌 (통통 튀는 현상)
    //    Debug.Log("Input Event Jump");
    //}
    //#endregion

    #region UNITY_EVENETS
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input != null)
        {
            moveDirection = new Vector3(input.x, 0f, input.y);
            Debug.Log($"UNITY EVENETS : {input.magnitude}");
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            runValue = 2f;
        }
        else
        {
            runValue = 1f;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            rb.velocity = Vector3.zero; // 위의 AddForce 의 추가 값을 없애줌 (통통 튀는 현상)
            Debug.Log("Input Event Jump");
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isInteraction = true;
        }
        else
        {
            isInteraction = false;
        }
    }

    #endregion
}
