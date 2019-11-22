using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuScoreUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI best;
    [SerializeField] private TextMeshProUGUI last;

    

    private void Start()
    {
        InitializeScores();
    }

    private void InitializeScores()
    {
        int highScore = 0;
        int lastScore = 0;
        if (PlayerPrefs.HasKey("highScores"))
        {
            highScore = PlayerPrefs.GetInt("highScores");
        }


        if(PlayerPrefs.HasKey("lastScore"))
        {
            lastScore = PlayerPrefs.GetInt("lastScore");
        }

        best.text = highScore.ToString();
        last.text = lastScore.ToString();
    }
}
