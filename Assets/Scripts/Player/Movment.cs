using UnityEngine;
using UnityEngine.InputSystem;

public class Movment : MonoBehaviour
{
    //Player Settings
    [SerializeField] float characterSpeed = 3.5f;
    [SerializeField] float runningMultiply = 1.3f;
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float rotationSpeed = 10f;

    //Actions
    InputAction moveAction;
    InputAction jumpAction;
    InputAction Sprint;

    //private
    private Vector3 velocity;
    private float gravityValue = -9.81f;
    private float currentMultiply = 1f;

    public bool canMove = true;

    //Components
    private Rigidbody m_Rigidbody;
    private CharacterController m_CharacterController;
    private Animator m_Animator;
    private Transform cameraTransform;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        Sprint = InputSystem.actions.FindAction("Sprint");

        m_Rigidbody = GetComponent<Rigidbody>();
        m_CharacterController = GetComponent<CharacterController>();
        m_Animator = GetComponentInChildren<Animator>();

        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    void Update()
    {
        if (!canMove) { Gravity(); return; }
        MovmentFunc();
        Run();
        Gravity();
        Jump();
    }

    private void Gravity()
    {
        velocity.y += gravityValue * Time.deltaTime;
        m_CharacterController.Move(velocity * Time.deltaTime);

        m_Animator.SetFloat("VelocityVertical", m_CharacterController.velocity.y);
    }
    private void Jump() {
        if (jumpAction.IsPressed() && m_CharacterController.isGrounded == true)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        m_Animator.SetBool("IsGrounded", m_CharacterController.isGrounded);
    }

    private void Run()
    {
        if (Sprint.WasPerformedThisFrame())
        {
            currentMultiply = runningMultiply;
        }

        if (Sprint.WasReleasedThisFrame())
        {
            currentMultiply = 1f;
        }
    }

    private void MovmentFunc()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        
        m_Animator.SetBool("Moving", moveValue != Vector2.zero);
        m_Animator.SetBool("IsRunning", currentMultiply > 1);

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * moveValue.y + moveValue.x * right).normalized;
        m_CharacterController.Move(move * characterSpeed * currentMultiply * Time.deltaTime);

        if (move.magnitude > 0.1f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
