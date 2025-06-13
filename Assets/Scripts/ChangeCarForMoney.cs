using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeCarForMoney : MonoBehaviour
{
    public YandexGame sdk;
    public SceneField[] scenes;
    private int nextSceneIndex = -1;
    public GameObject CarSelectionCanvas;

    void Start()
    {
        Time.timeScale = 1f;
        Debug.Log("Start method called. SDK reference: " + (sdk != null ? "Assigned" : "Not Assigned"));
        if (sdk == null)
        {
            Debug.LogError("YandexGame component is not assigned! Please assign it in Inspector.");
        }
        else
        {
            // Подписываемся на событие завершения рекламы
            YandexGame.RewardVideoEvent += OnRewarded;
        }
        StartCoroutine(ShowCarSelectionCanvasPeriodically());
    }

    void OnDestroy()
    {
        // Отписываемся от события, чтобы избежать утечек памяти
        YandexGame.RewardVideoEvent -= OnRewarded;
    }

    private IEnumerator ShowCarSelectionCanvasPeriodically()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(120f);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (CarSelectionCanvas != null)
            {
                CarSelectionCanvas.SetActive(true);
                yield return new WaitForSecondsRealtime(120f);
                CarSelectionCanvas.SetActive(true);
                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void AdCar(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < scenes.Length)
        {
            nextSceneIndex = sceneIndex;
            if (sdk != null)
            {
                Debug.Log("Attempting to show rewarded ad for scene index: " + sceneIndex);
                sdk._RewardedShow(1); // Показываем рекламу
            }
            else
            {
                Debug.LogError("SDK is null, cannot show ad!");
            }
        }
        else
        {
            Debug.LogError("Invalid scene index: " + sceneIndex);
        }
    }

    private void OnRewarded(int id)
    {
        // Проверяем, что это нужная реклама (id == 1)
        if (id == 1)
        {
            LoadSceneAfterAd();
        }
    }

    public void LoadSceneAfterAd()
    {
        if (nextSceneIndex >= 0 && nextSceneIndex < scenes.Length)
        {
            SceneManager.LoadScene(scenes[nextSceneIndex].SceneName);
            nextSceneIndex = -1;
        }
    }

    public void Quit()
    {
        CarSelectionCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

[System.Serializable]
public class SceneField
{
    [SerializeField] private Object sceneAsset;
    [SerializeField] private string sceneName;

    public string SceneName
    {
        get { return sceneName; }
        set { sceneName = value; }
    }
}