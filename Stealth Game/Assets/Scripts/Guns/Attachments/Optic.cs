using UnityEngine;

public class Optic : Attachment
{
    public Transform cameraAimPos;
    public float zoomFactor;
    public float zoomSpeed;
    public float adsSpeed;

    public override void Enable()
    {
        gameObject.SetActive(true);
    }

    public override void Disable()
    {
        gameObject.SetActive(false);
    }
}
