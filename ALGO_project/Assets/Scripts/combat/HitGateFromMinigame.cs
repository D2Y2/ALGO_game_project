using UnityEngine;



public class HitGateFromMinigame : MonoBehaviour, IHitGate
{
    [SerializeField] private MinigameManager minigame;
    [SerializeField] private bool consumeOnHit = true;
    private bool consumed = false;

    public bool CanHit()
    {
        if (minigame == null) return true; // 참고용: 미니게임 미연결 시 통과
        if (consumed && consumeOnHit) return false;
        return minigame.IsInSuccessZone();
    }

    public bool CanHitAndConsume()
    {
        bool ok = CanHit();
        if (ok && consumeOnHit) consumed = true;
        return ok;
    }

    public float GetDamageMultiplier() => 1f;

    public void ResetConsumption() => consumed = false;
}
