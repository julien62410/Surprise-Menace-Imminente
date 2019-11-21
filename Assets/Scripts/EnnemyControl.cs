using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnnemyControl : MonoBehaviour
{
    public GameObject ennemyObject;
    private EnemyVisuals visuals;

    private void Start()
    {
        visuals = GetComponentInChildren<EnemyVisuals>();
        if (visuals) visuals.onDeathFeedbackPlayed += DesactivateGameObject;
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
        float _step = VariableManager.variableManager.enemySpeed * Time.deltaTime;
        ennemyObject.transform.position = Vector3.MoveTowards(ennemyObject.transform.position, VariableManager.variableManager.arCamera.gameObject.transform.position, _step);

        ennemyObject.transform.LookAt(VariableManager.variableManager.arCamera.transform);
    }

    private void DistanceWithPlayer()
    {
        Transform player = VariableManager.variableManager.arCamera.gameObject.transform;
        float _distance = Vector3.Distance(ennemyObject.transform.position, player.position);
        
        if (_distance < VariableManager.variableManager.distBetweenEnemyAndPlayerToDamagePlayer)
        {
            Vector3 relative = Quaternion.Euler(-player.rotation.eulerAngles.x, -player.rotation.eulerAngles.y, -player.rotation.eulerAngles.z) * (ennemyObject.transform.position - player.position);

            EnnemySpawn.Instance.CollisionWithPlayer(this.gameObject);

            if (relative.z >= 0 && relative.z >= Mathf.Abs(relative.x))
            {
                VariableManager.variableManager.DamagePlayer(0);
            }
            else if (relative.x <= 0 && -relative.x >= Mathf.Abs(relative.z))
            {
                VariableManager.variableManager.DamagePlayer(1);

            }
            else if (relative.z <= 0 && -relative.z >= Mathf.Abs(relative.x))
            {
                VariableManager.variableManager.DamagePlayer(2);

            }
            else if (relative.x >= 0 && relative.x >= Mathf.Abs(relative.z))
            {
                VariableManager.variableManager.DamagePlayer(3);

            }

            DesactivateGameObject();

        }
    }

    private void DesactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}


