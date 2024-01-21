using System.Collections.Generic;
using UnityEngine;

public class AttachmentManager : MonoBehaviour
{
    [Header("Optics")]
    public new Camera camera;

    private float originalFOV;
    private Vector3 originalPos;
    private Optic activeOptic;

    [Header("Attachments")]
    public List<Optic> optics;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new();

        SwapToOptic(0);
        originalFOV = camera.fieldOfView;
        originalPos = transform.localPosition;
    }

    private void Update()
    {
        HandleAimingFOV();
        HandleAimingPos();
    }

    private void HandleAimingFOV()
    {
        float targetFOV = 0;
        if (inputActions.Player.ADS.IsPressed())
        {
            targetFOV = Mathf.Lerp(camera.fieldOfView, originalFOV / activeOptic.zoomFactor, activeOptic.zoomSpeed * Time.deltaTime);
        }
        else
        {
            targetFOV = Mathf.Lerp(camera.fieldOfView, originalFOV, activeOptic.zoomSpeed * Time.deltaTime);
        }

        camera.fieldOfView = targetFOV;
    }
    
    private void HandleAimingPos()
    {
        Vector3 offset = transform.parent.InverseTransformVector(camera.transform.position - activeOptic.cameraAimPos.position);

        Vector3 targetPos = inputActions.Player.ADS.IsPressed() ? transform.localPosition + offset : originalPos;

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, activeOptic.adsSpeed * Time.deltaTime);
    }

    #region ---Optics---
    public void SwapToOptic(Optic oldOptic, Optic newOptic)
    {
        oldOptic.gameObject.SetActive(false);
        newOptic.gameObject.SetActive(true);
    }

    public void SwapToOptic(Optic optic)
    {
        if (!optics.Contains(optic))
        {
            Debug.LogError($"Attachment \"{optic.name}\" is not on this gun ({gameObject.name})");
            return;
        }

        SwapToOptic(optics.IndexOf(optic));
    }

    public void SwapToOptic(int index)
    {
        for (int i = 0; i < optics.Count; i++)
        {
            if(i != index)
            {
                optics[i].Disable();
                continue;
            }

            optics[i].Enable();
            activeOptic = optics[i];
        }
    }
    #endregion

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
