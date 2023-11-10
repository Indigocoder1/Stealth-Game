using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositioner : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;

    private void LateUpdate()
    {
        transform.position = targetPosition.position;
        transform.rotation = targetPosition.rotation;
    }
}
