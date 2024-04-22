using UnityEngine;
using Photon.Pun;
using static UnityEditor.Rendering.ShadowCascadeGUI;

public class GroundMovement : MonoBehaviourPunCallbacks
{
    [Header("Player Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float maxWalkSpeed;
    public float maxSprintSpeed;
    public float maxCrouchSpeed;
    public float groundDrag;
    public float airSpeedMultiplier;

    private float currentMoveSpeed;
    private float currentMaxSpeed;
    private bool isSprinting;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;

    private bool readyToJump;

    [Header("Crouching")]
    public GameObject playerModel;
    public CapsuleCollider playerCollider;
    public float crouchScaleSpeed;
    public float crouchYScale;

    private float startYScale;
    private float startCapsuleYScale;
    private float startCameraYPos;
    private bool isCrouching;

    [Header("Ground Detection")]
    public Transform groundDetectionPoint;
    public float raycastLength;
    public LayerMask whatIsGround;

    private bool isGrounded;

    [Header("Camera Movement")]
    public float xSensitivity;
    public float ySensitivity;
    public Transform cameraHolder;
    public RecoilManager recoilManager;

    private PlayerInputActions playerActions;
    private Rigidbody rb;

    private float xRotation;
    private float yRotation;

    private MovementState movementState;

    public enum MovementState
    {
        Walking,
        Sprinting,
        Crouching,
        Air
    }

    private void Awake()
    {
        playerActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        startYScale = playerModel.transform.localScale.y;
        startCapsuleYScale = playerCollider.height;
        startCameraYPos = cameraHolder.localPosition.y;

        recoilManager.OnRotationDeltaCalculated += RecoilManager_OnRotationDeltaCalculated;
        playerActions.Player.Sprint.performed += _ =>
        {
            isSprinting = true;
        };
        playerActions.Player.Crouch.performed += _ =>
        {
            isCrouching = !isCrouching;
        };
    }

    private void Update()
    {
        HandleState();

        CheckForGround();
        TryJump();
        LimitSpeed();
        HandleCrouch();

        Look(playerActions.Player.CameraMovement.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        Move(playerActions.Player.Movement.ReadValue<Vector2>());
    }

    private void Move(Vector2 direction)
    {
        Vector3 moveDir = cameraHolder.forward * direction.y + cameraHolder.right * direction.x;
        moveDir.y = 0;

        float speedMultiplier = !isGrounded ? airSpeedMultiplier : 1;
        rb.AddForce(moveDir.normalized * currentMoveSpeed * speedMultiplier, ForceMode.Force);
    }

    private void Look(Vector2 delta)
    {
        float mouseX = delta.x * xSensitivity * Time.deltaTime;
        float mouseY = delta.y * ySensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    private void CheckForGround()
    {
        isGrounded = Physics.Raycast(groundDetectionPoint.position, Vector3.down, raycastLength, whatIsGround);

        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if(flatVel.magnitude > currentMaxSpeed)
        {
            Vector3 cappedVector = flatVel.normalized * currentMaxSpeed;
            rb.velocity = new Vector3(cappedVector.x, rb.velocity.y, cappedVector.z);
        }
    }

    private void TryJump()
    {
        if (playerActions.Player.Jump.IsPressed() && readyToJump && isGrounded)
        {
            readyToJump = false;

            Jump();
            Invoke(nameof(ResetAllowedToJump), jumpCooldown);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetAllowedToJump()
    {
        readyToJump = true;
    }

    private void HandleState()
    {
        if(rb.velocity.sqrMagnitude < .1f)
        {
            isSprinting = false;
        }

        movementState = MovementState.Air;

        if (isGrounded && isSprinting)
        {
            movementState = MovementState.Sprinting;
            currentMoveSpeed = sprintSpeed;
            currentMaxSpeed = maxSprintSpeed;
        }
        else if(isGrounded && isCrouching)
        {
            movementState = MovementState.Crouching;
            currentMoveSpeed = crouchSpeed;
            currentMaxSpeed = maxCrouchSpeed;
        }
        else if(isGrounded)
        {
            movementState = MovementState.Walking;
            currentMoveSpeed = walkSpeed;
            currentMaxSpeed = maxWalkSpeed;
        }
    }

    private void HandleCrouch()
    {
        float targetScale = 1f;

        if(isCrouching)
        {
            targetScale = crouchYScale;
        }

        Vector3 targetModelScale = new Vector3(playerModel.transform.localScale.x, startYScale * targetScale, playerModel.transform.localScale.z);
        playerModel.transform.localScale = Vector3.MoveTowards(playerModel.transform.localScale, targetModelScale, crouchScaleSpeed * Time.deltaTime);

        Vector3 targetCamPos = new Vector3(cameraHolder.localPosition.x, startCameraYPos * targetScale, cameraHolder.localPosition.z);
        cameraHolder.localPosition = Vector3.MoveTowards(cameraHolder.localPosition, targetCamPos, crouchScaleSpeed * Time.deltaTime);

        if(playerCollider.gameObject != playerModel)
        {
            playerCollider.height = Mathf.MoveTowards(playerCollider.height, startCapsuleYScale * targetScale, crouchScaleSpeed * Time.deltaTime);
        }
    }

    private void RecoilManager_OnRotationDeltaCalculated(object sender, RecoilManagerEventArgs e)
    {
        yRotation -= e.rotationDelta.y;
        xRotation += e.rotationDelta.x;
    }

    /*
    private void HandleJump()
    {
        if (!grounded)
        {
            return;
        }

        if (timeSinceLastJump < minTimeSinceLastJump)
        {
            timeSinceLastJump += Time.deltaTime;
            return;
        }

        if (playerActions.Player.AscendDescend.ReadValue<float>() > 0)
        {
            rb.AddForce(-gravityDirection * jumpForce * 10f, ForceMode.Force);
        }
    }
    */

    public override void OnEnable()
    {
        playerActions.Enable();
        recoilManager?.SetRotateAllowed(false);
        Invoke(nameof(ResetAllowedToJump), jumpCooldown);
    }

    public override void OnDisable()
    {
        playerActions.Disable();
        recoilManager?.SetRotateAllowed(true);
    }
}
