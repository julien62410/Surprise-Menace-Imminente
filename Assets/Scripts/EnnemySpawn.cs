using System.Collections;
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
        timer = 0f;
        InitPoolList();
        SpawnEnnemy();
    }

    private void Update()
    {
        if (VariableManager.variableManager.startGame)
            if (!VariableManager.variableManager.gameOver)
            {
                timer += Time.deltaTime;
                if (timer >= 5 - 4 * VariableManager.variableManager.difficulty)
                {
                    timer = 0f;
                    SpawnEnnemy();
                }
                if (VariableManager.variableManager.battery <= 25 && !batteryObject.activeInHierarchy)
                {
                    SpawnBattery();
                }
            }
            else
            {
                DeactivateAll();
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
            _ennemy.GetComponentInChildren<AudioSource>().Play();
        } else
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

            objects.GetComponentInChildren<BatteryVisuals>().Pickup();
            objects.GetComponent<BatteryAudio>().grab.Play();
            objects.GetComponent<BatteryAudio>().bzzt.Stop();

            objects.GetComponentInChildren<BatteryVisuals>().Pickup();


        }
        else if (objects.layer == LayerMask.NameToLayer("Multi"))
        {
            VariableManager.variableManager.multiplyScore = 10;

            if (VariableManager.variableManager.waitForResetMultiplicateur != null)
                StopCoroutine(VariableManager.variableManager.waitForResetMultiplicateur);
            VariableManager.variableManager.waitForResetMultiplicateur = StartCoroutine("MultiplyScore");

            Destroy(objects);
        }
        else if (objects.layer == LayerMask.NameToLayer("Heart"))
        {
            VariableManager.variableManager.lifePlayer = VariableManager.variableManager.maxLifePlayer;
            Destroy(objects);
        }
    }

    private void SpawnBonus (GameObject parentSpawn)
    {
        if (Vector3.Distance(parentSpawn.transform.position, VariableManager.variableManager.arCamera.transform.position) > VariableManager.variableManager.distanceSpawnBonus)
        {
            GameObject bonus;
            int rand = Random.Range(0, 6);

            if (VariableManager.variableManager.heart != null && VariableManager.variableManager.multiplicateur != null)
            {
                if (rand == 0)
                    bonus = Instantiate(VariableManager.variableManager.heart);
                else if (rand == 1)
                    bonus = Instantiate(VariableManager.variableManager.multiplicateur);
                else
                    return;

                bonus.transform.SetParent(parentSpawn.transform);
                bonus.transform.position = parentSpawn.transform.position;
            }
        }
    }

    private IEnumerator MultiplyScore()
    {
        yield return new WaitForSeconds(VariableManager.variableManager.durationMultiplicateur);
        VariableManager.variableManager.multiplyScore = 1;
    }
}
