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

    [SerializeField] private Image[] fillImages;

    [Header("Fill Settings")]
    [SerializeField] private float fillSmoothness = 0.2f;

    private float currentVelFill;
    private Vector3 initialRightPos;
    [SerializeField] private float targetFill = 1f;
    private bool useBattery;

    private void Start()
    {
        useBattery = false;
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
        if(fillImage)
        {
            float smoothFill = Mathf.SmoothDamp(fillImage.fillAmount, targetFill, ref currentVelFill, fillSmoothness);
            fillImage.fillAmount = smoothFill;

            if (!useBattery)
            {
                if (fillImage.fillAmount > 0.8f)
                {
                    fillImage.color = new Color(0.33f, 1, MapValues(fillImage.fillAmount, 0.8f, 1, 0.33f, 0.66f), 1);
                }
                else if (fillImage.fillAmount > 0.6f)
                {
                    fillImage.color = new Color(MapValues(fillImage.fillAmount, 0.6f, 0.8f, 1, 0.33f), 1, 0.33f, 1);
                }
                else if (fillImage.fillAmount > 0.2f)
                {
                    fillImage.color = new Color(1, MapValues(fillImage.fillAmount, 0.2f, 0.6f, 0.33f, 1), 0.33f, 1);
                }
                else
                {
                    fillImage.color = new Color(1, MapValues(fillImage.fillAmount, 0, 0.2f, 0, 0.33f), MapValues(fillImage.fillAmount, 0, 0.2f, 0, 0.33f), 1);
                }

                for (int i = 0; i < fillImages.Length; i++)
                {
                    fillImages[i].fillAmount = smoothFill;
                }
            }

            if (VariableManager.variableManager.IsDamaging())
            {
                if (useBattery)
                {
                    fillImage.color = Color.white;
                    useBattery = false;
                }
                else
                {
                    useBattery = true;
                }
            }
            else
            {
                useBattery = false;
            }

            if (rightImage && leftImage)
                rightImage.transform.position = Vector3.Lerp(leftImage.transform.position, initialRightPos, smoothFill);
        }
    }

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
