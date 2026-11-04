using System;
using UnityEngine;

public class RegeneratingHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float startingHealth = 100f;
    [SerializeField] private float regenPerSecond = 30f;

    private float currentHealth;

    public event Action<float> OnHPPctChanged = delegate { };
    public event Action OnDied = delegate { };

    public float CurrentHpPct
    {
        get { return currentHealth / startingHealth; }
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    private void Update()
    {
        if (currentHealth >= startingHealth)
            return;

        currentHealth = Mathf.Min(startingHealth, currentHealth + regenPerSecond * Time.deltaTime);

        OnHPPctChanged(CurrentHpPct);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        OnHPPctChanged(CurrentHpPct);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        OnDied();
        GameObject.Destroy(this.gameObject);
    }
}
