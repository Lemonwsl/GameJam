using UnityEngine;
using DamageNumbersPro;

public abstract class StatsPawn : MonoBehaviour
{
    [SerializeField] private float _health = 100f; public float health => _health;
    [SerializeField] private float _healthMax = 100f; public float HealthMax => _healthMax;

    [SerializeField] private DamageNumber numberPrefab;

    public void TakDamage(float amount)
    {
        _health = _health - amount;
        DamageNumber(amount);
        if (_health <= 0)
        {
            OnDeath();
        }
    }
    public void DamageNumber(float amount)
    {
        numberPrefab.Spawn(transform.position, amount);
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }
    
}