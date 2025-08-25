using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private MonoBehaviour hitGateSource; // HitGateFromMinigame를 드래그
    private IHitGate hitGate;

    [SerializeField] private TraitController traits;
    [SerializeField] private RuntimeStats myStats;

    void Awake()
    {
        if (hitGate == null && hitGateSource != null)
            hitGate = hitGateSource as IHitGate;
        if (myStats == null) myStats = GetComponent<RuntimeStats>();
        if (traits == null) traits = GetComponent<TraitController>();
    }

    // 애니메이션 이벤트/키 입력/버튼에서 호출: 타겟 전달
    public void PerformAttack(GameObject target)
    {
        if (target == null) return;
        if (hitGate != null && !hitGate.CanHitAndConsume()) {
            Debug.Log("[BasicAttack] Hit blocked by gate.");
            return;
        }

        var targetHP = target.GetComponent<Health>();
        if (targetHP == null) {
            Debug.LogWarning("[BasicAttack] Target has no Health.");
            return;
        }

        if (myStats == null) myStats = GetComponent<RuntimeStats>();

        int baseDmg = myStats != null ? myStats.Attack : 10;
        int finalDmg = traits != null ? traits.ApplyOutgoingDamageMods(baseDmg) : baseDmg;

        var targetStats = target.GetComponent<RuntimeStats>();
        int defense = targetStats != null ? targetStats.Defense : 0;
        int dealt = Mathf.Max(0, finalDmg - defense);

        targetHP.Damage(dealt);
        Debug.Log($"[BasicAttack] Dealt {dealt} damage (base:{baseDmg}, def:{defense}).");
    }
}
