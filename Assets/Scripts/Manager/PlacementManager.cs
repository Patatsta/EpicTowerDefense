using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Wichtig für den UI-Check

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private LayerMask _posMask;
    [SerializeField] private LayerMask _upgradeMask;
    private TurretManager _lastHovered;
    [SerializeField] private int _gunIndex = 0;
    private bool _isPlacing = false;

    private int _lastKnownWarfunds = -1;
    [SerializeField] private Turret _currentTurret;
    public static PlacementManager Instance { get; private set; }

    public List<TurretStats> turretStats = new List<TurretStats>();
    [SerializeField] private GameObject _cancelButton;

    [System.Serializable]
    public class TurretStats
    {
        public int index;
        public int cost;
        public int upgradeCost;
        public int singleRefund;
        public int doubleRefund;
    }

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
        _cancelButton.SetActive(false);
      
    }
    private void Update()
    {
        if (GameManager.Instance.warfunds != _lastKnownWarfunds && _isPlacing)
        {
           
            _lastKnownWarfunds = GameManager.Instance.warfunds;

            if (_lastHovered != null)
            {
                _lastHovered.TogglePlaceHolder(false, _gunIndex);
                _lastHovered.TogglePlaceHolder(true, _gunIndex);
            }
        }

        HandleHover();

        if (Input.GetMouseButtonDown(0))
            HandleClick();
    }

    private void HandleHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _posMask))
        {
            TurretManager turret = hit.transform.GetComponent<TurretManager>();

            if (turret != null && turret != _lastHovered)
            {
                if (_lastHovered != null)
                {
                    _lastHovered.hoverGate?.SetActive(false);
                    _lastHovered.TogglePlaceHolder(false, _gunIndex);
                }

                turret.hoverGate?.SetActive(true);

                if (_isPlacing && !turret.IsEnabled)
                {
                    turret.TogglePlaceHolder(true, _gunIndex);
                }

                _lastHovered = turret;
            }

            if (!_isPlacing && _currentTurret == null)
            {
                _currentTurret = hit.transform.GetComponentInChildren<Turret>();
            }
        }
        else
        {
            if (_lastHovered != null)
            {
                _lastHovered.hoverGate?.SetActive(false);
                _lastHovered.TogglePlaceHolder(false, _gunIndex);
                _lastHovered = null;
            }

            if (_currentTurret != null)
            {
                _cancelButton.SetActive(false);
                _currentTurret._turretManager.upgradeButton.SetActive(false);
                _currentTurret._turretManager.dismantelButton.SetActive(false);
                _currentTurret = null;
            }
        }
    }


    private void HandleClick()
    {
     
        if (EventSystem.current.IsPointerOverGameObject())
            return;
      
        if (_lastHovered != null && !_lastHovered.IsEnabled && _isPlacing && PlacementManager.Instance.turretStats[_gunIndex].cost <= GameManager.Instance.warfunds)
        {
            _cancelButton.SetActive(false);
            _isPlacing = false;
            _lastHovered.Turret(_gunIndex);
            return;
        }

        if (_currentTurret != null)
        {
            if (!_currentTurret.isUpgrade)
            {
                UIManager.Instance.UpdateUpgrade(_currentTurret);
            }
            else
            {
                UIManager.Instance.UpdateNoUpgrade(_currentTurret);
            }
        }
    }

    public void StartPlacing(int i)
    {
        _isPlacing = true;
        _gunIndex = i;
        _lastKnownWarfunds = GameManager.Instance.warfunds;
    }

    public void StopPlacing()
    {
        _isPlacing = false;
    }
}

