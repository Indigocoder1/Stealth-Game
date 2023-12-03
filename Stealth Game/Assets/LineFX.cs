using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LineFX : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int quality;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve effectCurve;

    private Transform origin;
    private Transform target;
    private Vector3 currentRopePosition;

    private void Start()
    {
        lineRenderer.gameObject.SetActive(false);
    }

    public void DrawRope()
    {
        if(!target)
        {
            lineRenderer.gameObject.SetActive(false);
            lineRenderer.positionCount = 0;
            currentRopePosition = origin.position;
            return;
        }
        else if(!lineRenderer.gameObject.activeSelf)
        {
            lineRenderer.gameObject.SetActive(true);
        }

        lineRenderer.positionCount = quality + 1;

        Vector3 ropeUp = Quaternion.LookRotation((target.position - origin.position).normalized) * Vector3.up;

        currentRopePosition = Vector3.Lerp(currentRopePosition, target.position, Time.deltaTime * 12f);

        for (int i = 0; i < quality + 1; i++)
        {
            float delta = i / (float)quality;
            Vector3 offset = ropeUp * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * effectCurve.Evaluate(delta);
            lineRenderer.SetPosition(i, Vector3.Lerp(origin.position, currentRopePosition, delta) + offset);
        }
    }

    public void SetOrigin(Transform origin) => this.origin = origin;
    public void SetTarget(Transform target) => this.target = target;
}
