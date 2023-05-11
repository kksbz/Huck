using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public struct DamageMessage
{
    public GameObject causer;
    public float damageAmount;

    // 더 필요할만한거 알아서 추가하기
}
public interface IDamageable
{
    public virtual void TakeDamage(DamageMessage message)
=======
public class DamageMessage
{
    public GameObject causer;
    public int damageAmount;
    public ItemData item;
    public DamageMessage(GameObject causer, int damageAmount, ItemData item = default)
    {
        this.causer = causer;
        this.damageAmount = damageAmount;
        this.item = item;
    }
}
public interface IDamageable
{
    public void TakeDamage(DamageMessage message)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    {
        /* virtual method */
    }
}
