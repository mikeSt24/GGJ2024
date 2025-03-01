using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void ToMainGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void ToOptionsMenu()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ToWinScreen()
    {
        SceneManager.LoadScene("WinScreen");
    }

    public void ToLoseScreen()
    {
        SceneManager.LoadScene("LoseScreen");
    }

    public void ToCredits()
    {
        SceneManager.LoadScene("CreditsScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
