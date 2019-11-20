using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFill : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image leftImage;
    [SerializeField] private Image rightImage;
    [SerializeField] private Image fillImage;

    [Header("Fill Settings")]
    [SerializeField] private float fillSmoothness = 0.2f;

    private float currentVelFill;
    private Vector3 initialRightPos;
    [SerializeField] private float targetFill = 1f;

    private void Start()
    {
        initialRightPos = rightImage ? rightImage.transform.position : Vector3.right;
    }

    public void SetFill(float maxValue, float currentValue)
    {
        float newFill = Mathf.InverseLerp(0, maxValue, currentValue);
        targetFill = newFill;
    }

    private void Update()
    {
        UpdateFill();
    }

    private void UpdateFill()
    {
        if(rightImage && leftImage && fillImage)
        {
            float smoothFill = Mathf.SmoothDamp(fillImage.fillAmount, targetFill, ref currentVelFill, fillSmoothness);
            fillImage.fillAmount = smoothFill;

            rightImage.transform.position = Vector3.Lerp(leftImage.transform.position, initialRightPos, smoothFill);
        }
    }
}
