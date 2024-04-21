using Photon.Pun;
using UnityEngine;

public class GroundMovement : MonoBehaviourPunCallbacks
{
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
    }

    private void Update()
    {
        Look(playerActions.Player.CameraMovement.ReadValue<Vector2>());
    }

    private void Look(Vector2 delta)
    {
        float mouseX = delta.x * xSensitivity * Time.deltaTime;
        float mouseY = delta.y * ySensitivity * Time.deltaTime;

        Vector3 lastRecoilRotationDelta = recoilManager.GetRotationDelta();
        yRotation += mouseX + lastRecoilRotationDelta.y;
        xRotation -= mouseY - lastRecoilRotationDelta.x;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
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
    }

    public override void OnDisable()
    {
        playerActions.Disable();
    }
}
