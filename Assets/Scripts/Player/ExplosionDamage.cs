using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] LayerMask Enemy;
    public int damage;
    [SerializeField] private float _explosionRadius;
    private void Start()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, _explosionRadius, Enemy);

        foreach (Collider c in collider)
        {
            c.GetComponent<IDamageable>().TakeDamage(damage);
        }

    }
}
