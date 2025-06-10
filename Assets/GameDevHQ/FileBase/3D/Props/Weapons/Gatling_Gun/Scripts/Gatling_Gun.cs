using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameDevHQ.FileBase.Gatling_Gun
{

    [RequireComponent(typeof(AudioSource))]
    public class Gatling_Gun : Turret
    {
        [SerializeField] private Transform _gunBarrel;
        [SerializeField] private GameObject Muzzle_Flash;
        [SerializeField] private ParticleSystem bulletCasings;
        [SerializeField] private AudioClip fireSound;

   

        [SerializeField] private float _tickrate = 0.2f;
        private float _timer = 0;

        protected override void Start()
        {
            base.Start();
            _gunBarrel = GameObject.Find("Barrel_to_Spin").GetComponent<Transform>();
            Muzzle_Flash.SetActive(false);
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = true;
            _audioSource.clip = fireSound;
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

  
    void TargetLogic()
        {
            if (_currentTarget != null)
            {
                RotateBarrel();
                Muzzle_Flash.SetActive(true);
                bulletCasings.Emit(1);

                if (_startWeaponNoise == true)
                {
                    _audioSource.Play();
                    _startWeaponNoise = false;
                }

            }
            else if (_currentTarget == null)
            {
                Muzzle_Flash.SetActive(false);
                _audioSource.Stop();
                _startWeaponNoise = true;
            }
        }

        void RotateBarrel()
        {
            _rotateBody.transform.LookAt(_currentTarget.position);
        }

        void DamageLogic()
        {
            _timer += Time.deltaTime;
            if (_timer > _tickrate)
            {
                _timer = 0;
                _enemyHealth.TakeDamage(1);
            }
        }
    }
}


