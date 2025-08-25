//기본적으로 여러 요소가 공통적으로 가질 수 있는 특성의 기본값(HP,데미지 등) 관리
// Assets/Scripts/Stats/StatBlock.cs
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/StatBlock")]
public class StatBlock : ScriptableObject
{
    [Header("Base Stats")]
    public int maxHP = 100;
    public int attack = 10;
    public int defense = 0;
    public float moveSpeed = 5f;
}
