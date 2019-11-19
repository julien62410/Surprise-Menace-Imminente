using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBehaviour : MonoBehaviour
{

    public int hp;
    public float damageDelay;

    private Collider col2d;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        col2d = GetComponent<Collider>();
    }

    // Update is called once per frame
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
        Debug.Log(name + " died");
    }

}
