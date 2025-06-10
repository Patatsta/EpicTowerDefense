using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *@author GameDevHQ 
 * For support, visit gamedevhq.com
 */

namespace GameDevHQ.FileBase.Missile_Launcher
{
    public class Missile_Launcher : Turret
    {
        [SerializeField]
        private GameObject _missilePrefab; 
        
        [SerializeField]
        private GameObject[] _misslePositions; 
        [SerializeField]
        private float _destroyTime = 10.0f; 


        [SerializeField] private int _missleIndex = 0;


        [SerializeField] private float _tickrate = 0.2f;
        private float _timer = 0;

        [SerializeField] private float _flightDuration;

        protected override void Start()
        {
            base.Start();

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

          rocket.transform.parent = _misslePositions[_missleIndex].transform;
          rocket.transform.localPosition = Vector3.zero; 
          rocket.transform.localEulerAngles = new Vector3(-90, 0, 0); 
          rocket.transform.parent = null;

            rocket.GetComponent<Missile>().AssignMissleRules(_currentTarget.position, _destroyTime, _flightDuration);

            _misslePositions[_missleIndex].SetActive(false); 

            _missleIndex++;
            if (_missleIndex >= _misslePositions.Length)
            {
                _missleIndex = 0;
                ResetLauncher();
            }
           
        }

        void ResetLauncher()
        {
            for (int i = 0; i < _misslePositions.Length; i++) 
            {

                _misslePositions[i].SetActive(true); 
            }
        }
    }
}

