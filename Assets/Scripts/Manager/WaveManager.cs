using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _parentObject;
    [SerializeField] private List<WaveData> _waves;
    [SerializeField] private GameObject[] _enemyPrefabs; // Reihenfolge muss mit enemyType übereinstimmen!
    [SerializeField] private float _intervalPause, _wavePause;
    private Dictionary<string, Queue<GameObject>> _enemyPools = new Dictionary<string, Queue<GameObject>>();

    private void Start()
    {
        // Pool vorbereiten
        foreach (var prefab in _enemyPrefabs)
        {
            string key = prefab.name;
            _enemyPools[key] = new Queue<GameObject>();

            for (int i = 0; i < 10; i++)
            {
                GameObject enemy = Instantiate(prefab, _spawnPoint.position, Quaternion.identity, _parentObject);
                enemy.name = key; // wichtig für dictionary key!
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

            // Alle Gegner dieser Welle sammeln
            List<string> enemiesToSpawn = new List<string>();
            foreach (var entry in wave.enemies)
            {
                for (int i = 0; i < entry.amount; i++)
                {
                    enemiesToSpawn.Add(entry.enemyType);
                }
            }

            // Liste mischen
            ShuffleList(enemiesToSpawn);

            // Gegner spawnen
            foreach (var enemyType in enemiesToSpawn)
            {
                GameObject enemy = GetPooledEnemy(enemyType);
                if (enemy == null) continue;

                enemy.transform.position = _spawnPoint.position + new Vector3(0, 0, Random.Range(-1f, 1f));
                enemy.GetComponent<Enemy>()?.SetPath(_wayPoints);
                enemy.SetActive(true);

                yield return new WaitForSeconds(_intervalPause);
            }

            yield return new WaitForSeconds(_wavePause); // Pause nach jeder Welle
        }
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

            // Wenn alle aktiv sind, erweitern wir den Pool
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

    [SerializeField] private List<Transform> _wayPoints; // vergessen?
}
