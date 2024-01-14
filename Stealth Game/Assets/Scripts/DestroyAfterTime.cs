using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float delay;

    private void Start()
    {
        Destroy(gameObject, delay);
    }
}
