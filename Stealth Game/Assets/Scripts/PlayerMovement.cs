using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float drag;

    [Header("Camera Movement")]
    [SerializeField] private float xSensitivity;
    [SerializeField] private float ySensitivity;
    [SerializeField] private Transform cameraHolder;

    private PlayerInputActions playerActions;
    private Rigidbody rb;
    private float xRotation;
    private float yRotation;

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
        Look(playerActions.Player.CameraMovement.ReadValue<Vector2>());
        Move(playerActions.Player.Movement.ReadValue<Vector2>());
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
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
}
