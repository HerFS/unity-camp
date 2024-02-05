using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [SerializeField]
    private Define.State _state = Define.State.Idle;
    protected Animator animator;

    public virtual Define.State State
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;

            switch (_state)
            {
                case Define.State.Idle:
                    animator.CrossFade("Idle", 0.1f);
                    break;
                case Define.State.Walk:
                    animator.CrossFade("Walking", 0.1f);
                    break;
                case Define.State.Run:
                    animator.CrossFade("Running", 0.1f);
                    break;
            }
        }
    }

    void Start()
    {
        Init();
        Debug.Log("Hello");
    }

    void Update()
    {
        switch (State)
        {
            case Define.State.Idle:
                UpdateIdle();
                //animator.CrossFade("Idle", 0.1f);
                break;
            case Define.State.Walk:
                Debug.Log("Controle Walk");
                UpdateWalk();
                break;
            case Define.State.Run:
                UpdateRun();
                break;
        }
        Debug.Log($"Stat {State}");
    }

    public abstract void Init();

    protected virtual void UpdateIdle() { }
    protected virtual void UpdateWalk() { }
    protected virtual void UpdateRun() { }
}
