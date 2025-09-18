using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] [Range(0f, 1f)] private float backgroundVolume = 0.5f;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        ConfigureAudioSource();

        if (backgroundMusic != null)
        {
            PlayBackgroundMusic(backgroundMusic);
        }
    }

    private void ConfigureAudioSource()
    {
        audioSource.playOnAwake = false;
        audioSource.priority = 0;
        audioSource.spatialBlend = 0f; // 2D sound
        audioSource.volume = backgroundVolume;
    }

    public void PlayBackgroundMusic(AudioClip clip, bool loop = true)
    {
        if (audioSource == null || clip == null) return;

        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.volume = backgroundVolume;
        audioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        if (audioSource == null) return;

        audioSource.Stop();
    }

    public void SetBackgroundVolume(float volume)
    {
        backgroundVolume = Mathf.Clamp01(volume);
        if (audioSource != null)
            audioSource.volume = backgroundVolume;
    }
}