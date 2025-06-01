using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioMixer audioMixer;
    public AudioClip[] musicClips;
    private AudioSource audioSource;

    private int currentIndex = -1;
    private System.Random rng = new System.Random();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();

            ShuffleSongs();
            PlayNext();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNext();
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
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    private void PlayNext()
    {
        if (musicClips.Length == 0) return;

        currentIndex++;
        if (currentIndex >= musicClips.Length)
        {
            ShuffleSongs();
            currentIndex = 0;
        }

        audioSource.clip = musicClips[currentIndex];
        audioSource.Play();
    }

    private void ShuffleSongs()
    {
        for (int i = musicClips.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (musicClips[i], musicClips[j]) = (musicClips[j], musicClips[i]);
        }
    }
}