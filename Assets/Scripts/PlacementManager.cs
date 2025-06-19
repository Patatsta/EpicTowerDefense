using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private TurretManager _lastHovered;
    [SerializeField] private int _gunIndex = 0;
    private bool _isPlacing = false;

    private int _lastKnownWarfunds = -1;

    public static PlacementManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate GameManager detected, destroying this one.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!_isPlacing) return;

        if (GameManager.Instance._warfunds != _lastKnownWarfunds)
        {
            _lastKnownWarfunds = GameManager.Instance._warfunds;

            // Warfunds haben sich geändert → aktualisiere Hover-Zustand
            if (_lastHovered != null)
            {
                _lastHovered.TogglePlaceHolder(false, _gunIndex);
                _lastHovered.TogglePlaceHolder(true, _gunIndex);
            }
        }

        HandleHover();
        HandleClick();
    }

    private void HandleHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
        {
            TurretManager turret = hit.transform.GetComponent<TurretManager>();
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
        else
        {
            if (_lastHovered != null)
            {
                _lastHovered.TogglePlaceHolder(false, _gunIndex);
                _lastHovered = null;
            }
        }
    }

    private void HandleClick()
    {
        if (Input.GetMouseButtonDown(0) && _lastHovered != null && !_lastHovered.IsEnabled)
        {
            _lastHovered.Turret(_gunIndex);
        }
    }

    public void StartPlacing(int i)
    {
        _isPlacing = true;
        _gunIndex = i;
        _lastKnownWarfunds = GameManager.Instance._warfunds; // direkt setzen
    }

    public void StopPlacing()
    {
        _isPlacing = false;
    }
}
