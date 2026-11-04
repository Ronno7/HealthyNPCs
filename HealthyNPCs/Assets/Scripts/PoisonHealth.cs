using System;
using UnityEngine;

public class PoisonHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float startingHealth = 100f;
    [SerializeField] private float hpLossPerSecondPerHit = 8f;

    private float currentHealth;
    private int hitsTaken;

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
        if (hitsTaken == 0)
            return;

        currentHealth -= hitsTaken * hpLossPerSecondPerHit * Time.deltaTime;

        OnHPPctChanged(CurrentHpPct);

        if (currentHealth <= 0)
            Die();
    }

    // The amount is ignored: a hit does no direct damage, it adds another stack of poison.
    public void TakeDamage(int amount)
    {
        hitsTaken++;
    }

    private void Die()
    {
        OnDied();
        GameObject.Destroy(this.gameObject);
    }
}
