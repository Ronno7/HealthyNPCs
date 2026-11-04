using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public event Action OnHit = delegate { };

    internal void TakeDamage(int amount)
    {
        OnHit();
        GetComponent<IHealth>().TakeDamage(amount);
    }
}
