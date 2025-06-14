using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int _heath;
    [SerializeField] private int _maxHealth;

    public void TakeDamage(int dmg)
    {
        _heath -= dmg;
        if( _heath <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _heath = _maxHealth;
    }


}
