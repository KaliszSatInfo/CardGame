using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public AudioMixer audioMixer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float sliderValue)
    {
        float minDb = -60f;
        float maxDb = -20f;

        float volumeDb = Mathf.Lerp(minDb, maxDb, sliderValue);

        audioMixer.SetFloat("MusicVolume", volumeDb);

        Debug.Log($"Set volume: {sliderValue} -> {volumeDb} dB");
    }


    public void PlayMusic()
    {
        var source = GetComponent<AudioSource>();
        if (!source.isPlaying)
            source.Play();
    }

    public void StopMusic()
    {
        GetComponent<AudioSource>().Stop();
    }
}