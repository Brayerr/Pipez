using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action OnClickedEsc;
    public static event Action OnChangedScene;
    int currentScene;
    public static bool gamePaused;

    private void Start()
    {
        currentScene = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.PlayClickSound.Invoke();
            OnClickedEsc.Invoke();
        }
    }

    private void OnEnable()
    {
        UIManager.GamePaused += GamePaused;
        UIManager.GameUnpaused += GameUnpaused;
    }

    private void OnDestroy()
    {
        UIManager.GamePaused -= GamePaused;
        UIManager.GameUnpaused -= GameUnpaused;
    }

    public void LoadMainMenu()
    {
        UIManager.PlayClickSound.Invoke();
        SceneManager.LoadScene(0);
        OnChangedScene.Invoke();
        currentScene = 0;
    }

    public void LoadLevel1()
    {
        UIManager.PlayClickSound.Invoke();
        SceneManager.LoadScene(1);
        //OnChangedScene.Invoke();
        currentScene = 1;
    }

    public void LoadLevel2()
    {
        UIManager.PlayClickSound.Invoke();
        SceneManager.LoadScene(2);
        OnChangedScene.Invoke();
        currentScene = 2;
    }

    public void LoadLevel3()
    {
        UIManager.PlayClickSound.Invoke();
        SceneManager.LoadScene(3);
        OnChangedScene.Invoke();
        currentScene = 3;
    }

    public void LoadNextLevel()
    {
        UIManager.PlayClickSound.Invoke();
        SceneManager.LoadScene(currentScene + 1);
        OnChangedScene.Invoke();
        currentScene++;
    }

    public void QuitGame()
    {
        UIManager.PlayClickSound.Invoke();
        Application.Quit();
    }

    void GamePaused()
    {
        gamePaused = true;
    }

    void GameUnpaused()
    {
        gamePaused = false;
    }
}
