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
    public int pointsPerEnemyDead;
    public int lifePlayer;

    [Header("Bonus")]
    public GameObject heart;
    public GameObject multiplicateur;
    public int durationMultiplicateur;

    public static VariableManager variableManager = null;

    [HideInInspector]
    public float difficulty;
    //[HideInInspector]
    public int multiplyScore = 1;
    [HideInInspector]
    public int maxLifePlayer;
    [HideInInspector]
    public Coroutine waitForResetMultiplicateur = null;
    [HideInInspector]
    public int score;

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
        LifeUI.Instance.Lose();
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
