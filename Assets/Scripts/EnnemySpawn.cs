﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnnemySpawn : MonoBehaviour
{
    private static EnnemySpawn _instance;
    public static EnnemySpawn Instance { get { return _instance; } }
    public DetectSurfaces detectSurfaces;

    private List<GameObject> ennemyPoolList = new List<GameObject>();
    private GameObject batteryObject;
    private float timer;
    private float startTimer;

    private void OnEnable()
    {
        detectSurfaces.PlaneFoundInWorld += SpawnBonus;
    }

    private void OnDisable()
    {
        detectSurfaces.PlaneFoundInWorld -= SpawnBonus;
    }

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
        startTimer = 0f;
        timer = 0f;
        InitPoolList();
    }

    private void Update()
    {
        if (VariableManager.variableManager.startGame)

        {

            startTimer += Time.deltaTime;

            if (startTimer >= 2)

                if (!VariableManager.variableManager.gameOver)
                {
                    timer += Time.deltaTime;
                    if (timer >= 5 - 4 * VariableManager.variableManager.difficulty)
                    {
                        timer = 0f;
                        SpawnEnnemy();
                    }
                    if (VariableManager.variableManager.battery <= 25 && !batteryObject.activeInHierarchy)
                        SpawnBattery();
                }
                else
                {
                    DeactivateAll();
                }

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

    private void DeactivateAll()
    {
        foreach (Transform tsf in VariableManager.variableManager.enemyPool.transform)
        {
            if (tsf.gameObject.layer == LayerMask.NameToLayer("Battery"))
            {
                batteryObject.SetActive(false);
                batteryObject.GetComponent<BatteryAudio>().bzzt.Play();
            }
            else
            {
                tsf.gameObject.SetActive(false);
                tsf.gameObject.GetComponentInChildren<AudioSource>().Stop();

            }
        }
    }
    private void SpawnEnnemy()
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
            _ennemy.GetComponentInChildren<AudioSource>().pitch = Random.Range(1.75f, 2.25f);
            _ennemy.GetComponentInChildren<AudioSource>().Play();
        }
        else
        {
            Debug.LogWarning("Pool entièrement utilisé");
        }
    }

    private void SpawnBattery()
    {
        int negX = 1;
        int negZ = 1;

        if (Random.Range(-1, 1) < 0)
        {
            negX = -1;
        }
        if (Random.Range(-1, 1) < 0)
        {
            negZ = -1;
        }

        batteryObject.transform.position = new Vector3(negX * Random.Range(0.5f, 0.7f),
            batteryObject.transform.position.y,
            negZ * Random.Range(0.5f, 0.7f));
        batteryObject.SetActive(true);
        batteryObject.GetComponent<BatteryAudio>().bzzt.Play();
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

    public void CollisionWithPlayer(GameObject objects)
    {
        if (objects.layer == LayerMask.NameToLayer("Battery"))
        {
            VariableManager.variableManager.battery = 100f;
            BatteryUI.Instance.Earn();

            objects.GetComponentInChildren<EntityVisuals>().Pickup();
            objects.GetComponent<BatteryAudio>().grab.Play();
            objects.GetComponent<BatteryAudio>().bzzt.Stop();
        }

        else if (objects.layer == LayerMask.NameToLayer("Heart"))
        {
            VariableManager.variableManager.Heal();
        }

        objects.GetComponentInChildren<EntityVisuals>().Pickup();
    }

    private void SpawnBonus(GameObject parentSpawn)
    {
        GameObject bonus;

        if (Vector3.Distance(parentSpawn.transform.position, VariableManager.variableManager.arCamera.transform.position) > VariableManager.variableManager.distanceSpawnBonus)
            if (VariableManager.variableManager.heart != null)
                bonus = Instantiate(VariableManager.variableManager.heart,parentSpawn.transform);
    }

}
