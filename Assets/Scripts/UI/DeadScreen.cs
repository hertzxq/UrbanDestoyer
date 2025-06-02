using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadScreen : MonoBehaviour
{
    public GameObject deathScreen;  // Ссылка на панель

    void Start()
    {
        // Проверка на null
        if (deathScreen == null)
        {
            Debug.LogError("DeathScreen: Панель deathScreen не назначена в инспекторе!");
            return;
        }

        deathScreen.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
            Time.timeScale = 0f; // Пауза игры
            Debug.Log("Экран смерти показан");
        }
        else
        {
            Debug.LogError("DeathScreen: Панель deathScreen не найдена!");
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Возобновить игру
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Перезагружаем сцену: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}