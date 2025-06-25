using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private int _hP = 10;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate GameManager detected, destroying this one.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: bleibt beim Szenenwechsel erhalten
    }

    public int _warfunds{ private set; get; }

    private void Start()
    {
        _warfunds = 300;
        print(_warfunds);
        UpdateUIManager();
        UIManager.Instance.UpdateHP(_hP);
        
    }

    public void AddWarFunds(int funds)
    {
        print(funds);
        _warfunds += funds;
        UpdateUIManager();
    }


    public void PlaceTurret(int cost)
    {
        _warfunds -= cost;
        UpdateUIManager() ;
    }

    void UpdateUIManager()
    {
       
        UIManager.Instance.UpdateWarfunds(_warfunds);
    }

    public void LoseHP()
    {
        _hP--;
        UIManager.Instance.UpdateHP(_hP);
       
        if(_hP <= 0)
        {
            Debug.Log("EndGame");
        }
    }

    public void SetTimeScale(float time)
    {
        Time.timeScale = time;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}

