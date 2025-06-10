using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameDevHQ.FileBase.Gatling_Gun
{

    [RequireComponent(typeof(AudioSource))]
    public class Gatling_Gun : MonoBehaviour
    {
        private Transform _gunBarrel; 
        public GameObject Muzzle_Flash; 
        public ParticleSystem bulletCasings; 
        public AudioClip fireSound;

        private AudioSource _audioSource;
        private bool _startWeaponNoise = true;

        private Transform _currentTarget;
        private Transform _rotateBody;

        private List<Transform> _enemiesInRange = new List<Transform>();

        private EnemyHealth _enemyHealth;

        [SerializeField] private float _tickrate = 0.2f;
        private float _timer = 0;   
      
        void Start()
        {
            _rotateBody = transform.GetChild(0);
            _gunBarrel = GameObject.Find("Barrel_to_Spin").GetComponent<Transform>(); 
            Muzzle_Flash.SetActive(false); 
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = true; 
            _audioSource.clip = fireSound; 
        }

      
        void Update()
        {
            if(_currentTarget != null)
            {
                DamageLogic();
            }
            TargetLogic();
        }

        void DamageLogic()
        {
            _timer += Time.deltaTime;
            if (_timer > _tickrate)
            {
                _timer = 0;
                _enemyHealth.TakeDamage();
            }
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

        private void OnTriggerEnter(Collider other)
        {
            if (other == null) return;
            if (other.CompareTag("Enemy"))
            {
                print("Enter");
                _enemiesInRange.Add(other.transform);
                if(_currentTarget == null)
                {
                    _enemyHealth = other.GetComponent<EnemyHealth>();
                    _currentTarget = other.transform;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other == null) return;
            if (other.CompareTag("Enemy"))
            {
                print("Exit");
                _enemiesInRange.Remove(other.transform);
                if(_enemiesInRange.Count > 0)
                {
                    _enemyHealth = other.GetComponent<EnemyHealth>();
                    _currentTarget = _enemiesInRange[0];
                }
                else
                {
                    _enemyHealth = null;
                    _currentTarget = null;
                }
            }
        }
    }

}
