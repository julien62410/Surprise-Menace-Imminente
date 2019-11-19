using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawn : MonoBehaviour
{
    public GameObject ennemy;
    public Camera arCamera;

    public float spawnDistance;
    public float speed;
    private bool isEnnemySpawned = false;

    private void Start()
    {
        RespawnEnnemy();
    }

    private void Update()
    {
        MoveEnnemy();
        DetectColisionWithPlayer();
    }

    private void RespawnEnnemy()
    {
        int _rotation = Random.Range(0, 361);
        ennemy.transform.position = new Vector3(spawnDistance, 0, 0);
        ennemy.transform.LookAt(arCamera.transform);
        transform.eulerAngles = new Vector3(0, _rotation, 0);
        ennemy.SetActive(true); 
        isEnnemySpawned = true;
    }

    private void MoveEnnemy()
    {
        if (isEnnemySpawned)
        {
            float _step = speed * Time.deltaTime;
            ennemy.transform.position = Vector3.MoveTowards(ennemy.transform.position, arCamera.transform.position, _step);
        }
    }

    private void DetectColisionWithPlayer()
    {
        if (isEnnemySpawned)
        {
           float _distance = Vector3.Distance(ennemy.transform.position, arCamera.transform.position); 

            if (_distance < 1f)
            {
                ennemy.SetActive(false);
                RespawnEnnemy();
            }
        }
    }

}
