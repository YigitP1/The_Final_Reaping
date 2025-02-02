using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public Slider volumeSlider;
    public Dropdown resolutionDropdown;


    private bool isPaused = false;

    void Start()
    {
        // Hide menus initially
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);

        // Populate resolution dropdown


        // Load saved volume
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = volumeSlider.value;

        // Add listeners for settings changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;  // Resume game
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;  // Pause game
        isPaused = true;
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;  // Resume time before switching scenes
        SceneManager.LoadScene("MainMenu");  // Replace with your Main Menu scene name
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }


}
