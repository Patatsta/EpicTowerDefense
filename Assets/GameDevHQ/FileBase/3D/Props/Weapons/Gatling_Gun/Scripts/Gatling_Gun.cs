using System.Collections.Generic;
using UnityEngine;

public class Gatling_Gun : Turret
{
    [SerializeField] private GameObject _gat1, _gat2;
    [SerializeField] private List<GameObject> _single_Muzzle_Flash = new List<GameObject>();
    [SerializeField] private List<GameObject> _double_Muzzle_Flash = new List<GameObject>();
    [SerializeField] private List<ParticleSystem> _single_BulletCasings = new List<ParticleSystem>();
    [SerializeField] private List<ParticleSystem> _double_BulletCasings = new List<ParticleSystem>();

    [SerializeField] private AudioClip fireSound1;
    [SerializeField] private AudioClip fireSound2;
    [SerializeField] private float _tickrate = 0.2f;

    [SerializeField] private List<Transform> _single_Barrels = new List<Transform>();
    [SerializeField] private List<Transform> _double_Barrels = new List<Transform>();
    private List<Transform> _barrels = new List<Transform>();

    private List<GameObject> _muzzle_list = new List<GameObject>();
    private List<ParticleSystem> _bulletCasings = new List<ParticleSystem>();
    private float _timer = 0f;

    protected override void Start()
    {
        base.Start();
        SetWeaponMode(upgraded: false);

        foreach (GameObject go in _muzzle_list)
            go.SetActive(false);

        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = true;
    }

    protected override void Update()
    {
        base.Update();

        if (_currentTarget != null)
        {
            DamageLogic();
        }

        TargetLogic();
    }

    private void TargetLogic()
    {
        bool hasTarget = _currentTarget != null;

        foreach (GameObject go in _muzzle_list)
            go.SetActive(hasTarget);

        if (hasTarget)
        {
            RotateToTarget();
            RotateBarrels();

            foreach (ParticleSystem ps in _bulletCasings)
                ps.Emit(1);

            if (_startWeaponNoise)
            {
                _audioSource.Play();
                _startWeaponNoise = false;
            }
        }
        else
        {
            _audioSource.Stop();
            _startWeaponNoise = true;
        }
    }

    private void RotateToTarget()
    {
        _rotateBody.transform.LookAt(_currentTarget.position);
    }

    private void RotateBarrels()
    {
        foreach (Transform barrel in _barrels)
        {
            barrel.Rotate(Vector3.forward * 1000f * Time.deltaTime);
        }
    }

    private void DamageLogic()
    {
        _timer += Time.deltaTime;
        if (_timer > _tickrate)
        {
            _timer = 0;
            _enemyHealth.TakeDamage(1);
        }
    }

    public void UpgradeTurret()
    {
        SetWeaponMode(upgraded: true);
    }

    private void SetWeaponMode(bool upgraded)
    {
        _muzzle_list = upgraded ? _double_Muzzle_Flash : _single_Muzzle_Flash;
        _bulletCasings = upgraded ? _double_BulletCasings : _single_BulletCasings;
        _barrels = upgraded ? _double_Barrels : _single_Barrels;
        if(_audioSource.clip != null)
        {
            _audioSource.clip = upgraded ? fireSound2 : fireSound1;
            _audioSource.Play();
        }
        else
        {
            _audioSource.clip = upgraded ? fireSound2 : fireSound1;
        }
     
        _rotateBody = upgraded ? _rotateDouble : _rotateSingle;

        _gat1.SetActive(!upgraded);
        _gat2.SetActive(upgraded);  
    }
}
