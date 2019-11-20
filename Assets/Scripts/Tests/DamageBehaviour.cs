using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBehaviour : MonoBehaviour
{

    public int hp;
    public float damageDelay;
    public Collider col2d;

    [SerializeField] private EnemyVisuals visuals;

    private float timer;
    private int hpSave;
    private bool isInit = false;


    private void Awake()
    {
        if (!isInit)
        {
            hpSave = hp;
            isInit = true;
        }

    }

    void Start()
    {
        timer = 0f;
    }

    private void OnEnable()
    {
        hp = hpSave;
    }

    void Update()
    {
        timer += Time.deltaTime;

        Vector3 center = Camera.main.WorldToViewportPoint(col2d.bounds.center);

        if (timer >= damageDelay && center.x > 0 && center.y > 0 && center.z > 0 && center.x < 1 && center.y < 1)
        {
            hp--;
            timer = 0f;
            if (visuals) visuals.DamageAnim();
        }

        if (hp <= 0)
        {
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
