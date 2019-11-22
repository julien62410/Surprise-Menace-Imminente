using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string GameSceneName;

    public void StartGame()
    {
        StartCoroutine(StartingGame());
    }

    private IEnumerator StartingGame()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(GameSceneName);
    }
}
