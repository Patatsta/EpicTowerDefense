using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private TMP_Text _hPText;
    [SerializeField] private TMP_Text _waveCount;
    [SerializeField] private GameObject _upgradeGatImage, _upgradeLaunchImage, _dismantelImage;
    [SerializeField] private Button _upgradeGatButton, _upgradeLaunchButton;
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private TMP_Text _dismantelRefundText;
    [SerializeField] private Button _acceptDismantel;

    private void Start()
    {
     
        _upgradeGatImage.SetActive(false);
        _upgradeLaunchImage.SetActive(false);
        _dismantelImage.SetActive(false);
    }
    public void UpdateWarfunds(int amount)
    {
      
        _warfundsText.text = amount.ToString();
    }

    public void UpdateHP(int hp)
    {
        _hPText.text = hp.ToString();
        string status = "Good";

        if (hp <= 2)
        {
            status = "Critical";
        }
        else if (hp <= 5)
        {
            status = "Danger";
        }
        else if (hp <= 8)
        {
            status = "Caution";
        }

        _statusText.text = status;
    }

    public void UpdateWaveCount(int count)
    {
        _waveCount.text = count.ToString() + " / 20";
    }

    public void UpdateUpgrade(Turret tur)
    {
        if (tur._weaponIndex == 0)
        {
            _upgradeGatImage.SetActive(true);
            _upgradeGatButton.onClick.RemoveAllListeners();
            _upgradeGatButton.onClick.AddListener(tur.Upgrade);
            _upgradeGatButton.onClick.AddListener(ButtonPress);

        }
        else if (tur._weaponIndex == 1)
        {
            _upgradeLaunchImage.SetActive(true);
            _upgradeLaunchButton.onClick.RemoveAllListeners();
            _upgradeLaunchButton.onClick.AddListener(tur.Upgrade);
            _upgradeLaunchButton.onClick.AddListener(ButtonPress);
        }
        _dismantelRefundText.text = PlacementManager.Instance._turretStats[tur._weaponIndex].singleRefund.ToString();
        _dismantelImage.SetActive(true);
        _acceptDismantel.onClick.RemoveAllListeners();
        _acceptDismantel.onClick.AddListener(tur.Dismantel);
        _acceptDismantel.onClick.AddListener(ButtonPress);

    }

    public void UpdateNoUpgrade(Turret tur)
    {
      
        _dismantelRefundText.text = PlacementManager.Instance._turretStats[tur._weaponIndex].singleRefund.ToString();
        _dismantelImage.SetActive(true);
        _acceptDismantel.onClick.RemoveAllListeners();
        _acceptDismantel.onClick.AddListener(tur.Dismantel);
        _acceptDismantel.onClick.AddListener(ButtonPress);
    }

    public void CancelUpgrade()
    {
        _acceptDismantel.onClick.RemoveAllListeners();
        _upgradeGatButton.onClick.RemoveAllListeners();
    }

    public void ButtonPress()
    {
        _upgradeGatImage.SetActive(false);
        _upgradeLaunchImage.SetActive(false);
        _dismantelImage.SetActive(false);
    }
}
