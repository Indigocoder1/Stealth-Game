using System;
using UnityEngine;

public class RecoilManager : MonoBehaviour
{
    public event EventHandler<RecoilManagerEventArgs> OnRotationDeltaCalculated;
    public float snappiness;

    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private bool isRotateAllowed;

    private void Update()
    {
        Vector3 oldRotation = currentRotation;
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);

        Vector3 rotationDelta = currentRotation - oldRotation;
        OnRotationDeltaCalculated?.Invoke(this, new RecoilManagerEventArgs(rotationDelta));

        if(isRotateAllowed)
        {
            transform.rotation *= Quaternion.Euler(rotationDelta);
        }
    }

    public void AddRecoil(float recoilX, float recoilY)
    {
        targetRotation += new Vector3(recoilX, UnityEngine.Random.Range(-recoilY, recoilY), 0);
    }

    public void SetRotateAllowed(bool isRotateAllowed)
    {
        this.isRotateAllowed = isRotateAllowed;
    }
}

public class RecoilManagerEventArgs : EventArgs
{
    public Vector3 rotationDelta;

    public RecoilManagerEventArgs(Vector3 rotationDelta)
    {
        this.rotationDelta = rotationDelta;
    }
}
