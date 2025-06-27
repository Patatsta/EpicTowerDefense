using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _warfunds = 10;
    [SerializeField] private Slider _healthSlider;

    private int _health;
    private bool _isDead = false;

    private void Start()
    {
 
        _healthSlider.maxValue = _maxHealth;
        ResetHealth();
    }

    private void OnEnable()
    {
        ResetHealth();
    }

    private void ResetHealth()
    {
        _health = _maxHealth;
        _healthSlider.value = _health;
        _isDead = false;
    }

    public void TakeDamage(int dmg)
    {
        if (_isDead) return;

        _health -= dmg;
        _healthSlider.value = _health;
        print(dmg);
        print(_health);

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
}



