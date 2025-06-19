using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
    }

    public void AddWarFunds(int funds)
    {
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
}

