using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _parentObject;
    [SerializeField] private List<GameObject> _enemies = new List<GameObject>();

    [SerializeField] private int _waveIndex = 0;

    private void Start()
    {
    
        for (int i = 0; i < 10; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefab, _spawnPoint.position, Quaternion.identity, _parentObject);
            enemy.SetActive(false);
            _enemies.Add(enemy);
        }
        UIManager.Instance.UpdateWaveCount(_waveIndex);
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        _waveIndex++;
        UIManager.Instance.UpdateWaveCount(_waveIndex);
        for (int i = 0; i < _waveIndex; i++)
        {
            GameObject enemy = GetPooledEnemy();

            if (enemy == null)
            {
                enemy = Instantiate(_enemyPrefab, _spawnPoint.position, Quaternion.identity, _parentObject);
                _enemies.Add(enemy);
            }

            enemy.transform.position = _spawnPoint.position;
            enemy.SetActive(true);

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(5f);

        if (_waveIndex < 10)
            StartCoroutine(SpawnWaves());
    }

    private GameObject GetPooledEnemy()
    {
        foreach (GameObject enemy in _enemies)
        {
            if (!enemy.activeSelf)
                return enemy;
        }

        return null; 
    }
}

