using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeMenuShow : MonoBehaviour
{
    [SerializeField] GameObject volumeMenu;
    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioSource audioSource;
    void Start()
    {
        volumeMenu.SetActive(false);
        Time.timeScale = 1f;
        
        if (audioSource != null && volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;
        }
        
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            bool isMenuActive = !volumeMenu.activeSelf;
            volumeMenu.SetActive(isMenuActive);
            
            Cursor.visible = isMenuActive;
            Cursor.lockState = isMenuActive ? CursorLockMode.None : CursorLockMode.Locked;
            Time.timeScale = !isMenuActive ? 1f : 0f;
        }
    }
    
    void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}
