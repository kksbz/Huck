using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageMessage
{
    public GameObject causer;
    public float damageAmount;

    // 더 필요할만한거 알아서 추가하기
}
public interface IDamageable
{
    void TakeDamage(GameObject attacker, int damage);
}
