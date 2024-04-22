using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Ground Check")]
    public float groundDistanceCheck;

    [Header("Gravity")]
    public Rigidbody rb;
    public Vector3 gravityDirection;
    public float gravityForce;

    [Header("Multiplayer")]
    [SerializeField] private GameObject player;

    private ZeroGMovement zeroGMovement;
    private GroundMovement groundMovement;

    private bool isGrounded;

    private void Start()
    {
        zeroGMovement = GetComponent<ZeroGMovement>();
        groundMovement = GetComponent<GroundMovement>();
    }

    private void Update()
    {
        HandleMovements();
    }

    private void FixedUpdate()
    {
        HandleGravity();
    }

    private void HandleMovements()
    {
        if(InGravity())
        {
            groundMovement.enabled = true;
            zeroGMovement.enabled = false;
        }
        else
        {
            groundMovement.enabled = false;
            zeroGMovement.enabled = true;
        }
    }

    private void HandleGravity()
    {
        if(InGravity())
        {
            rb.AddForce(gravityDirection * gravityForce, ForceMode.Acceleration);
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool InGravity()
    {
        return gravityDirection.sqrMagnitude > 0.01f;
    }

    
}
