using UnityEngine;

public class RecoilManager : MonoBehaviour
{
    public float snappiness;

    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private Vector3 lastRotationDelta;

    private void Update()
    {
        Vector3 oldRotation = currentRotation;
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        lastRotationDelta = currentRotation - oldRotation;
        transform.rotation *= Quaternion.Euler(lastRotationDelta);
    }

    public void AddRecoil(float recoilX, float recoilY)
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), 0);
    }

    public Vector3 GetRotationDelta()
    {
        return lastRotationDelta;
    }
}
