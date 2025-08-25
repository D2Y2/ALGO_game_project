using UnityEngine;

public abstract class Trait : ScriptableObject
{
    [TextArea] public string description;

    public virtual void OnEquip(GameObject owner, RuntimeStats stats, Health hp) {}
    public virtual void OnUnequip(GameObject owner, RuntimeStats stats, Health hp) {}
    public virtual void OnTick(GameObject owner, RuntimeStats stats, Health hp, float dt) {}
    public virtual void OnDamaged(GameObject owner, RuntimeStats stats, Health hp, int amount) {}
    public virtual int ModifyOutgoingDamage(int baseDamage) => baseDamage;
}
