using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMaterialAnimator : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float value = 1f;
    [SerializeField] private string propertyName = "_Emissive";
    private Material instance;

    private void OnDrawGizmos()
    {
        UpdateVisuals();
    }

    private void Update()
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (image)
        {
            if (image.material)
            {
                if (instance == null) instance = new Material(image.material);
                instance.SetFloat(propertyName, value);
                image.material = instance;
            }
        }
    }
}
