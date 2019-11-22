using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBehaviour : MonoBehaviour
{
    public AudioSource sound, explosionSound;

    private float hp;

    [HideInInspector]
    public bool dead;

    private Collider col;

    [SerializeField] private EnemyVisuals visuals;
    private float hpSave;
    private bool isInit = false;

    void Start()
    {
        if (!isInit)
        {
            hp = VariableManager.variableManager.lifeEnemy;
            hpSave = hp;
            isInit = true;
        }
        dead = false;
        col = GetComponent<EnnemyControl>().ennemyObject.GetComponent<BoxCollider>();
        if (sound)
        {
            sound.pitch += Random.Range(-0.1f, 0.1f);
        }
    }

    private void OnEnable()
    {
        hp = hpSave;
        StartCoroutine(PlaySound());
    }

    void Update()
    {
        Vector3 center = Camera.main.WorldToViewportPoint(col.bounds.center);
        if (!dead)
        {
            if (VariableManager.variableManager.battery > 0 && center.x > 0.2f && center.y > 0.15f && center.z > 0 && center.x < 0.8f && center.y < 0.85f)
            {
                hp = Mathf.Max(0, hp - Time.deltaTime); ;
                if (visuals) visuals.DamageAnim();
                VariableManager.variableManager.Damaging();
            }

            if (hp <= 0)
            {
                dead = true;
                TriggerDeath();
            }
        }

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if(visuals)
        {
            visuals.SetExplosionProgress(hpSave, hp);
        }
    }

    private void TriggerDeath()
    {
        ScoreUI.Instance.Earn();
        VariableManager.variableManager.score += (VariableManager.variableManager.pointsPerEnemyDead * VariableManager.variableManager.multiplyScore);
        if (sound)
        {
            StopAllCoroutines();
            sound.Stop();
        }
        if (explosionSound)
        {
            explosionSound.Play();
        }
        if (visuals) visuals.DeathFeedback();
    }

    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(0.5f);
        if (sound) sound.Play();
    }
}
