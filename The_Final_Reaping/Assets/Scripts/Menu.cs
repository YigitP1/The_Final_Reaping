using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SoundEffectSlider;
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SetMusicValue()
    {
        float volume = MusicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }
    public void SetSoundEffectValue()
    {
        float volume = SoundEffectSlider.value;
        myMixer.SetFloat("SoundEffect", Mathf.Log10(volume) * 20);
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
