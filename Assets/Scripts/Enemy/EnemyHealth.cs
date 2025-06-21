using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _warfunds;

    private bool _isDead = false;

    public void TakeDamage(int dmg)
    {
        if (_isDead) return;

        _health -= dmg;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (_isDead) return;

        _isDead = true;
        GameManager.Instance.AddWarFunds(_warfunds);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        _health = _maxHealth;
    }

    private void OnEnable()
    {
        _health = _maxHealth;
        _isDead = false;
    }
}


