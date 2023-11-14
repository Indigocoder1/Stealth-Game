using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float drag;

    [Header("Camera Movement")]
    [SerializeField] private float xSensitivity;
    [SerializeField] private float ySensitivity;
    [SerializeField] private Transform cameraHolder;

    [Header("Model Alignment")]
    [SerializeField] private Transform model;
    [SerializeField] private float alignmentSpeed;

    private PlayerInputActions playerActions;
    private Rigidbody rb;
    private float xRotation;
    private float yRotation;
    private Vector3 targetLerpPos;
    private bool movementLocked = false;

    private void Awake()
    {
        playerActions = new PlayerInputActions();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();

        rb.drag = drag;
    }

    private void Update()
    {
        if (!movementLocked)
        {
            Look(playerActions.Player.CameraMovement.ReadValue<Vector2>());
            Move(playerActions.Player.Movement.ReadValue<Vector2>());
            LimitSpeed();
            AlignModel();
        }
    }

    private void Move(Vector2 direction)
    {
        Vector3 moveDirection = cameraHolder.forward * direction.y + cameraHolder.right * direction.x;

        rb.AddForce(moveDirection.normalized * moveSpeed * Time.deltaTime * 10f, ForceMode.Force);
    }

    private void Look(Vector2 delta)
    {
        float mouseX = delta.x * Time.deltaTime * xSensitivity;
        float mouseY = delta.y * Time.deltaTime * ySensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        targetLerpPos = cameraHolder.rotation.eulerAngles + rb.velocity.normalized;
    }

    private void LimitSpeed()
    {
        Vector3 velocity = rb.velocity;
        if(velocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = velocity.normalized * maxSpeed;
            rb.velocity = limitedVelocity;
        }
    }

    private void AlignModel()
    {
        //Adding "rb.velocity.magnitude * " to the speed multiplication will make it faster when the player is going fast and slow when the player is going slow
        //This doesn't quite feel right but feel free to experiment
        model.rotation = Quaternion.Slerp(model.rotation, Quaternion.Euler(targetLerpPos), alignmentSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    public void setMovementLockState(bool lockState)
    {
        movementLocked = lockState;
    }
}
