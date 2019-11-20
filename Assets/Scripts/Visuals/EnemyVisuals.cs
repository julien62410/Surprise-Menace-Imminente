using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Death")]

    [Header("Anticipation")]
    [Range(0f, 1f)] [SerializeField] private float explosionProgress = 0f;

    [Header("Scale")]
    [SerializeField] private GameObject[] toScale;
    private List<Vector3> initialScales = new List<Vector3>();
    [SerializeField] private float scaleMul = 1.2f;

    [Header("Shake")]
    [SerializeField] private ObjectShaker[] toShake;
    [SerializeField] private float shake = 0.05f;

    [Header("Gonfle")]
    [SerializeField] private float maxGonfle = 0.2f;
    [SerializeField] private Renderer rend;


    [Header("Climax")]
    [SerializeField] private ParticleSystem explosionPs;
    [SerializeField] private GameObject[] toDeactivate;

    public delegate void VisualEvent();
    public VisualEvent onDeathFeedbackPlayed;

    private void OnEnable()
    {
        for (int i = 0; i < toDeactivate.Length; i++)
        {
            toDeactivate[i].SetActive(true);
        }
    }

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

    public void DamageAnim()
    {
        animator.SetBool("damaged", true);
    }

    public void IdleAnim()
    {
        animator.SetBool("damaged", false);
    }

    public void SetExplosionProgress(int hpMax, int currentHp)
    {
        float normalized = Mathf.InverseLerp(0, hpMax, currentHp);
        explosionProgress = 1 - normalized;
    }

    private void UpdateInitialScales()
    {
        if (toScale.Length != initialScales.Count)
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
        if (rend)
        {
            Material mat = rend.sharedMaterial;
            mat.SetFloat("_Gonfle", maxGonfle * explosionProgress);
            rend.sharedMaterial = mat;
        }
    }

    public void DeathFeedback()
    {
        explosionPs.Play();
        for (int i = 0; i < toDeactivate.Length; i++)
        {
            toDeactivate[i].SetActive(false);
        }

        StartCoroutine(DeactivatingGameObject());
    }

    private IEnumerator DeactivatingGameObject()
    {
        yield return new WaitForSeconds(2f);
        onDeathFeedbackPlayed.Invoke();
    }
}
