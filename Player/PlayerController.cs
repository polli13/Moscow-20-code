using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] bool playerForward;
    [SerializeField] private GameObject activeModel;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float jogSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float rotateSpeed;

    private float moveSpeed;

    public bool InAction { get; set; }

    private Vector3 LookDirection = Vector3.forward;
    private Vector3 MoveDirection;
    private Vector3 CameraForward;

    [SerializeField] private Transform camTransform;
    [SerializeField] private float toGround = 0.5f;
    [SerializeField] private LayerMask groundMask;

    public States states;
    public InputsVariables inputs;

    [HideInInspector] public float delta;
    private Rigidbody rb;
    private Animator anim;
    private Collider capsuleCollider;

    [SerializeField]
    private bool stopControl = false;

    public static PlayerController m_PlayerController { get; set; }
    public void Init()
    {
        if (m_PlayerController == null)
            m_PlayerController = this;

        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        anim = activeModel.GetComponent<Animator>();
        rb.drag = 4;

        if (stopControl)
            ControlState(true);
    }

    public Animator CurrentAnimator() { return anim; }

    public void ControlState(bool _act)
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = _act;
        anim.speed = 1;
        stopControl = _act;
        inputs.vertical = 0;
        inputs.horizontal = 0;
    }

    public void FixedUpdateStates(float d)
    {
        delta = d;
        CameraForward = Vector3.Scale(camTransform.forward, new Vector3(1, 0, 1)).normalized;

        if(!playerForward)
            MoveDirection = inputs.vertical * CameraForward + inputs.horizontal * camTransform.right;
        else
            MoveDirection = inputs.vertical * transform.forward + inputs.horizontal * transform.right;

        MoveDirection.y = 0;
        if (MoveDirection.magnitude > 1f) 
            MoveDirection.Normalize();

        float amount = Mathf.Abs(inputs.vertical) + Mathf.Abs(inputs.horizontal);
        amount = Mathf.Clamp01(amount);
        inputs.moveAmount = amount;

        if (stopControl || states.isInAction)
            return;

        if (!states.onGround)
            return;

        HandleRotation();
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 Forward = Vector3.Scale(MoveDirection, new Vector3(1, 0, 1)).normalized;

        if (states.onGround)
        {
            rb.drag = inputs.moveAmount > 0 ? 0 : 4;
            Forward *= moveSpeed;

            rb.velocity = Forward;
        }
    }

    private void HandleRotation()
    {
        Vector3 targetDir = MoveDirection;
        Vector3 cross = transform.forward;

        if (states.isLockedOn)
        {
            LookDirection.Set(LookDirection.x, 0, LookDirection.z);
            targetDir = inputs.lockOnTransform != null ? inputs.lockOnTransform.position - transform.position : LookDirection;
        }

        targetDir.y = 0;
        if (targetDir == Vector3.zero)
            targetDir = transform.forward;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, delta * rotateSpeed);
        transform.rotation = targetRotation;
    }

    public void UpdateStates(float d)
    {
        delta = d;
        states.onGround = OnGround();
        states.isInAction = anim.GetBool(StaticStrings.InAction);

        InAction = states.isInAction;

        if (!states.onGround)
        {
            states.isRunning = false;
            states.isLockedOn = false;
            states.isWalking = false;
            rb.drag = 3;
        }

        MovementSpeed();
        HandleMovementAnim();
    }

    private void MovementSpeed()
    {
        if (states.isRunning)
        {
            moveSpeed = sprintSpeed;
            states.isLockedOn = false;
            states.isWalking = false;
        }
        else
        {
            moveSpeed = states.isWalking ? walkSpeed : jogSpeed;
        }
    }

    private void HandleMovementAnim()
    {
        anim.SetBool(StaticStrings.Run, states.isRunning);

        var velocity = rb.velocity.magnitude;
        if (stopControl || inputs.moveAmount <= 0) velocity = 0;

        anim.SetFloat(StaticStrings.Forward, velocity, 0.2f, delta);  //HASH Anim!!!!
    }

    public bool OnGround()
    {
        bool _ground = false;

        Vector3 origin = transform.position + (Vector3.up * 0.5f);
        Vector3 dir = -Vector3.up;
        float dis = toGround;

        RaycastHit hit;
        Debug.DrawRay(origin, dir * dis, Color.red);

        if (Physics.Raycast(origin, dir, out hit, dis, groundMask))
        {
            _ground = true;
            transform.position = hit.point;
        }

        return _ground;
    }

    public bool CheckVelocityRun()
    {
        return rb.velocity.magnitude > 0.005f && (inputs.vertical != 0 || inputs.horizontal != 0);
    }

    public bool CheckVelocityStep()
    {
        return rb.velocity.magnitude > 0.28 && (inputs.vertical != 0 || inputs.horizontal != 0) && !states.isAttacking && !states.isInAction;
    }

    public bool CheckRunButton() => states.isRunning;

    [System.Serializable]
    public class InputsVariables
    {
        public float vertical;
        public float horizontal;
        public float moveAmount;
        public Transform lockOnTransform;
    }

    [System.Serializable]
    public class States
    {
        public bool onGround;
        public bool isWalking;
        public bool isRunning;
        public bool isLockedOn;
        public bool isInAction;
        public bool isTwoHanded;
        public bool isAttacking;
    }
}
