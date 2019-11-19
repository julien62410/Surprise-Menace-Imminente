using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawn : MonoBehaviour
{
    private static EnnemySpawn _instance;
    public static EnnemySpawn Instance { get { return _instance; } }

    public GameObject ennemy;
    public Camera arCamera;

    public float spawnDistance;
    public float speed;
    public int ennemyCountPerSpawn = 1;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        SpawnEnnemy();
    }

    private void SpawnEnnemy(int number = 1)
    {
        for (int i = 0; i < ennemyCountPerSpawn; i++)
        {
            int _rotation = Random.Range(0, 361);

            GameObject _ennemy = Instantiate(ennemy);

            EnnemyControl _ennemyControl = _ennemy.GetComponent<EnnemyControl>();

            _ennemyControl.ennemy.transform.position = new Vector3(spawnDistance, 0, 0);
            _ennemyControl.ennemy.transform.LookAt(arCamera.transform);
            _ennemy.transform.eulerAngles = new Vector3(0, _rotation, 0);
            _ennemy.transform.SetParent(this.transform);
        }


    }

    public void CollisionWithPlayer(GameObject collider)
    {
        Destroy(collider);
        SpawnEnnemy(ennemyCountPerSpawn);
    }


}
