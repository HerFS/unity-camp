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
    public bool isGrounded;
    public bool isInteraction;

    protected const float CONVERT_UINT_VALUE = 0.1f;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Move();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        RaycastHit hit;
        if (Physics.Raycast(FootRaycastPos.transform.position, Vector3.down, out hit, 0.1f))
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

    void Move()
    {
        bool hasControl = (moveDirection != Vector3.zero);

        if (hasControl)
        {
            //float currentMoveSpeed = moveSpeed * CONVERT_UINT_VALUE;
            LookAt();
            rb.velocity = moveDirection * moveSpeed + Vector3.up * rb.velocity.y;
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

    #region SEND_MESSAGE

    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (input != null)
        {
            moveDirection = new Vector3(input.x, 0f, input.y);
            Debug.Log($"SEND MESSAGE : {input.magnitude}");
        }
    }

    void OnJump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        rb.velocity = Vector3.zero; // 위의 AddForce 의 추가 값을 없애줌 (통통 튀는 현상)
        Debug.Log("Input Event Jump");
    }
    #endregion

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
