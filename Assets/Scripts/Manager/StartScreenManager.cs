using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private GameObject[] _zombiePrefabs;
    [SerializeField] private Transform[] _zombiePath;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 5f;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        Time.timeScale = 1.0f;

        if (SoundManager.Instance != null)
        {
            musicSlider.value = SoundManager.Instance.MusicVolume;
            sfxSlider.value = SoundManager.Instance.SFXVolume;
        }

        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        StartCoroutine(SpawnZombiesRoutine());
    }

    private void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
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
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            GameObject zombiePrefab = _zombiePrefabs[Random.Range(0, _zombiePrefabs.Length)];
            GameObject newZombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);

            Enemy enemy = newZombie.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetPath(new List<Transform>(_zombiePath));
            }
        }
    }
}


