using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyControl : MonoBehaviour
{
    public GameObject player;
    public GameObject ennemy;

    public float speed;
    public float distanceToHit = 1;

    private void Start()
    {
        player = EnnemySpawn.Instance.arCamera.gameObject;
        speed = EnnemySpawn.Instance.speed;
        distanceToHit = EnnemySpawn.Instance.distanceToHit;
    }

    private void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            MoveEnnemy();
            DistanceWithPlayer();
        }
    }

    private void MoveEnnemy()
    {
        float _step = speed * Time.deltaTime;
        ennemy.transform.position = Vector3.MoveTowards(ennemy.transform.position, player.transform.position, _step);
    }

    private void DistanceWithPlayer()
    {
        float _distance = Vector3.Distance(ennemy.transform.position, player.transform.position);

        if (_distance < distanceToHit)
        {
            EnnemySpawn.Instance.CollisionWithPlayer(this.gameObject);
        }
    }
}
