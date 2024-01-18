using UnityEngine;

public class KickbackManager : MonoBehaviour
{
    public float snappiness;
    public float returnSpeed;
    private float originalSnappiness;
    private float originalReturnSpeed;

    private Vector3 targetPos;
    private Vector3 currentPos;

    private void Start()
    {
        currentPos = transform.position;
        originalSnappiness = snappiness;
        originalReturnSpeed = returnSpeed;
    }

    private void Update()
    {
        targetPos = Vector3.Lerp(targetPos, Vector3.zero, returnSpeed * Time.deltaTime);
        currentPos = Vector3.Lerp(currentPos, targetPos, snappiness * Time.deltaTime);
        transform.localPosition = currentPos;
    }

    public void AddKickback(Vector3 kick, float snappiness = 0, float returnSpeed = 0)
    {
        targetPos += kick;

        this.snappiness = returnSpeed != 0 ? snappiness : originalSnappiness;
        this.returnSpeed = returnSpeed != 0 ? returnSpeed : originalReturnSpeed;
    }
}
