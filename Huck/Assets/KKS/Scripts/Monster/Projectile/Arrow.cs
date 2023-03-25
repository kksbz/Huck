using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private int damage = default;
    [SerializeField] private float speed = default;
    private DamageMessage damageMessage = default;
    private Rigidbody arrowRb = default;
    
    private void OnEnable()
    {
        arrowRb = GetComponent<Rigidbody>();
        damageMessage = new DamageMessage(gameObject, damage);
        arrowRb.AddForce(transform.forward * speed, ForceMode.Impulse);
        StartCoroutine(EnqueueArrow());
    } // OnEnable

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GData.PLAYER_MASK || other.tag == GData.BUILD_MASK)
        {
            other.gameObject.GetComponent<IDamageable>().TakeDamage(damageMessage);
        }
        arrowRb.velocity = Vector3.zero;
        ArrowPool.Instance.ReturnArrow(gameObject);
    } // OnTriggerEnter

    //! 발사한 화살 회수하는 함수
    private IEnumerator EnqueueArrow()
    {
        yield return new WaitForSeconds(5f);
        if (gameObject.activeInHierarchy == true)
        {
            arrowRb.velocity = Vector3.zero;
            ArrowPool.Instance.ReturnArrow(gameObject);
        }
    } // EnqueueArrow
}
