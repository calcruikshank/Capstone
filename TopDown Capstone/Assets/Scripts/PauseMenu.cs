using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public static bool GameHasStarted = false;
    public GameObject startGameUI;

    // Update is called once per frame
    void Awake()
    {
        GameIsPaused = false;
        //GameHasStarted = false;
    }

    void Start()
    {
        GameStart();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.D))
        {
            Resume();
            GameHasStarted = true;
            //GameIsPaused = false;
        }


    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        startGameUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        startGameUI.SetActive(false);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void GameStart()
    {
        startGameUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        GameHasStarted = false;
    }

}
