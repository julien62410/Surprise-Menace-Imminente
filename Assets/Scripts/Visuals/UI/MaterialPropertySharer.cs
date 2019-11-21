using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropertyType
{
    Float,
    Vector,
    Color
}

[System.Serializable]
public struct MaterialProperty
{
    #region Fields

    [SerializeField] private string name;
    [SerializeField] private PropertyType propertyType;
    [SerializeField] private float floatValue;
    [SerializeField] private Vector4 vectorValue;
    [SerializeField] private Color colorValue;
    [SerializeField] private bool foldEvent;

    #endregion

    #region Properties

    public string Name { get => name; set => name = value; }
    public PropertyType PropertyTypeEnum { get => propertyType; set => propertyType = value; }
    public float FloatValue { get => floatValue; set => floatValue = value; }
    public Vector4 VectorValue { get => vectorValue; set => vectorValue = value; }
    public Color ColorValue { get => colorValue; set => colorValue = value; }
    public bool FoldEvent { get => foldEvent; set => foldEvent = value; }

    #endregion
}

[ExecuteInEditMode]
public class MaterialPropertySharer : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material[] mats;

    [SerializeField] private MaterialProperty[] properties;

    public MaterialProperty[] Properties { get => properties; set => properties = value; }

    public void Update()
    {
        if (mats.Length > 0 && Properties.Length > 0)
            ShareProperties();
    }

    private void ShareProperties()
    {
        for (int i = 0; i < mats.Length; i++)
        {
            for (int a = 0; a < Properties.Length; a++)
            {
                if (Properties[a].PropertyTypeEnum == PropertyType.Float)
                {
                    mats[i].SetFloat(Properties[a].Name, Properties[a].FloatValue);
                }

                else if (Properties[a].PropertyTypeEnum == PropertyType.Color)
                {
                    mats[i].SetColor(Properties[a].Name, Properties[a].ColorValue);
                }

                else
                {
                    mats[i].SetColor(Properties[a].Name, Properties[a].VectorValue);
                }
            }
        }
    }
}