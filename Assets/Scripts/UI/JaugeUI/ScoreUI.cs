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
    private VariableManager v;

    private void Start()
    {
        v = VariableManager.variableManager;
    }

    private void Update()
    {
        if (scoreText && v) scoreText.text = v.score.ToString();
    }
}
