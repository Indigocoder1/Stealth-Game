using UnityEngine;

public class RecoilManager : MonoBehaviour
{
    public float snappiness;

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private void Update()
    {
        Vector3 oldRotation = currentRotation;
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        Vector3 rotationDelta = currentRotation - oldRotation;
        transform.rotation *= Quaternion.Euler(rotationDelta);
    }

    public void AddRecoil(float recoilX, float recoilY)
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), 0);
    }
}
