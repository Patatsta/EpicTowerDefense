using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event Action<bool> OnPauseChanged;

    [SerializeField] private int _hP = 10;
    private float _currentTimeScale;
    private float _lastNonZeroTimeScale = 1f;

    public int _warfunds { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
 
    }

    private void Start()
    {
        _warfunds = 1000;
        _currentTimeScale = 1f;
        _lastNonZeroTimeScale = 1f;
        SetTimeScale(_currentTimeScale);

        UpdateUIManager();
        UIManager.Instance.UpdateHP(_hP);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale != 0)
            {
                UIManager.Instance.UIPauseScreen(true);
                SetTimeScale(0);
              
                OnPauseChanged?.Invoke(true);
            }
            else
            {
                UIManager.Instance.UIPauseScreen(false);
                SetTimeScale(_lastNonZeroTimeScale);
                OnPauseChanged?.Invoke(false);
            }
        }
    }

    public void AddWarFunds(int funds)
    {
        _warfunds += funds;
        UpdateUIManager();
    }

    public void PlaceTurret(int cost)
    {
        _warfunds -= cost;
        UpdateUIManager();
    }

    private void UpdateUIManager()
    {
        UIManager.Instance.UpdateWarfunds(_warfunds);
    }

    public void LoseHP()
    {
        _hP--;
        UIManager.Instance.UpdateHP(_hP);

        if (_hP <= 0)
        {
            UIManager.Instance.EndGame(false);
            WaveManager.Instance.StopAllCoroutines();
            SetTimeScale(0);
            OnPauseChanged?.Invoke(true);
        }
    }

    public void SetTimeScale(float time)
    {
        Time.timeScale = time;
        _currentTimeScale = time;

        if (time == 0)
        {
            OnPauseChanged?.Invoke(true);
        }
        else
        {
            OnPauseChanged?.Invoke(false);
            _lastNonZeroTimeScale = time;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadStartScreen()
    {
        SceneManager.LoadScene(0);
    }
}
