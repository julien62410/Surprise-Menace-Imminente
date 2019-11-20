using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawn : MonoBehaviour
{
    private static EnnemySpawn _instance;
    public static EnnemySpawn Instance { get { return _instance; } }

    public int ennemyCountPerSpawn = 1;

    private List<GameObject> ennemyPoolList = new List<GameObject>();

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
        InitPoolList();
        SpawnEnnemy();
    }

    private void InitPoolList()
    {
        foreach (Transform tsf in VariableManager.variableManager.enemyPool.transform)
        {
            ennemyPoolList.Add(tsf.gameObject);
        }
    }

    private void SpawnEnnemy(int number = 1)
    {
        for (int i = 0; i < ennemyCountPerSpawn; i++)
        {
            GameObject _ennemy = GetFirstFreeEnnemyInPool();

            if (_ennemy != null)
            {
                _ennemy.SetActive(true);
                EnnemyControl _ennemyControl = _ennemy.GetComponent<EnnemyControl>();
                int _rotation = Random.Range(0, 361);
                _ennemyControl.transform.position = new Vector3(VariableManager.variableManager.enemySpawnDistance, 0, 0);
                _ennemyControl.transform.LookAt(VariableManager.variableManager.arCamera.transform);
                _ennemy.transform.eulerAngles = new Vector3(0, _rotation, 0);
            } else
            {
                Debug.LogWarning("Pool entièrement utilisé");
            }
        }
    }

    private GameObject GetFirstFreeEnnemyInPool()
    {
        foreach (GameObject ennemy in ennemyPoolList)
        {
            if (!ennemy.activeInHierarchy)
            {
                return ennemy;
            }
        }
        return null;
    }

    public void CollisionWithPlayer(GameObject ennemy)
    {
        ennemy.SetActive(false);
        SpawnEnnemy(ennemyCountPerSpawn);
    }


}
