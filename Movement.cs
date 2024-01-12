using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
//[RequireComponent(typeof())]

public class Movement : MonoBehaviour
{
    float vAxis;
    float hAxis;
    bool isRun = false;
    public float moveSpeed = 3f;
    public float runSpeed = 3f;
    public float jumpForce = 3f;

    Rigidbody rigid;

    public Transform FootRaycastPos;
    [SerializeField]
    private Color _rayColor = Color.red;

    bool jumpDown;
    public bool isGround;


    const float RAY_DISTANCE = 2f;
    RaycastHit slopeHit;
    [Header("slope")]
    public int groundLayer; // 땅 레이어만 체크
    public float maxSlopeAngle;
    public float angle;
    public bool isSlope = false;
    public Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = true;
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        //rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        //transform.Translate(new Vector3(1f, 1f, 1f)); // 지금 위치에서 지정한 값을 더함
        //transform.position = new Vector3(1f, 1f, 1f); // 지정한 값으로 위치 이동
    }

    void FixedUpdate()
    {
        //Vector3 moveMent = new Vector3(hAxis * Time.fixedDeltaTime, 0, vAxis * Time.fixedDeltaTime);
        //transform.Translate(moveMent);
        //rigid.velocity = new Vector3(hAxis, 0, vAxis);
        Move();
        Jump();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        FreezePosition();
        isSlope = IsOnSlope();
        AdjustDirectionToSlope(direction);
        //isRun = Input.GetKey(KeyCode.LeftShift);
        //if (isRun == true)
        //{
        //    vAxis = Input.GetAxis("Vertical") * moveSpeed * 2f;
        //    hAxis = Input.GetAxis("Horizontal") * moveSpeed * 2f;
        //} else
        //{
        //    vAxis = Input.GetAxis("Vertical") * moveSpeed;
        //    hAxis = Input.GetAxis("Horizontal") * moveSpeed;
        //}
    }

    void FreezePosition()
    {
        if (vAxis == 0 || hAxis == 0)
        {
            rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
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


    void OnDrawGizmos()
    {
        Gizmos.color = _rayColor;
        RaycastHit hit;
        if (Physics.Raycast(FootRaycastPos.transform.position, Vector3.down, out hit, 0.1f))
        {
            Debug.DrawRay(FootRaycastPos.transform.position, Vector3.down * 0.1f, _rayColor);
            isGround = true;
        } else
        {
            //Debug.DrawRay(FootRaycastPos.transform.position, Vector3.down * 0.1f);
            isGround = false;
        }
    }

    void GetInput()
    {
        vAxis = Input.GetAxisRaw("Vertical");
        hAxis = Input.GetAxisRaw("Horizontal");
        isRun = Input.GetKey(KeyCode.LeftShift);
        jumpDown = Input.GetKey(KeyCode.LeftAlt);
        direction = new Vector3(vAxis, 0f, hAxis);
    }

    void Move()
    {
        Vector3 movement = new Vector3(hAxis, 0, vAxis).normalized;
        //Vector3 veleocity = isSlope ? AdjustDirectionToSlope(Vector3 direction) : direction;

        if (isRun)
        {
            transform.Translate(movement * runSpeed * Time.fixedDeltaTime);
        } else
        {
            transform.Translate(movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void Jump()
    {
        if (isGround && jumpDown && !isSlope)
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            rigid.velocity = Vector3.zero; // 위의 AddForce 의 추가 값을 없애줌 (통통 튀는 현상)
        }
    }
}
