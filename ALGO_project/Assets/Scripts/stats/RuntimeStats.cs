using UnityEngine;

public class RuntimeStats : MonoBehaviour
{
    [SerializeField] private StatBlock baseStats;
    public StatBlock Base => baseStats;

    public int MaxHP { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public float MoveSpeed { get; private set; }

    void Awake() => ResetFromBase();

    public void ResetFromBase()
    {
        MaxHP     = baseStats != null ? baseStats.maxHP : 100;
        Attack    = baseStats != null ? baseStats.attack : 10;
        Defense   = baseStats != null ? baseStats.defense : 0;
        MoveSpeed = baseStats != null ? baseStats.moveSpeed : 5f;
    }

    public void AddAttack(int delta)  => Attack = Mathf.Max(0, Attack + delta);
    public void AddDefense(int delta) => Defense = Mathf.Max(0, Defense + delta);
    public void AddMoveSpeed(float d) => MoveSpeed = Mathf.Max(0f, MoveSpeed + d);
    public void AddMaxHP(int delta)   => MaxHP = Mathf.Max(1, MaxHP + delta);
}
