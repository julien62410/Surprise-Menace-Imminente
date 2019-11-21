using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string GameSceneName;

    public void StartGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }
}
