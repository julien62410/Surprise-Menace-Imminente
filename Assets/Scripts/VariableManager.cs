﻿using System.Collections;
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

    [Header("Reticle")]
    public Image reticle;
    public Color damageColor;
    public Color normalColor;
    public Color noBatteryColor;
    public Animator outOfBattery;

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
    public AudioSource[] damageSounds;
    public AudioSource startSound;
    public AudioSource gameOverSound;
    public AudioSource[] lowBattery;

    [Header("Bonus")]
    public GameObject heart;
    public GameObject multiplicateur;
    public int durationMultiplicateur;
    public float distanceSpawnBonus;

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

    private bool damaging, trueDamaging, batterySound;

    private void Awake()
    {
        Time.timeScale = 1f;
        gameOver = false;
        batterySound = false;
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
        if (battery > 0)
        {
            foreach (AudioSource a in lowBattery)
            {
                a.Stop();
            }
            batterySound = false;
        }
        else if(!batterySound)
        {
            foreach (AudioSource a in lowBattery)
            {
                a.Play();
            }
            batterySound = true;
        }
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

        outOfBattery.SetBool("OutOfBattery", !(battery > 0));

        if (battery > 0)
        {
            if (trueDamaging)
            {
                reticle.color = damageColor;
            }
            else
            {
                reticle.color = normalColor;
            }
        }

        else
        {
            reticle.color = noBatteryColor;
        }
    }

    public void DamagePlayer(int direction)
    {
        lifePlayer--;
        LifeUI.Instance.Lose();
        lifeBar.SetFill(maxLifePlayer, lifePlayer);
        foreach (AudioSource a in damageSounds)
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

        startGame = false;
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
