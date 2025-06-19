using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate GameManager detected, destroying this one.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private TMP_Text _warfundsText;
    private void Start()
    {
        _warfundsText.text = 0.ToString();
    }
    public void UpdateWarfunds(int amount)
    {
       
        _warfundsText.text = amount.ToString();
    }
}
