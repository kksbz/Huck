using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] ParticleSystem fireBallStart = default; // ���̾ ����Ʈ
    [SerializeField] ParticleSystem fireBallEnd = default; // ���̾���� ����Ʈ
    private DamageMessage damageMessage = default; // ������ó��
    private GameObject target = default; // Ÿ��
    private Vector3 lastTargetPos = default; // ������Ȱ��ȭ �� ���⺤�� ���� ����
    private Vector3 lastDir = default; // ������Ȱ��ȭ �� ���⺤�� ����
    private bool isEndFollow = false; // ����üũ ����
    private bool isHit = false; // �浹üũ ����
    private float speed = 5f;
    private float attackRange = 2f; // �����Ÿ� ����

    private void OnEnable()
    {
        // Ȱ��ȭ �� �ʱ�ȭ
        isHit = false;
        isEndFollow = false;
        fireBallStart.gameObject.SetActive(true);
        fireBallStart.Play();
        StartCoroutine(EnqueueFireBall());
    } // OnEnable

    private void OnDisable()
    {
        // ��Ȱ��ȭ �� Ÿ�� �ʱ�ȭ
        target = default;
        lastTargetPos = default;
        fireBallEnd.gameObject.SetActive(false);
    } // OnDisable

    // Update is called once per frame
    void Update()
    {
        MoveFireBall();
    } // Update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != GData.ENEMY_MASK)
        {
            isHit = true;
            if (other.tag == GData.PLAYER_MASK || other.tag == GData.BUILD_MASK)
            {
                other.gameObject.GetComponent<IDamageable>().TakeDamage(damageMessage);
            }
            StartCoroutine(FireBallExplosion());
        }
    } // OnTriggerEnter

    //! ���̾�� �̵� �Լ�
    private void MoveFireBall()
    {
        if (isHit == false)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            // Ÿ���� attackRange �ȿ� ������ ������ ���߰� �����ϴ� �������� �̵��ϰ� ó��
            if (distance <= attackRange && isEndFollow == false)
            {
                isEndFollow = true;
                // ���̾�� Ȱ��ȭ�� �� Ÿ���� attackRange �ȿ����� ��� ����ó��
                if (lastTargetPos == null || lastTargetPos == default)
                {
                    lastTargetPos = target.transform.position + Vector3.up;
                }
                lastDir = (lastTargetPos - transform.position).normalized;
                transform.forward = lastDir;
            }

            if (isEndFollow == false)
            {
                // Ÿ���� ����
                lastTargetPos = target.transform.position + Vector3.up;
                Vector3 dir = ((target.transform.position + Vector3.up) - transform.position).normalized;
                transform.forward = dir;
                transform.position += dir * speed * Time.deltaTime;
            }
            else
            {
                // �����ϴ� ���� �״�� �̵�
                transform.position += lastDir * speed * Time.deltaTime;
            }
        }
    } // MoveFireBall

    //! �������޽����� ��ü�� �޾ƿ� �Լ�
    public void InitDamageMessage(GameObject attacker, int damage, GameObject _target)
    {
        damageMessage = new DamageMessage(attacker, damage);
        target = _target;
    } // InitDamageMessage

    //! �߻��� ���̾ ȸ���ϴ� �Լ�
    private IEnumerator EnqueueFireBall()
    {
        yield return new WaitForSeconds(6f);
        if (gameObject.activeInHierarchy == true)
        {
            ProjectilePool.Instance.EnqueueProjecttile(gameObject);
        }
    } // EnqueueFireBall

    //! ���̾ ���� �ڷ�ƾ�Լ�
    private IEnumerator FireBallExplosion()
    {
        fireBallStart.gameObject.SetActive(false);
        fireBallEnd.gameObject.SetActive(true);
        fireBallEnd.Play();
        yield return new WaitForSeconds(fireBallEnd.main.duration + fireBallEnd.main.startLifetime.constant);
        gameObject.SetActive(false);
    } // FireBallExplosion
} // FireBall
