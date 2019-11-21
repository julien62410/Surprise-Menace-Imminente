using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : JaugeUI
{
    private static ScoreUI instance;
    public static ScoreUI Instance
    {
        get
        {
            if (!instance) instance = FindObjectOfType<ScoreUI>();
            return instance;
        }
    }

    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (scoreText)
        {
            scoreText.SetText(VariableManager.variableManager.score.ToString());
        }
    }
}
