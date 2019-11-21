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
            if (VariableManager.variableManager.battery > 0 && center.x > 0 && center.y > 0 && center.z > 0 && center.x < 1 && center.y < 1)
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
        VariableManager.variableManager.score++;
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
        EnnemySpawn.Instance.CollisionWithPlayer(this.gameObject);
    }

    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(0.5f);
        if (sound) sound.Play();
    }
}
