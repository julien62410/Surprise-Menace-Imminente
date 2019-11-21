using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMaterialAnimator : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image[] images;
    [SerializeField] private float value = 1f;
    [SerializeField] private string propertyName = "_Emissive";
    private Material instance;
    public bool updateInEditor = false;

    private void OnDrawGizmosSelected()
    {
        if(updateInEditor)UpdateVisuals();
    }

    private void Update()
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        UpdateImage(image);
        for (int i = 0; i < images.Length; i++)
        {
            UpdateImage(images[i]);
        }
    }

    private void UpdateImage(Image image)
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
