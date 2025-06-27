using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
   
      
        
            if (GameManager.Instance._warfunds != _lastKnownWarfunds)
            {
                _lastKnownWarfunds = GameManager.Instance._warfunds;

                if (_lastHovered != null)
                {
                    _lastHovered.TogglePlaceHolder(false, _gunIndex);
                    _lastHovered.TogglePlaceHolder(true, _gunIndex);
                }
            }

            HandleHover();
        

     
        if(Input.GetMouseButtonDown(0)) HandleClick();
    }

    private void HandleHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
       
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _posMask))
        {
            TurretManager turret = hit.transform.GetComponent<TurretManager>();
            if (_isPlacing)
            {
               
                if (turret == null) return;

                if (_lastHovered != turret)
                {
                    if (_lastHovered != null)
                    {
                        _lastHovered.TogglePlaceHolder(false, _gunIndex);
                    }

                    if (!turret.IsEnabled)
                    {
                        turret.TogglePlaceHolder(true, _gunIndex);
                    }

                    _lastHovered = turret;
                }

            }
            else if (_currentTurret == null)
            {

                _currentTurret = hit.transform.GetComponentInChildren<Turret>();
            }
        }
        else
        {
            if (_lastHovered != null)
            {
                _lastHovered.TogglePlaceHolder(false, _gunIndex);
                _lastHovered = null;
              
            }
            if (_currentTurret != null)
            {
                _currentTurret._turretManager._upgradeButton.SetActive(false);
                _currentTurret._turretManager._dismantelButton.SetActive(false);
                _currentTurret = null;
            }
        }
       
    }

  

    private void HandleClick()
    {
        if (_lastHovered != null && !_lastHovered.IsEnabled && _isPlacing)
        {
            _isPlacing = false;
            _lastHovered.Turret(_gunIndex);
            return;
        }
        if(_currentTurret != null)
        {
            if (!_currentTurret._isUpgrade)
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
        _lastKnownWarfunds = GameManager.Instance._warfunds;
    }

    public void StopPlacing()
    {
        _isPlacing = false;
    }


    public List<TurretStats> _turretStats = new List<TurretStats>();

    [System.Serializable]
    public class TurretStats
    {
        public int index;
        public int cost;
        public int upgradeCost;
        public int singleRefund;
        public int doubleRefund;
    }
}
