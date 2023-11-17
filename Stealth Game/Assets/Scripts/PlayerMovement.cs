using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed;
    public float maxSpeed;
    public float airDrag;
    public float groundDrag;
    public float ascendDescendSpeed;
    public float minTimeSinceLastJump;
    public float jumpForce;

    [Header("Camera Movement")]
    public float xSensitivity;
    public float ySensitivity;
    public float tiltSpeed;
    public Transform cameraHolder;

    [Header("Model Alignment")]
    public Transform model;
    public float alignmentSpeed;

    [Header("Gravity")]
    public Vector3 gravityDirection;
    public float gravityForce;

    private PlayerInputActions playerActions;
    private Rigidbody rb;
    private float xRotation;
    private float yRotation;
    private float tilt;
    private float groundedDistanceCheck = 0.1f;
    private float timeSinceLastJump;
    private Vector3 targetLerpPos;
    private bool usingGravity;
    private bool grounded;

    private void Awake()
    {
        playerActions = new PlayerInputActions();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();

        rb.drag = airDrag;
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, gravityDirection.normalized, groundedDistanceCheck))
        {
            grounded = true;

        }
        else
        {
            grounded = false;

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Look(playerActions.Player.CameraMovement.ReadValue<Vector2>(), playerActions.Player.Tilt.ReadValue<float>());
        Move(playerActions.Player.Movement.ReadValue<Vector2>());
        HandleGravity();
        HandleDrag();
        HandleJump();
        HandleAscendDescend();
        LimitSpeed();
        AlignModel();
    }

    private void HandleGravity()
    {
        if (grounded)
        {
            return;
        }

        if (!InGravity())
        {
            usingGravity = false;
            return;
        }

        rb.AddForce(gravityDirection.normalized * gravityForce, ForceMode.Acceleration);
        usingGravity = true;
    }

    private void HandleDrag()
    {
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void Move(Vector2 direction)
    {
        Vector3 moveDirection = cameraHolder.forward * direction.y + cameraHolder.right * direction.x;
        if (usingGravity)
        {
            moveDirection.y = 0;
        }

        rb.AddForce(moveDirection.normalized * moveSpeed * Time.deltaTime * 10f, ForceMode.Force);
    }

    private void Look(Vector2 delta, float tiltAmount)
    {
        float mouseX = delta.x * Time.deltaTime * xSensitivity;
        float mouseY = delta.y * Time.deltaTime * ySensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;
        tilt = (tilt - tiltAmount * tiltSpeed) % 360;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        cameraHolder.Rotate(new Vector3(-mouseY, mouseX, -tiltAmount), Space.Self);
        targetLerpPos = cameraHolder.rotation.eulerAngles + rb.velocity.normalized;
    }

    private void LimitSpeed()
    {
        Vector3 velocity = rb.velocity;
        if (velocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = velocity.normalized * maxSpeed;
            rb.velocity = limitedVelocity;
        }
    }

    private void AlignModel()
    {
        Quaternion targetRotation = Quaternion.identity;
        if (usingGravity)
        {
            targetRotation = Quaternion.Euler(0, targetLerpPos.y, 0);
        }
        else
        {
            targetRotation = Quaternion.Euler(targetLerpPos);
        }

        model.rotation = Quaternion.Slerp(model.rotation, targetRotation, alignmentSpeed * Time.deltaTime);
    }

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

    private void HandleAscendDescend()
    {
        if (grounded || usingGravity)
        {
            return;
        }

        float ascendDescendAmount = playerActions.Player.AscendDescend.ReadValue<float>();
        rb.AddForce(cameraHolder.up * ascendDescendAmount * ascendDescendSpeed * 200f * Time.deltaTime, ForceMode.Force);
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    public bool IsGrounded()
    {
        return grounded;
    }

    public bool InGravity()
    {
        return gravityDirection.sqrMagnitude > 0.01f;
    }
}