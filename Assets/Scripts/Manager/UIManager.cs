using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TMP_Text _warfundsText;
    [SerializeField] private TMP_Text _hPText;
    [SerializeField] private TMP_Text _waveCount;
    [SerializeField] private GameObject _upgradeGatImage, _upgradeLaunchImage, _dismantelImage;
    [SerializeField] private Button _upgradeGatButton, _upgradeLaunchButton;
    [SerializeField] private TMP_Text _launchUpText, _gatUpText;
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private TMP_Text _dismantelRefundText;
    [SerializeField] private Button _acceptDismantel;
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private TMP_Text _endScreenText;
    [SerializeField] private GameObject _gameUI;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private GameObject _pauseScreen;

    private Turret _currentTur;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _winClip;
    [SerializeField] private AudioClip _loseClip;
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
        _audioSource = GetComponent<AudioSource>();
        _pauseScreen.SetActive(false);
        _gameUI.SetActive(true);
        _upgradeGatImage.SetActive(false);
        _upgradeLaunchImage.SetActive(false);
        _dismantelImage.SetActive(false);
        _endScreen.SetActive(false);

        if (musicSlider != null)
        {
            musicSlider.value = SoundManager.Instance.MusicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = SoundManager.Instance.SFXVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void UpdateWarfunds(int amount)
    {
        _warfundsText.text = amount.ToString();
    }

    public void UpdateHP(int hp)
    {
        _hPText.text = hp.ToString();
        string status = "Good";

        if (hp <= 2) status = "Critical";
        else if (hp <= 5) status = "Danger";
        else if (hp <= 8) status = "Caution";

        _statusText.text = status;
    }

    public void UpdateWaveCount(int count)
    {
        _waveCount.text = count.ToString() + " / 10";
    }

    public void UpdateUpgrade(Turret tur)
    {
        _currentTur = tur;
        if (tur._weaponIndex == 0)
        {
            _gatUpText.text = PlacementManager.Instance.turretStats[tur._weaponIndex].upgradeCost.ToString();
            _upgradeGatButton.onClick.RemoveAllListeners();
            _upgradeGatButton.onClick.AddListener(tur.Upgrade);
            _upgradeGatButton.onClick.AddListener(ButtonPress);
        }
        else if (tur._weaponIndex == 1)
        {
            _launchUpText.text = PlacementManager.Instance.turretStats[tur._weaponIndex].upgradeCost.ToString();
            _upgradeLaunchButton.onClick.RemoveAllListeners();
            _upgradeLaunchButton.onClick.AddListener(tur.Upgrade);
            _upgradeLaunchButton.onClick.AddListener(ButtonPress);
        }

        tur._turretManager.upgradeButton.SetActive(true);
        tur._turretManager.dismantelButton.SetActive(true);

        _dismantelRefundText.text = PlacementManager.Instance.turretStats[tur._weaponIndex].singleRefund.ToString();

        _acceptDismantel.onClick.RemoveAllListeners();
        _acceptDismantel.onClick.AddListener(tur.Dismantel);
        _acceptDismantel.onClick.AddListener(ButtonPress);
    }

    public void UpdateNoUpgrade(Turret tur)
    {
        _dismantelRefundText.text = PlacementManager.Instance.turretStats[tur._weaponIndex].doubleRefund.ToString();
        tur._turretManager.dismantelButton.SetActive(true);
        _acceptDismantel.onClick.RemoveAllListeners();
        _acceptDismantel.onClick.AddListener(tur.Dismantel);
        _acceptDismantel.onClick.AddListener(ButtonPress);
    }

    public void UpgradeButtonPressed()
    {
        if (_currentTur._weaponIndex == 0) _upgradeGatImage.SetActive(true);
        else _upgradeLaunchImage.SetActive(true);
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

    public void EndGame(bool win)
    {
        _gameUI.SetActive(false);
        _endScreen.SetActive(true);
        _endScreenText.text = win ? "Victory" : "Defeat";
        if (win)
        {
            _audioSource.clip = _winClip;
        }
        else
        {
            _audioSource.clip = _loseClip;
        }
        SoundManager.Instance.StopMusic();
        _audioSource.Play();
        GameManager.Instance.GameOver();
    }

    private void SetMusicVolume(float value)
    {
     
        SoundManager.Instance.SetMusicVolume(value);
    }

    private void SetSFXVolume(float value)
    {
     
        SoundManager.Instance.SetSFXVolume(value);
    }

    public void UIPauseScreen(bool on)
    {
        _pauseScreen.SetActive(on);
    }
}
