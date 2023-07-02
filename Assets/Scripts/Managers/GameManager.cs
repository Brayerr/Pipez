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

    private void Start()
    {
        currentScene = 0;
    }

    private void OnEnable()
    {
        BoardManager.OnPathComplete += LevelWon;        
    }

    private void OnDestroy()
    {
        BoardManager.OnPathComplete -= LevelWon;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) OnClickedEsc.Invoke();
    }

    void LevelWon()
    {
        Debug.Log("level won sequence");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        OnChangedScene.Invoke();
        currentScene = 0;
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(1);
        //OnChangedScene.Invoke();
        currentScene = 1;
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene(2);
        OnChangedScene.Invoke();
        currentScene = 2;
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene(3);
        OnChangedScene.Invoke();
        currentScene = 3;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(currentScene + 1);
        OnChangedScene.Invoke();
        currentScene++;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
