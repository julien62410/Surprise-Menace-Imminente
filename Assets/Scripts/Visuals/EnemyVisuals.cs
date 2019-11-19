using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    [Header("Explosion")]

    [Range(0f, 1f)] [SerializeField] private float explosionProgress = 0f;
    [SerializeField] private GameObject[] toScale;
    private List<Vector3> initialScales = new List<Vector3>();
    [SerializeField] private float scaleMul = 1.2f;

    [SerializeField] private ObjectShaker[] toShake;
    [SerializeField] private float shake = 0.05f;

    [SerializeField] private float maxGonfle = 0.2f;
    [SerializeField] private Renderer rend;

    private void Update()
    {
        UpdateInitialScales();
        UpdateExplosion();
    }

    private void OnDrawGizmosSelected()
    {
        UpdateInitialScales();
        UpdateExplosion();
    }

    private void UpdateInitialScales()
    {
        if(toScale.Length != initialScales.Count)
        {
            initialScales.Clear();
            for (int i = 0; i < toScale.Length; i++)
            {
                initialScales.Add(toScale[i].transform.localScale);
            }
        }
    }

    private void UpdateExplosion()
    {
        for (int i = 0; i < toScale.Length; i++)
        {
            toScale[i].transform.localScale = Vector3.Lerp(initialScales[i], initialScales[i] * scaleMul, explosionProgress);
        }

        for (int i = 0; i < toShake.Length; i++)
        {
            toShake[i].intensity = shake * explosionProgress;
        }
        Material mat = rend.sharedMaterial;
        mat.SetFloat("_Gonfle", maxGonfle * explosionProgress);
        rend.sharedMaterial = mat;
    }
}
