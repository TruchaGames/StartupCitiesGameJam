using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject creditScreen;
    public GameObject mainMenuScreen;
    public GameObject optionsScreen;
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pauseMenu.activeSelf == false)
            {
                PauseGame();
            }
            else if (pauseMenu.activeSelf == true)
            {
                ResumeGame();
            }
        }
    }

    // Goes to Level 1
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    //Goes to Level 1
    public void StartLevel1()
    {
        SceneManager.LoadScene(1);
    }

    public void StartLevel2()
    {
        SceneManager.LoadScene(2);
    }

    public void StartLevel3()
    {
        SceneManager.LoadScene(3);
    }

    public void OpenCredits()
    {
        if (creditScreen.activeSelf == false)
        {
            if (creditScreen != null) creditScreen.SetActive(true);
            if (mainMenuScreen != null) mainMenuScreen.SetActive(false);
            if (optionsScreen != null) optionsScreen.SetActive(false);
            if (pauseMenu != null) pauseMenu.SetActive(false);

        }
    }

    public void CloseCredits()
    {
        if (creditScreen.activeSelf == true)
        {

            if (mainMenuScreen != null) mainMenuScreen.SetActive(true);
            if (creditScreen != null) creditScreen.SetActive(false);
            if (optionsScreen != null) optionsScreen.SetActive(false);
            if (pauseMenu != null) pauseMenu.SetActive(true);



        }

    }

    public void OpenOptions()
    {
        if (optionsScreen.activeSelf == false)
        {
            if (mainMenuScreen != null) mainMenuScreen.SetActive(false);
            if (creditScreen != null) creditScreen.SetActive(false);
            if (optionsScreen != null) optionsScreen.SetActive(true);
            if (pauseMenu != null) pauseMenu.SetActive(false);

        }
    }

    public void CloseOptions()
    {
        if (optionsScreen.activeSelf == true)
        {
            if (mainMenuScreen != null) mainMenuScreen.SetActive(false);
            if (creditScreen != null) creditScreen.SetActive(false);
            if (optionsScreen != null) optionsScreen.SetActive(false);
            if (pauseMenu != null) pauseMenu.SetActive(true);
        }

    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        if (mainMenuScreen != null) mainMenuScreen.SetActive(false);
        if (creditScreen != null) creditScreen.SetActive(false);
        if (optionsScreen != null) optionsScreen.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
