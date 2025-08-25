using UnityEngine;
using System.Collections.Generic;

public class TraitController : MonoBehaviour
{
    [SerializeField] private List<Trait> equippedTraits = new();
    private RuntimeStats stats;
    private Health hp;

    void Awake()
    {
        stats = GetComponent<RuntimeStats>();
        hp = GetComponent<Health>();
    }

    void OnEnable()
    {
        if (hp != null) hp.OnDamaged += HandleDamaged;
        foreach (var t in equippedTraits) t?.OnEquip(gameObject, stats, hp);
    }

    void OnDisable()
    {
        foreach (var t in equippedTraits) t?.OnUnequip(gameObject, stats, hp);
        if (hp != null) hp.OnDamaged -= HandleDamaged;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        foreach (var t in equippedTraits) t?.OnTick(gameObject, stats, hp, dt);
    }

    void HandleDamaged(int amount, int _current) {
        foreach (var t in equippedTraits) t?.OnDamaged(gameObject, stats, hp, amount);
    }

    public int ApplyOutgoingDamageMods(int baseDamage)
    {
        int v = baseDamage;
        foreach (var t in equippedTraits) v = t != null ? t.ModifyOutgoingDamage(v) : v;
        return v;
    }

    public void Equip(Trait t)
    {
        if (t == null) return;
        equippedTraits.Add(t);
        t.OnEquip(gameObject, stats, hp);
    }

    public void Unequip(Trait t)
    {
        if (t == null) return;
        if (equippedTraits.Remove(t)) t.OnUnequip(gameObject, stats, hp);
    }
}
