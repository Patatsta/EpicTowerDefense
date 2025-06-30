using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    protected AudioSource _audioSource;
    protected bool _startWeaponNoise = true;
    protected Transform _currentTarget;
    protected Transform _rotateBody;
    [SerializeField] protected Transform _rotateSingle;
    [SerializeField] protected Transform _rotateDouble;
    [SerializeField] protected GameObject _singleTurret;
    [SerializeField] protected GameObject _doubleTurret;
    public TurretManager _turretManager;
    protected List<Transform> _enemiesInRange = new List<Transform>();
    protected IDamageable _enemyHealth;

    public int _weaponIndex;
    public bool _isUpgrade { get; protected set; }

    protected virtual void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _rotateBody = _rotateSingle;
    }

    protected void OnEnable()
    {
        _singleTurret.SetActive(true);
        _doubleTurret.SetActive(false);

        _enemiesInRange.Clear();
        _currentTarget = null;
        _enemyHealth = null;
        _startWeaponNoise = true;
        GameManager.OnPauseChanged += HandlePauseChanged;
    }

    protected virtual void Update()
    {
        _enemiesInRange.RemoveAll(enemy => enemy == null || !enemy.gameObject.activeInHierarchy);

        if (_currentTarget == null && _enemiesInRange.Count > 0)
        {
            _currentTarget = _enemiesInRange[0];
            _enemyHealth = _currentTarget.GetComponent<IDamageable>();
        }

        if (_currentTarget != null && !_currentTarget.gameObject.activeInHierarchy)
        {
            _enemiesInRange.Remove(_currentTarget);
            _currentTarget = null;
            _enemyHealth = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        if (other.CompareTag("Enemy"))
        {
            _enemiesInRange.Add(other.transform);
            if (_currentTarget == null)
            {
                _currentTarget = other.transform;
                _enemyHealth = _currentTarget.GetComponent<IDamageable>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == null) return;
        if (other.CompareTag("Enemy"))
        {
            _enemiesInRange.Remove(other.transform);

            if (_currentTarget == other.transform)
            {
                if (_enemiesInRange.Count > 0)
                {
                    _currentTarget = _enemiesInRange[0];
                    _enemyHealth = _currentTarget.GetComponent<IDamageable>();
                }
                else
                {
                    _currentTarget = null;
                    _enemyHealth = null;
                }
            }
        }
    }

    private void OnDisable()
    {
        GameManager.OnPauseChanged -= HandlePauseChanged;
    }

    protected void HandlePauseChanged(bool isPaused)
    {
        if (isPaused)
        {
            if (_audioSource.isPlaying)
                _audioSource.Pause();
        }
        else
        {
            if (_currentTarget != null && Time.timeScale > 0)
            {
                _audioSource.Play();
            }
        }
    }

    public abstract void Upgrade();
    public abstract void Dismantel();
}

