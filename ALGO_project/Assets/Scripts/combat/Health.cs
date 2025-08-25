using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public int CurrentHP { get; private set; }
    public bool IsDead => CurrentHP <= 0;

    public event Action<int,int> OnDamaged;
    public event Action<int,int> OnHealed;
    public event Action OnDied;

    private RuntimeStats stats;

    void Awake()
    {
        stats = GetComponent<RuntimeStats>();
        CurrentHP = stats != null ? stats.MaxHP : 100;
    }

    public void Damage(int amount)
    {
        if (IsDead) return;
        int dmg = Mathf.Max(0, amount);
        CurrentHP = Mathf.Max(0, CurrentHP - dmg);
        OnDamaged?.Invoke(dmg, CurrentHP);
        if (CurrentHP <= 0) OnDied?.Invoke();
    }

    public void Heal(int amount)
    {
        if (IsDead) return;
        int heal = Mathf.Max(0, amount);
        int maxHP = stats != null ? stats.MaxHP : 100;
        CurrentHP = Mathf.Min(maxHP, CurrentHP + heal);
        OnHealed?.Invoke(heal, CurrentHP);
    }
}
