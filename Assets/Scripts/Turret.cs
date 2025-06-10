using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{


    protected AudioSource _audioSource;
    protected bool _startWeaponNoise = true;

    protected Transform _currentTarget;
    [SerializeField] protected Transform _rotateBody;

    protected List<Transform> _enemiesInRange = new List<Transform>();
   
    protected IDamageable _enemyHealth;

   
    protected virtual void Start()
    {
        
        _audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        if(_currentTarget == null && _enemiesInRange.Count > 1)
        {
            _enemiesInRange.RemoveAt(0);
            if (_enemiesInRange[0] != null)
            {
                _enemyHealth = _enemiesInRange[0].GetComponent<IDamageable>();
                _currentTarget = _enemiesInRange[0];
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("enter");
        if (other == null) return;
        if (other.CompareTag("Enemy"))
        {
           
            _enemiesInRange.Add(other.transform);
            if (_currentTarget == null)
            {
                _enemyHealth = other.GetComponent<IDamageable>();
                _currentTarget = other.transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("exit");
        if (other == null) return;
        if (other.CompareTag("Enemy"))
        {
          
            _enemiesInRange.Remove(other.transform);
            if (_enemiesInRange.Count > 0)
            {
                _enemyHealth = other.GetComponent<IDamageable>();
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
