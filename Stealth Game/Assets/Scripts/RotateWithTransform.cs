using UnityEngine;

public class RotateWithTransform : MonoBehaviour
{
    public Transform transformToCopy;
    public bool localRotation;
    public bool rotateX;
    public bool rotateY;
    public bool rotateZ;

    private void LateUpdate()
    {
        float x = rotateX ? transformToCopy.eulerAngles.x : 0;
        float y = rotateY ? transformToCopy.eulerAngles.y : 0;
        float z = rotateZ ? transformToCopy.eulerAngles.z : 0;

        if(localRotation)
        {
            transform.localRotation = Quaternion.Euler(x, y, z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(x, y, z);
        }
    }
}
