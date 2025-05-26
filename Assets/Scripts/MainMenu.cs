using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider volumeSlider;

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
        AudioListener.volume = savedVolume;

        if (MusicManager.instance != null)
        {
            MusicManager.instance.SetVolume(savedVolume);
            MusicManager.instance.PlayMusic();
        }

        Debug.Log("AudioListener.volume: " + AudioListener.volume);
    }
    public void SetVolume(float volume)
    {
        Debug.Log("SetVolume called with: " + volume);

        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);

        if (MusicManager.instance != null)
        {
            MusicManager.instance.SetVolume(volume);
        }
    }
}
