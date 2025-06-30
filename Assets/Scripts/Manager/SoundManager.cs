using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;

    private float musicVolume = 0.5f;
    private float sfxVolume = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetMusicVolume(musicVolume);
            SetSFXVolume(sfxVolume);
        }
        else
        {
            Destroy(gameObject);
        }
    }

  

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            PlayMusic(menuMusic);
        }
        else if (scene.buildIndex == 1)
        {
            PlayMusic(gameMusic);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Pause();
    }

    public void SetMusicVolume(float volume)
    {
   
        musicVolume = Mathf.Clamp01(volume);
        audioMixer.SetFloat("MusicVolume", SliderToDb(musicVolume));
    }

    public void SetSFXVolume(float volume)
    {
    
        sfxVolume = Mathf.Clamp01(volume);
        audioMixer.SetFloat("SFXVolume", SliderToDb(sfxVolume));
    }

    public float MusicVolume => musicVolume;

    public float SFXVolume => sfxVolume;

    private float SliderToDb(float volume)
    {
        return Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
    }
}
