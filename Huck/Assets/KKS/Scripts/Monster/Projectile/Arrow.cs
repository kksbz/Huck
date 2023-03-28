using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed = default;
    private DamageMessage damageMessage = default;
    private Rigidbody arrowRb = default;

    private void OnEnable()
    {
        arrowRb = GetComponent<Rigidbody>();
        arrowRb.AddForce(transform.forward * speed, ForceMode.Impulse);
        StartCoroutine(EnqueueArrow());
    } // OnEnable

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != GData.ENEMY_MASK)
        {
            if (other.tag == GData.PLAYER_MASK || other.tag == GData.BUILD_MASK)
            {
                other.gameObject.GetComponent<IDamageable>().TakeDamage(damageMessage);
            }
            arrowRb.velocity = Vector3.zero;
            ProjectilePool.Instance.EnqueueProjecttile(gameObject);
        }
    } // OnTriggerEnter

    //! �������޽����� ��ü�� �޾ƿ� �Լ�
    public void InitDamageMessage(GameObject attacker, int damage)
    {
        damageMessage = new DamageMessage(attacker, damage);
    } // InitDamageMessage

    //! �߻��� ȭ�� ȸ���ϴ� �Լ�
    private IEnumerator EnqueueArrow()
    {
        yield return new WaitForSeconds(5f);
        if (gameObject.activeInHierarchy == true)
        {
            arrowRb.velocity = Vector3.zero;
            ProjectilePool.Instance.EnqueueProjecttile(gameObject);
        }
    } // EnqueueArrow
}
