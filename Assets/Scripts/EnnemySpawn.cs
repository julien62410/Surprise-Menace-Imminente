using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawn : MonoBehaviour
{
    private static EnnemySpawn _instance;
    public static EnnemySpawn Instance { get { return _instance; } }

    public int ennemyCountPerSpawn = 1;

    private List<GameObject> ennemyPoolList = new List<GameObject>();
    private GameObject batteryObject;
    private float timer;

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
        timer = 0f;
        InitPoolList();
        SpawnEnnemy();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer>= 5 - 4 * VariableManager.variableManager.difficulty)
        {
            timer = 0f;
            SpawnEnnemy();
        }
        if(VariableManager.variableManager.battery <= 25 && !batteryObject.activeInHierarchy)
        {
            SpawnBattery();
        }
    }

    private void InitPoolList()
    {
        foreach (Transform tsf in VariableManager.variableManager.enemyPool.transform)
        {
            if (tsf.gameObject.layer == LayerMask.NameToLayer("Battery"))
            {
                batteryObject = tsf.gameObject;
            }
            else
            {
                ennemyPoolList.Add(tsf.gameObject);
            }
        }
    }

    private void SpawnEnnemy(int number = 1)
    {
        for (int i = 0; i < ennemyCountPerSpawn; i++)
        {
            GameObject _ennemy = GetFirstFreeEnnemyInPool();

            if (_ennemy != null)
            {
                EnnemyControl _ennemyControl = _ennemy.GetComponent<EnnemyControl>();
                int _rotation = Random.Range(0, 361);

                _ennemy.transform.position = VariableManager.variableManager.arCamera.transform.position;
                _ennemy.GetComponent<DamageBehaviour>().dead = false;
                _ennemyControl.ennemyObject.transform.position = new Vector3(_ennemy.transform.position.x + VariableManager.variableManager.enemySpawnDistance, 
                    _ennemy.transform.position.y, 
                    _ennemy.transform.position.z);
                _ennemy.transform.eulerAngles = new Vector3(0, _rotation, 0);
                _ennemy.SetActive(true);
                _ennemy.GetComponentInChildren<AudioSource>().Play();
            } else
            {
                Debug.LogWarning("Pool entièrement utilisé");
            }
        }
    }

    private void SpawnBattery()
    {
        int negX = 1;
        int negZ = 1;

        if (Random.Range(-1, 1) < 0)
        {
            negZ = -1;
        }
        if (Random.Range(-1, 1) < 0)
        {
            negZ = -1;
        }

        batteryObject.transform.position = new Vector3(negX * Random.Range(0.5f, 1.0f),
            batteryObject.transform.position.y,
            negZ * Random.Range(0.5f, 1.0f));
        batteryObject.SetActive(true);
        //batteryObject.GetComponentInChildren<AudioSource>().Play();
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
        if(ennemy.layer == LayerMask.NameToLayer("Battery"))
        {
            VariableManager.variableManager.battery = 100f;
            BatteryUI.Instance.Earn();
            ennemy.GetComponentInChildren<BatteryVisuals>().Pickup();

        }
        //SpawnEnnemy(ennemyCountPerSpawn);
    }
}
