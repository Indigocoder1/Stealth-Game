using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class ZeroGMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed;
    public float maxSpeed;
    public float drag;
    public float ascendDescendSpeed;

    [Header("Camera Movement")]
    public float xSensitivity;
    public float ySensitivity;
    public float cameraSmoothSpeed;
    public float tiltSpeed;
    public Transform cameraHolder;

    [Header("Model Alignment")]
    public Transform model;
    public float alignmentSpeed;

    private PlayerInputActions playerActions;
    private Rigidbody rb;
    private float xRotation;
    private float yRotation;
    private float tilt;
    private Vector3 targetLerpPos;

    private void Awake()
    {
        playerActions = new PlayerInputActions();

        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
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

        Look(playerActions.Player.CameraMovement.ReadValue<Vector2>());
        Move(playerActions.Player.Movement.ReadValue<Vector2>());
        HandleAscendDescend();
        LimitSpeed();
        AlignModel();
    }

    private void Move(Vector2 direction)
    {
        Vector3 moveDirection = cameraHolder.forward * direction.y + cameraHolder.right * direction.x;

        rb.AddForce(moveDirection.normalized * moveSpeed * Time.deltaTime * 10f, ForceMode.Force);
    }

    private void Look(Vector2 delta)
    {
        //Mouse rotation
        float mouseX = delta.x * Time.deltaTime * xSensitivity;
        float mouseY = delta.y * Time.deltaTime * ySensitivity;

        xRotation -= mouseY;
        yRotation += mouseX;

        //Tilt
        float tiltAmount = playerActions.Player.Tilt.ReadValue<float>() * Time.deltaTime * tiltSpeed;
        tilt -= tiltAmount;

        cameraHolder.rotation *= Quaternion.Euler(-mouseY, mouseX, -tiltAmount);
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
        Quaternion targetRotation = Quaternion.Euler(targetLerpPos);

        model.rotation = Quaternion.Slerp(model.rotation, targetRotation, alignmentSpeed * Time.deltaTime);
    }

    private void HandleAscendDescend()
    {
        float ascendDescendAmount = playerActions.Player.AscendDescend.ReadValue<float>();
        rb.AddForce(cameraHolder.up * ascendDescendAmount * ascendDescendSpeed * 200f * Time.deltaTime, ForceMode.Force);
    }

    private void OnEnable()
    {
        playerActions.Enable();
        if(rb)
        {
            rb.drag = drag;
        }
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
}