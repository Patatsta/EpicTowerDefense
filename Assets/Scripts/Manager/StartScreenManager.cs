using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    [SerializeField] private GameObject[] _zombiePrefabs;
    [SerializeField] private Transform[] _zombiePath;
    [SerializeField] private float _minSpawnInterval = 1f;
    [SerializeField] private float _maxSpawnInterval = 5f;
    [SerializeField] private Transform _spawnPoint;

    private void Start()
    {
        Time.timeScale = 1.0f;

        if (SoundManager.Instance != null)
        {
            _musicSlider.value = SoundManager.Instance.MusicVolume;
            _sfxSlider.value = SoundManager.Instance.SFXVolume;
        }

        _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        _sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        StartCoroutine(SpawnZombiesRoutine());
    }

    private void OnDestroy()
    {
        _musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        _sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
    }

    private void OnMusicVolumeChanged(float value)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetMusicVolume(value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetSFXVolume(value);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator SpawnZombiesRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(_minSpawnInterval, _maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            GameObject zombiePrefab = _zombiePrefabs[Random.Range(0, _zombiePrefabs.Length)];
            GameObject newZombie = Instantiate(zombiePrefab, _spawnPoint.position, Quaternion.identity);

            Enemy enemy = newZombie.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetPath(new List<Transform>(_zombiePath));
            }
        }
    }
}


