using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _parentObject;
    [SerializeField] private List<WaveData> _waves;
    [SerializeField] private GameObject[] _enemyPrefabs; 
    [SerializeField] private float _intervalPause, _wavePause;
    private Dictionary<string, Queue<GameObject>> _enemyPools = new Dictionary<string, Queue<GameObject>>();

    public static WaveManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (var prefab in _enemyPrefabs)
        {
            string key = prefab.name;
            _enemyPools[key] = new Queue<GameObject>();

            for (int i = 0; i < 10; i++)
            {
                GameObject enemy = Instantiate(prefab, _spawnPoint.position, Quaternion.identity, _parentObject);
                enemy.name = key; 
                enemy.SetActive(false);
                _enemyPools[key].Enqueue(enemy);
            }
        }

        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        for (int waveIndex = 0; waveIndex < _waves.Count; waveIndex++)
        {
            UIManager.Instance.UpdateWaveCount(waveIndex + 1);
            WaveData wave = _waves[waveIndex];

            List<string> enemiesToSpawn = new List<string>();
            foreach (var entry in wave.enemies)
            {
                for (int i = 0; i < entry.amount; i++)
                {
                    enemiesToSpawn.Add(entry.enemyType);
                }
            }

            ShuffleList(enemiesToSpawn);

            
            foreach (var enemyType in enemiesToSpawn)
            {
                GameObject enemy = GetPooledEnemy(enemyType);
                if (enemy == null) continue;

                enemy.transform.position = _spawnPoint.position + new Vector3(0, 0, Random.Range(-1f, 1f));
                enemy.GetComponent<Enemy>()?.SetPath(_wayPoints);
                enemy.SetActive(true);

                yield return new WaitForSeconds(_intervalPause);
            }

            yield return new WaitForSeconds(_wavePause); 
        }

        yield return new WaitUntil(AllEnemiesDefeated);

        UIManager.Instance.EndGame(true);
        GameManager.Instance.SetTimeScale(0);
    }

    private bool AllEnemiesDefeated()
    {
        foreach (var pool in _enemyPools.Values)
        {
            foreach (var enemy in pool)
            {
                if (enemy.activeSelf)
                    return false;
            }
        }
        return true;
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }


    private GameObject GetPooledEnemy(string type)
    {
        if (_enemyPools.TryGetValue(type, out var pool))
        {
            foreach (var enemy in pool)
            {
                if (!enemy.activeSelf)
                    return enemy;
            }


            foreach (var prefab in _enemyPrefabs)
            {
                if (prefab.name == type)
                {
                    GameObject newEnemy = Instantiate(prefab, _spawnPoint.position, Quaternion.identity, _parentObject);
                    newEnemy.name = type;
                    newEnemy.SetActive(false);
                    pool.Enqueue(newEnemy);
                    return newEnemy;
                }
            }
        }

        Debug.LogWarning($"No prefab found for enemy type: {type}");
        return null;
    }

    [SerializeField] private List<Transform> _wayPoints; 
}
