using UnityEngine;

public interface IHitGate
{
    // 지금 공격 허용 여부 (소비 X)
    bool CanHit();
    // 이번 공격에서 1회 소비(중복 발동 방지용). 소비 개념이 없으면 CanHit()와 동일하게 반환.
    bool CanHitAndConsume();
    // 중앙 적중 등 보너스를 데미지 배수로 제공. 기본 1.0
    float GetDamageMultiplier();
}
