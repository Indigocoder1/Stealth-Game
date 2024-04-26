using System;
using UnityEngine;

public class StairHandler : MonoBehaviour
{
    public event EventHandler<StairHandlerEventArgs> OnStairTrigger;

    private Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnStairTrigger?.Invoke(this, new StairHandlerEventArgs(other.ClosestPointOnBounds(col.transform.position)));
    }

    private void OnTriggerStay(Collider other)
    {
        OnStairTrigger?.Invoke(this, new StairHandlerEventArgs(other.ClosestPointOnBounds(col.transform.position)));
    }
}

public class StairHandlerEventArgs : EventArgs
{
    public Vector3 closestPointFromHitCol;

    public StairHandlerEventArgs(Vector3 closestPointFromHitCol)
    {
        this.closestPointFromHitCol = closestPointFromHitCol;
    }
}
