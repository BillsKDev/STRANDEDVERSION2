using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public static bool isPaused;

    void Start()
    {
        pauseMenu.SetActive(false);
    } 
    void Update ()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }
            else
            {
                PauseGame();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;


            }
        }
     
    } 
    public void PauseGame()
    {
        pauseMenu?.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
       

    } 
    public void ResumeGame()
    {
        pauseMenu?.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MAINMENU");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
