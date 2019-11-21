using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class VariableManager : MonoBehaviour
{
    [Header("Glitch")]
    public float radiusGlitch;
    public GlitchEffect scriptGlitchEffect;

    [Header("Enemy Object")]
    public GameObject enemyPrefab;
    public GameObject enemyPool;

    [Header("UI")]
    public ImageFill lifeBar;
    public TextMeshProUGUI scoreText;
    public DamageFilter damageFilter;

    [HideInInspector]
    public float difficulty;

    [Header("Enemy Stats")]
    public float lifeEnemy;
    public float enemySpawnDistance;
    public float enemySpeed;

    [Header("Enemy Damage")]
    public float distBetweenEnemyAndPlayerToDamagePlayer;

    [Header("Camera")]
    public Camera arCamera;

    [Header("Player")]
    public float batteryUsage;
    public float battery;
    public int score;
    public int lifePlayer;

    public static VariableManager variableManager = null;

    private int maxLifePlayer;
    private bool gameOver;
    private bool damaging, trueDamaging;
    
    private void Awake()
    {
        Time.timeScale = 1f;
        gameOver = false;
        Application.targetFrameRate = 30;
        maxLifePlayer = lifePlayer;
        if (variableManager == null)
        {
            variableManager = this;
            difficulty = 0;
        }
        else if (variableManager != this)
            Destroy(gameObject);
    }

    private void Update()
    {
        difficulty = Mathf.Min(1, (float)score / 100.0f);
        scoreText.SetText(score.ToString());
    }

    private void LateUpdate()
    {
        if (damaging)
        {
            trueDamaging = true;
            damaging = false;
        }
        else
        {
            trueDamaging = false;
        }
    }

    public void DamagePlayer(int direction)
    {
        lifePlayer--;
        lifeBar.SetFill(maxLifePlayer, lifePlayer);
        damageFilter.Trigger(direction);
        if (lifePlayer <= 0 && !gameOver)
        {
            gameOver = true;
            StartCoroutine(GameOver());
        }
    }

    private IEnumerator GameOver()
    {
        if (!PlayerPrefs.HasKey("highScores") || score > PlayerPrefs.GetInt("highScores"))
            PlayerPrefs.SetInt("highScores", score);

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(2);

        Time.timeScale = 1f;

        SceneManager.LoadScene("Menu");
    }

    public void Damaging()
    {
        damaging = true;
    }

    public bool IsDamaging()
    {
        return trueDamaging;
    }
}
