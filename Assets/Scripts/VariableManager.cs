using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableManager : MonoBehaviour
{
    [Header("Glitch")]
    public float radiusGlitchAndSound;
    public GlitchEffect scriptGlitchEffect;
    public Texture2D ennemyMaterial;
    public Shader glitchShader;

    [Header("Audio")]
    public float delayBetweenSoundFantome;

    [Header("Enemy Object")]
    public GameObject enemyPrefab;
    public GameObject enemyPool;

    [Header("Enemy Stats")]
    public int lifeEnemy;
    public float enemySpawnDistance;
    public float enemySpeed;

    [Header("Enemy Damage")]
    public float secondPerOneDamage;
    public float distBetweenEnemyAndPlayerToDamagePlayer;

    [Header("Camera")]
    public Camera arCamera;

    public static VariableManager variableManager = null;
    
    private void Awake()
    {
        if (variableManager == null)
            variableManager = this;
        else if (variableManager != this)
            Destroy(gameObject);
    }

}
