using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class GroundMovement : MonoBehaviourPunCallbacks
{
    [Header("Player Movement")]
    public float moveSpeed;
    public float maxSpeed;
    public float groundDrag;
    public float airSpeedMultiplier;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;

    private bool readyToJump;

    [Header("Ground Detection")]
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

    private void Awake()
    {
        playerActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        recoilManager.OnRotationDeltaCalculated += RecoilManager_OnRotationDeltaCalculated;
    }

    private void Update()
    {
        CheckForGround();
        TryJump();
        LimitSpeed();

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
        rb.AddForce(moveDir.normalized * moveSpeed * speedMultiplier, ForceMode.Force);
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
        isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastLength, whatIsGround);

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

        if(flatVel.magnitude > maxSpeed)
        {
            Vector3 cappedVector = flatVel.normalized * maxSpeed;
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
