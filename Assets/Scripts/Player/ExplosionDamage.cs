using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] LayerMask Enemy;
    private void Start()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, 20, Enemy);

        foreach (Collider c in collider)
        {
            c.GetComponent<IDamageable>().TakeDamage(25);
        }

    }
}
