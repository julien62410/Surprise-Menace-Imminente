using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyControl : MonoBehaviour
    private EnemyVisuals visuals;
    private void Start()
    {
        visuals = GetComponentInChildren<EnemyVisuals>();
        if (visuals) visuals.onDeathFeedbackPlayed += DeactivateGameObject;
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
        transform.position = Vector3.MoveTowards(transform.position, VariableManager.variableManager.arCamera.gameObject.transform.position, _step);
    }

    private void DistanceWithPlayer()
    {
        float _distance = Vector3.Distance(transform.position, VariableManager.variableManager.arCamera.gameObject.transform.position);

        if (_distance < VariableManager.variableManager.distBetweenEnemyAndPlayerToDamagePlayer)
        {
            EnnemySpawn.Instance.CollisionWithPlayer(this.gameObject);
        }
    }

    private void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}


