using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBehaviour : MonoBehaviour
{

    public int hp;
    public float damageDelay;

    private Collider col2d;
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
        col2d = this.GetComponent<EnnemyControl>().ennemy.GetComponent<Collider>();
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
