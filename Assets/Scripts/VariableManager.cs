using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    public Image reticle;

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


    [Header("Audio")]
    public AudioSource[] endSounds;
    public AudioSource startSound;
    public AudioSource gameOverSound;

    [Header("Bonus")]
    public GameObject heart;
    public GameObject multiplicateur;
    public int durationMultiplicateur;
    public int distanceSpawnBonus;

    public static VariableManager variableManager = null;

    [HideInInspector]
    public float difficulty;
    [HideInInspector]
    public int multiplyScore;
    [HideInInspector]
    public int maxLifePlayer;
    [HideInInspector]
    public Coroutine waitForResetMultiplicateur = null;
    [HideInInspector]
    public int score;
    [HideInInspector]
    public bool gameOver;
    [HideInInspector]
    public bool startGame;

    private bool damaging, trueDamaging;

    private void Awake()
    {
        Time.timeScale = 1f;
        gameOver = false;
        score = 0;
        multiplyScore = 1;
        startGame = false;
        Application.targetFrameRate = 30;
        maxLifePlayer = lifePlayer;

        if (variableManager == null)
        {
            variableManager = this;
            difficulty = 0;
        }
        else if (variableManager != this)
            Destroy(gameObject);
        startSound.Play();
    }

    private void Update()
    {
        difficulty = Mathf.Min(1, (float)score / 10000.0f);
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
        if (trueDamaging)
        {
            reticle.color = Color.red;
        }
        else
        {
            reticle.color = Color.white;
        }
    }

    public void DamagePlayer(int direction)
    {
        lifePlayer--;
        LifeUI.Instance.Lose();
        lifeBar.SetFill(maxLifePlayer, lifePlayer);
        foreach (AudioSource a in endSounds)
        {
            a.Play();
        }
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

        PlayerPrefs.SetInt("lastScore", score);

        gameOverSound.Play();

        yield return new WaitForSecondsRealtime(5f);

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
