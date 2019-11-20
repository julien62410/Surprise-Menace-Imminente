using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pataya.QuikFeedback;

/// <summary>
/// Manages the camera zooms.
/// </summary>
public class Zoomer : MonoBehaviour
{
    private bool isZooming = false;
    private float initialZoom = 0f;
    private float targetZoom = 0f;
    private float progress = 0f;
    private Camera cam;
    private ZoomFeedback z;

    public bool IsZooming { get => isZooming; set => isZooming = value; }

    private void Update()
    {
        ZoomUpdate();
    }

    public void Zoom(ZoomFeedback zoom, Camera _cam)
    {
        z = zoom;
        cam = _cam;
        initialZoom = IsOrtho(cam) ? cam.orthographicSize : cam.fieldOfView;
        targetZoom = z.multiplier * initialZoom;
        progress = 0f;
        isZooming = true;
    }

    private void ZoomUpdate()
    {
        if (isZooming)
        {
            if (progress <= 1f)
            {
                progress += Time.deltaTime * z.speed;
                float zoom = Mathf.LerpUnclamped(initialZoom, targetZoom, z.curve.Evaluate(progress));
                cam.orthographicSize = zoom;
                cam.fieldOfView = zoom;
            }

            else
            {
                EndZoom();
            }
        }
    }

    public void EndZoom()
    {
        isZooming = false;
        cam.orthographicSize = initialZoom;
        cam.fieldOfView = initialZoom;
    }

    private bool IsOrtho(Camera cam)
    {
        return cam.orthographic;
    }
}
