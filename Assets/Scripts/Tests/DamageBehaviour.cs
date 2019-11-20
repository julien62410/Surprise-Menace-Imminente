using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBehaviour : MonoBehaviour
{

    public int hp;
    public float damageDelay;

    [HideInInspector]
    public bool dead;

    private Collider col;

    [SerializeField] private EnemyVisuals visuals;
    private float timer;
    private int hpSave;
    private bool isInit = false;
    private ImminentDanger danger;

    void Start()
    {
        if (!isInit)
        {
            hp = VariableManager.variableManager.lifeEnemy;
            hpSave = hp;
            isInit = true;
        }
        dead = false;
        timer = 0f;
        col = GetComponent<Collider>();
        danger = Camera.main.GetComponent<ImminentDanger>();
    }

    private void OnEnable()
    {
        hp = hpSave;
    }

    void Update()
    {
        timer += Time.deltaTime;

        Vector3 center = Camera.main.WorldToViewportPoint(col.bounds.center);

        if (danger.battery > 0 && timer >= VariableManager.variableManager.delayBetweenSoundFantome && center.x > 0 && center.y > 0 && center.z > 0 && center.x < 1 && center.y < 1)
        {
            hp--;
            timer = 0f;
            if (visuals) visuals.DamageAnim();
        }

        if (!dead && hp <= 0)
        {
            dead = true;
            TriggerDeath();
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
        if(visuals) visuals.DeathFeedback();
        EnnemySpawn.Instance.CollisionWithPlayer(this.gameObject);
    }

}
