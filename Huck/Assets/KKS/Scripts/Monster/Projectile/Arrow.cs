using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using Unity.VisualScripting;
=======
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
using UnityEngine;

public class Arrow : MonoBehaviour
{
<<<<<<< HEAD
    private Rigidbody arrowRb = default;
    private CapsuleCollider arrowCollider = default;
    private MonsterController mController = default;
    private GameObject target = default;
    private bool isHit = false;
    private float gravityScale = 2f;
    void Awake()
    {
        arrowRb = gameObject.GetComponent<Rigidbody>();
        arrowCollider = gameObject.GetComponent<CapsuleCollider>();
        mController = transform.parent.gameObject.GetComponent<MonsterController>();
        target = mController.targetSearch.hit.gameObject;
    } // Awake

    private void OnEnable()
    {
        // �θ������Ʈ�� �������� ���󰡴°� �����ϱ� ���� ó��
        transform.parent = default;
        // ���� ������ �߷��� �����ϱ� ���� �⺻�߷� false
        arrowRb.useGravity = false;
        arrowRb.velocity = Vector3.zero;
        isHit = false;
        arrowCollider.isTrigger = true;
        arrowRb.AddForce(transform.forward * 20f, ForceMode.VelocityChange);
        StartCoroutine(EnqueueArrow());
    } // OnEnable

    private void FixedUpdate()
    {
        // ���ư��� �߿��� ���� ������ �߷� ����
        if (arrowRb.useGravity == false)
        {
            arrowRb.AddForce(Vector3.down * gravityScale, ForceMode.Acceleration);
        }
        // ȭ���� ������ ���� �޴� �������� ���ϵ��� �ϴ� ó��
        if (isHit == false && transform.forward != Vector3.zero)
        {
            transform.forward = arrowRb.velocity;
        }
    } // FixedUpdate

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.tag == GData.PLAYER_MASK || other.tag == GData.BUILD_MASK)
            {
                Debug.Log($"�÷��̾� ����! {other.tag}");
            }
            isHit = true;
            arrowCollider.isTrigger = false;
            arrowRb.velocity = Vector3.zero;
            arrowRb.useGravity = true;
        }
    } // OnTriggerEnter

    //! �߻��� ȭ�� ȸ���ϴ� �Լ�
    private IEnumerator EnqueueArrow()
    {
        yield return new WaitForSeconds(5f);
        ArrowPool.Instance.ReturnArrow(gameObject);
    } // EnqueueArrow

    //private void SetForce(Vector3 force)
    //{
    //    GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    //}

    //Vector3 GetVelocity(Vector3 currentPos, Vector3 targetPos, float initialAngle)
    //{
    //    float gravity = Physics.gravity.magnitude;
    //    float angle = initialAngle * Mathf.Deg2Rad;

    //    Vector3 planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
    //    Vector3 planarPosition = new Vector3(currentPos.x, 0, currentPos.z);

    //    float distance = Vector3.Distance(planarTarget, planarPosition);
    //    float yOffset = currentPos.y - targetPos.y;

    //    float initialVelocity = (1 / Mathf.Cos(angle)) *
    //        Mathf.Sqrt(
    //            (0.5f * gravity * Mathf.Pow(distance, 2)) /
    //            (distance * Mathf.Tan(angle) + yOffset));

    //    Vector3 velocity = new Vector3(0f,
    //        initialVelocity * Mathf.Sin(angle),
    //        initialVelocity * Mathf.Cos(angle));

    //    float angleBetweenObjects = Vector3.Angle(Vector3.forward,
    //        planarTarget - planarPosition) * (targetPos.x > currentPos.x ? 1 : -1);
    //    Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

    //    return finalVelocity;
    //}
=======
    [SerializeField] private float speed = default;
    private ProjectilePool arrowPool = default;
    private DamageMessage damageMessage = default;
    private Rigidbody arrowRb = default;
    private IEnumerator runningCoroutine = default; // ȸ��Ÿ�̸� �ڷ�ƾ ���� ����
    private bool isHit = false; // �浹üũ ����

    private void OnEnable()
    {
        // Ȱ��ȭ�� �ʱ�ȭ
        gameObject.transform.parent = null;
        arrowRb = GetComponent<Rigidbody>();
        arrowRb.AddForce(transform.forward * speed, ForceMode.Impulse);
        // �浹������ ���� ȸ���ڷ�ƾ�� �����ϱ� ���� ĳ��
        runningCoroutine = EnqueueArrow();
        StartCoroutine(runningCoroutine);
    } // OnEnable

    private void OnDisable()
    {
        arrowRb.velocity = Vector3.zero;
        arrowRb.isKinematic = false;
        isHit = false;
    } // OnDisable

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != GData.ENEMY_MASK && other.tag != "AttackRange")
        {
            isHit = true;
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damageMessage);
            }
            arrowRb.velocity = Vector3.zero;
            // �浹������ �پ��ְ� ����� ó��
            arrowRb.isKinematic = true;
            gameObject.transform.parent = other.transform;
            StopCoroutine(runningCoroutine);
            StartCoroutine(EnqueueArrow());
        }
    } // OnTriggerEnter

    //! �������޽����� ��ü�� �޾ƿ� �Լ�
    public void InitDamageMessage(ProjectilePool _arrowPool, GameObject attacker, int damage)
    {
        damageMessage = new DamageMessage(attacker, damage);
        arrowPool = _arrowPool;
    } // InitDamageMessage

    //! �߻��� ȭ�� ȸ���ϴ� �Լ�
    private IEnumerator EnqueueArrow()
    {
        if (isHit == false)
        {
            yield return new WaitForSeconds(5f);
        }
        else
        {
            // ȭ���� �浹�ϸ� 2�ʵڿ� ȸ��
            yield return new WaitForSeconds(2f);
        }
        // pool�� ������ �ı�
        if (damageMessage.causer == null || damageMessage.causer == default)
        {
            Destroy(gameObject);
        }
        arrowPool.EnqueueProjecttile(gameObject);
    } // EnqueueArrow
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
}
