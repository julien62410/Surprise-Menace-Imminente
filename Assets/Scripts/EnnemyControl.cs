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
    }

    private void DistanceWithPlayer()
    {
        float _distance = Vector3.Distance(ennemyObject.transform.position, VariableManager.variableManager.arCamera.gameObject.transform.position);

        if (_distance < VariableManager.variableManager.distBetweenEnemyAndPlayerToDamagePlayer)
        {
            EnnemySpawn.Instance.CollisionWithPlayer(this.gameObject);
            DesactivateGameObject();
            VariableManager.variableManager.lifePlayer--;

            if (VariableManager.variableManager.lifePlayer == 0)
            {
                if (!PlayerPrefs.HasKey("highScores") || VariableManager.variableManager.score > PlayerPrefs.GetInt("highScores"))
                    PlayerPrefs.SetInt("highScores", VariableManager.variableManager.score);

                SceneManager.LoadScene("Menu");
            }
        }
    }

    private void DesactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}


