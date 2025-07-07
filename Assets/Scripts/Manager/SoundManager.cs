using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameMusic;

    private float _musicVolume = 0.5f;
    private float _sfxVolume = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetMusicVolume(_musicVolume);
            SetSFXVolume(_sfxVolume);
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
            PlayMusic(_menuMusic);
        }
        else if (scene.buildIndex == 1)
        {
            PlayMusic(_gameMusic);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            _sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            _musicSource.clip = clip;
            _musicSource.Play();
        }
    }

    public void StopMusic()
    {
        _musicSource.Pause();
    }

    public void SetMusicVolume(float volume)
    {
   
        _musicVolume = Mathf.Clamp01(volume);
        _audioMixer.SetFloat("MusicVolume", SliderToDb(_musicVolume));
    }

    public void SetSFXVolume(float volume)
    {
    
        _sfxVolume = Mathf.Clamp01(volume);
        _audioMixer.SetFloat("SFXVolume", SliderToDb(_sfxVolume));
    }

    public float MusicVolume => _musicVolume;

    public float SFXVolume => _sfxVolume;

    private float SliderToDb(float volume)
    {
        return Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
    }
}
