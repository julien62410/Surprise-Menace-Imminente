using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBehaviour : MonoBehaviour
{

    public int hp;
    public float damageDelay;

    private Collider col;
    private float timer;
    private int hpSave;
    private bool isInit = false;
    private ImminentDanger danger;


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
        col = this.GetComponent<EnnemyControl>().ennemy.GetComponent<Collider>();
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

        if (danger.battery > 0 && timer >= damageDelay && center.x > 0 && center.y > 0 && center.z > 0 && center.x < 1 && center.y < 1)
        {
            hp--;
            Debug.Log(hp);
            timer = 0f;
        }

        if (hp <= 0)
        {
            TriggerDeath();
        }
    }

    private void TriggerDeath()
    {
        EnnemySpawn.Instance.CollisionWithPlayer(this.gameObject);
    }

}
