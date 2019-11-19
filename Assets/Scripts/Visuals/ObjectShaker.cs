using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShaker : MonoBehaviour
{
    [Header("Settings")]
    [Range(0f, 10f)] public float intensity = 0.0f;
    public bool xAxis = true;
    public bool yAxis = true;
    public bool zAxis = false;

    public bool autoSetInitialPos = true;
    private Vector3 initialPos;

    public GameObject go;

    private void Start()
    {
        if(autoSetInitialPos) SetInitialPos();
        if (!go) go = gameObject;
    }

    [ContextMenu("Set Initial Pos")]
    private void SetInitialPos()
    {
        initialPos = transform.localPosition;
    }

    private void Update()
    {
        Shake();
    }

    private void Shake()
    {
        float x = IntensityAxis(xAxis);
        float y = IntensityAxis(yAxis);
        float z = IntensityAxis(zAxis);
        Vector3 pos = new Vector3(x, y, z);
        go.transform.localPosition = initialPos + pos;
    }

    private float IntensityAxis(bool axis)
    {
        float output = axis ? Random.Range(-1f, 1f) * intensity : 0f;
        return output;
    }
}
