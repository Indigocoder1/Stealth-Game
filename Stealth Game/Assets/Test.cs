using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        foreach (var item in terrain.materialTemplate.GetPropertyNames(MaterialPropertyType.Vector))
        {
            Debug.Log(item);
        }
    }
}
