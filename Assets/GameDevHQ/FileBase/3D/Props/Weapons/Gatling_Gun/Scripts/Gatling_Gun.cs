using System.Collections.Generic;
using UnityEngine;

public class Gatling_Gun : Turret
{
    [Header("Visuals")]
    [SerializeField] private GameObject _gat1, _gat2;
    [SerializeField] private List<GameObject> _single_Muzzle_Flash = new List<GameObject>();
    [SerializeField] private List<GameObject> _double_Muzzle_Flash = new List<GameObject>();
    [SerializeField] private List<ParticleSystem> _single_BulletCasings = new List<ParticleSystem>();
    [SerializeField] private List<ParticleSystem> _double_BulletCasings = new List<ParticleSystem>();

    [Header("Audio")]
    [SerializeField] private AudioClip fireSound1;
    [SerializeField] private AudioClip fireSound2;

    [Header("Barrels")]
    [SerializeField] private List<Transform> _single_Barrels = new List<Transform>();
    [SerializeField] private List<Transform> _double_Barrels = new List<Transform>();

    private List<Transform> _barrels = new List<Transform>();
    private List<GameObject> _muzzle_list = new List<GameObject>();
    private List<ParticleSystem> _bulletCasings = new List<ParticleSystem>();

    private float _timer = 0f;
    private int _damage = 1;
    private float _tickrate = 0.2f;

    [SerializeField] private float _singleTickrate, _doubleTickrate;
    [SerializeField] private int _singleDamage, _doubleDamage;

    protected override void Start()
    {
        base.Start();

        SetWeaponMode(false);

        foreach (var go in _muzzle_list)
            go.SetActive(false);

        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = true;
    }

    protected override void Update()
    {
        if (!_turretManager.IsEnabled) return;

        base.Update();

        if (_currentTarget != null)
            DamageLogic();

        TargetLogic();
    }

    private void TargetLogic()
    {
        bool hasTarget = _currentTarget != null;

        foreach (var go in _muzzle_list)
            go.SetActive(hasTarget);

        if (hasTarget)
        {
            RotateToTarget();
            RotateBarrels();

            if (_startWeaponNoise && Time.timeScale > 0)
            {
                if (!_audioSource.isPlaying)
                    _audioSource.Play();
                _startWeaponNoise = false;
            }
        }
        else
        {
            if (_audioSource.isPlaying)
                _audioSource.Stop();
            _startWeaponNoise = true;
        }
    }

    private void RotateToTarget()
    {
        _rotateBody.LookAt(_currentTarget.position);
    }

    private void RotateBarrels()
    {
        foreach (var barrel in _barrels)
        {
            barrel.Rotate(Vector3.forward * 1000f * Time.deltaTime);
        }
    }

    private void DamageLogic()
    {
        _timer += Time.deltaTime;
        if (_timer > _tickrate)
        {
            _timer = 0f;

            if (_enemyHealth != null && _currentTarget != null)
            {
                // Sicherheitshalber nochmal prüfen, dass enemyHealth zum currentTarget passt
                if (_enemyHealth == _currentTarget.GetComponent<IDamageable>())
                {
                    _enemyHealth.TakeDamage(_damage);

                    foreach (var ps in _bulletCasings)
                        ps.Emit(1);
                }
                else
                {
                    // Falls nicht, neu zuweisen
                    _enemyHealth = _currentTarget.GetComponent<IDamageable>();
                }
            }
        }
    }

    private void SetWeaponMode(bool upgraded)
    {
        _isUpgrade = upgraded;
        _muzzle_list = upgraded ? _double_Muzzle_Flash : _single_Muzzle_Flash;
        _bulletCasings = upgraded ? _double_BulletCasings : _single_BulletCasings;
        _barrels = upgraded ? _double_Barrels : _single_Barrels;

        _audioSource.clip = upgraded ? fireSound2 : fireSound1;

        _rotateBody = upgraded ? _rotateDouble : _rotateSingle;
        _damage = upgraded ? _doubleDamage : _singleDamage;
        _tickrate = upgraded ? _doubleTickrate : _singleTickrate;

        _gat1.SetActive(!upgraded);
        _gat2.SetActive(upgraded);

        // Wichtig: Ziel und enemyHealth neu zuweisen nach Moduswechsel
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

        if (_currentTarget != null && Time.timeScale > 0)
        {
            _audioSource.Play();
            _startWeaponNoise = false;
        }
        else
        {
            _startWeaponNoise = true;
        }
    }

    public override void Upgrade()
    {
        int upgradeCost = PlacementManager.Instance._turretStats[0].upgradeCost;
        if (GameManager.Instance._warfunds >= upgradeCost)
        {
            GameManager.Instance.PlaceTurret(upgradeCost);
            SetWeaponMode(true);
        }
    }

    public override void Dismantel()
    {
        _turretManager.Dismantel(_weaponIndex, _isUpgrade);
        SetWeaponMode(false);
    }
}
