using System;
using System.Collections;
using UnityEngine;

public class TurncoatHealth : MonoBehaviour, IHealth
{
    [SerializeField] private int startingHealth = 100;
    [SerializeField] private float secondsBetweenFlips = 3f;
    [SerializeField] private Color hurtsWhenHitColor = new Color(0.8f, 0.2f, 0.9f);
    [SerializeField] private Color healsWhenHitColor = new Color(1f, 0.8f, 0.1f);

    private int currentHealth;
    private bool healsWhenHit;
    private Renderer bodyRenderer;

    public event Action<float> OnHPPctChanged = delegate { };
    public event Action OnDied = delegate { };

    public float CurrentHpPct
    {
        get { return (float)currentHealth / (float)startingHealth; }
    }

    private void Start()
    {
        currentHealth = startingHealth;
        bodyRenderer = GetComponent<Renderer>();
        bodyRenderer.material.color = hurtsWhenHitColor;
        StartCoroutine(FlipBetweenHurtingAndHealing());
    }

    public void TakeDamage(int amount)
    {
        if (healsWhenHit)
            currentHealth = Mathf.Min(startingHealth, currentHealth + amount);
        else
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

    private IEnumerator FlipBetweenHurtingAndHealing()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsBetweenFlips);
            healsWhenHit = !healsWhenHit;
            bodyRenderer.material.color = healsWhenHit ? healsWhenHitColor : hurtsWhenHitColor;
        }
    }
}
