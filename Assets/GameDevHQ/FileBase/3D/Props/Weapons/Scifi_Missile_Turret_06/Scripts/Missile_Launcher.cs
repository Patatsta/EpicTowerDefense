using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameDevHQ.FileBase.Missile_Launcher
{
    public class Missile_Launcher : Turret
    {
        [SerializeField]
        private GameObject _missilePrefab; 
        
        
        private List<GameObject> _missilePositions = new List<GameObject>();

        [SerializeField] private List<GameObject> _missileSingle = new List<GameObject>();
        [SerializeField] private List<GameObject> _missileDouble = new List<GameObject>();

        [SerializeField]
        private float _destroyTime = 10.0f; 


        [SerializeField] private int _missleIndex = 0;


        [SerializeField] private float _tickrate = 0.2f;
        private float _timer = 0;

        [SerializeField] private float _flightDuration;

        protected override void Start()
        {
            base.Start();
            _missilePositions = _missileSingle;
        }

        protected override void Update()
        {
            base.Update();
            TargetLogic();
        }


        void TargetLogic()
        {
            if (_currentTarget != null)
            {
                RotateBarrel();
               

                if (_startWeaponNoise == true)
                {
                    _audioSource.Play();
                    _startWeaponNoise = false;
                }

                _timer += Time.deltaTime;
                if (_timer >= _tickrate)
                {
                    FireRocket();
                    _timer = 0;
                }

            }
            else if (_currentTarget == null)
            {
               
                _audioSource.Stop();
                _startWeaponNoise = true;
            }
        }

        void RotateBarrel()
        {
            _rotateBody.transform.LookAt(_currentTarget.position);
        }

        void FireRocket()
        {
            
          GameObject rocket = Instantiate(_missilePrefab) as GameObject;

          rocket.transform.parent = _missilePositions[_missleIndex].transform;
          rocket.transform.localPosition = Vector3.zero; 
          rocket.transform.localEulerAngles = new Vector3(-90, 0, 0); 
          rocket.transform.parent = null;

            rocket.GetComponent<Missile>().AssignMissleRules(_currentTarget.position, _destroyTime, _flightDuration);

            _missilePositions[_missleIndex].SetActive(false); 

            _missleIndex++;
            if (_missleIndex >= _missilePositions.Count)
            {
                _missleIndex = 0;
                ResetLauncher();
            }
           
        }

        void ResetLauncher()
        {
            for (int i = 0; i < _missilePositions.Count; i++) 
            {

                _missilePositions[i].SetActive(true); 
            }
        }


        public override void Upgrade()
        {
            if (GameManager.Instance._warfunds >= PlacementManager.Instance._turretStats[1].upgradeCost)
            {
                GameManager.Instance.PlaceTurret(PlacementManager.Instance._turretStats[1].upgradeCost);
                SetWeaponMode(true);
            }
        }

        public override void Dismantel()
        {
            _turretManager.Dismantel(_weaponIndex, _isUpgrade);
            SetWeaponMode(false);        
            _audioSource.Stop();
        }
        private void SetWeaponMode(bool upgraded)
        {
            _isUpgrade = upgraded;
      
            _missilePositions = upgraded ? _missileDouble : _missileSingle;
            _tickrate = upgraded ? _tickrate / 2 : _tickrate * 2; 

            _singleTurret.SetActive(!upgraded);
            _doubleTurret.SetActive(upgraded);

            _rotateBody = upgraded ? _rotateDouble : _rotateSingle;

       
            if (_audioSource != null)
            {
                _audioSource.Stop();

                _startWeaponNoise = true;
            }

            ResetLauncher(); 
            _timer = 0f;
            _missleIndex = 0;
            _currentTarget = null;
        }


    }
}

