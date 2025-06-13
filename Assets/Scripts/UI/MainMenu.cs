using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("DefaultCarScene1");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting..."); 
    }
}