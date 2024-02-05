using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Controller
{
    InputTest2 _mainInput;
    InputAction _moveAction;
    InputAction _runAction;
    InputAction _jumpAction;


    public float WalkSpeed;
    public float RunSpeed;
    Rigidbody _rigid;
    public Vector3 _moveDirection;

    public bool isRun;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        _mainInput = new InputTest2();
        _moveAction = _mainInput.Test.Move;
        _runAction = _mainInput.Test.Run;
        _jumpAction = _mainInput.Test.Jump;

    }
    void OnEnable()
    {
        _mainInput.Enable();
        _moveAction.Enable();
        _runAction.Enable();
        _jumpAction.Enable();
    }

    void Start()
    {
        OnInputWalk();
        OnInputRun();
    }

    void OnDisable()
    {
        _mainInput.Disable();
        _moveAction.Disable();
        _runAction.Disable();
        _jumpAction.Disable();
    }

    public override void Init()
    {
        Debug.Log("init");
        State = Define.State.Idle;
    }

    protected override void UpdateIdle()
    {
        // TODO : 가만히 있을때의 모션 있으면 사용
    }

    protected override void UpdateWalk()
    {
        _rigid.velocity = WalkSpeed * Move();
        Debug.Log("Function Walk");
        //bool hasControl = (_moveDirection != Vector3.zero);
        //if (hasControl)
        //{
        //    Look();
        //    _rigid.velocity = WalkSpeed * _moveDirection;
        //    //transform.Translate(_moveDirection * Time.fixedDeltaTime);
        //}
        //_rigid.velocity = WalkSpeed * Move();
    }

    protected override void UpdateRun()
    {
        _rigid.velocity = RunSpeed * Move();
        //bool hasControl = (_moveDirection != Vector3.zero);
        //if (hasControl)
        //{
        //    Look();
        //    _rigid.velocity = RunSpeed * _moveDirection;
        //    //transform.Translate(_moveDirection * Time.fixedDeltaTime);
        //}
        //_rigid.velocity = RunSpeed * Move();
    }

    Vector3 Move()
    {
        bool hasControl = (_moveDirection != Vector3.zero);
        if (hasControl)
        {
            Look();
            return _moveDirection;
            //transform.Translate(_moveDirection * Time.fixedDeltaTime);
        }

        return Vector3.zero;
    }

    void Look()
    {
        transform.rotation = Quaternion.LookRotation(_moveDirection);
    }

    void OnInputWalk()
    {
        _moveAction.performed += context =>
        {
            Debug.Log("Walk");
            Vector2 inputValue = _moveAction.ReadValue<Vector2>();
            if (inputValue != null)
            {
                if (!isRun)
                {
                    State = Define.State.Walk;
                }
                _moveDirection = new Vector3(inputValue.x, 0f, inputValue.y);
            }
        };

        _moveAction.canceled += context =>
        {
            _moveDirection = Vector3.zero;
            State = Define.State.Idle;
        };
    }

    void OnInputRun()
    {
        _runAction.performed += context =>
        {
            Debug.Log("performed");
            if (context.performed && _moveDirection != Vector3.zero)
            {
                Debug.Log("Run");
                isRun = true;
                State = Define.State.Run;
            }
        };

        _runAction.canceled += context =>
        {
            isRun = false;
            Debug.Log("canceled");
            if (_moveDirection == Vector3.zero)
            {
                Debug.Log("Run -> Idle");
                State = Define.State.Idle;
            } else
            {
                Debug.Log("Run -> Walk");
                State = Define.State.Walk;
            }
        };
    }
}
