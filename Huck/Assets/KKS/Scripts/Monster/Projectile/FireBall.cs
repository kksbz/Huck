using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] ParticleSystem fireBallStart = default; // 파이어볼 이펙트
    [SerializeField] ParticleSystem fireBallEnd = default; // 파이어볼폭발 이펙트
    private DamageMessage damageMessage = default; // 데미지처리
    private GameObject target = default; // 타겟
    private Vector3 lastTargetPos = default; // 유도비활성화 시 방향벡터 구할 변수
    private Vector3 lastDir = default; // 유도비활성화 시 방향벡터 변수
    private bool isEndFollow = false; // 유도체크 변수
    private bool isHit = false; // 충돌체크 변수
    private float speed = 5f;
    private float attackRange = 2f; // 유도거리 변수

    private void OnEnable()
    {
        // 활성화 시 초기화
        isHit = false;
        isEndFollow = false;
        fireBallStart.gameObject.SetActive(true);
        fireBallStart.Play();
        StartCoroutine(EnqueueFireBall());
    } // OnEnable

    private void OnDisable()
    {
        // 비활성화 시 타겟 초기화
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

    //! 파이어볼의 이동 함수
    private void MoveFireBall()
    {
        if (isHit == false)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            // 타겟이 attackRange 안에 있으면 유도를 멈추고 진행하던 방향으로 이동하게 처리
            if (distance <= attackRange && isEndFollow == false)
            {
                isEndFollow = true;
                // 파이어볼이 활성화될 때 타겟이 attackRange 안에있을 경우 예외처리
                if (lastTargetPos == null || lastTargetPos == default)
                {
                    lastTargetPos = target.transform.position + Vector3.up;
                }
                lastDir = (lastTargetPos - transform.position).normalized;
                transform.forward = lastDir;
            }

            if (isEndFollow == false)
            {
                // 타겟을 따라감
                lastTargetPos = target.transform.position + Vector3.up;
                Vector3 dir = ((target.transform.position + Vector3.up) - transform.position).normalized;
                transform.forward = dir;
                transform.position += dir * speed * Time.deltaTime;
            }
            else
            {
                // 진행하던 방향 그대로 이동
                transform.position += lastDir * speed * Time.deltaTime;
            }
        }
    } // MoveFireBall

    //! 데미지메시지의 주체를 받아올 함수
    public void InitDamageMessage(GameObject attacker, int damage, GameObject _target)
    {
        damageMessage = new DamageMessage(attacker, damage);
        target = _target;
    } // InitDamageMessage

    //! 발사한 파이어볼 회수하는 함수
    private IEnumerator EnqueueFireBall()
    {
        yield return new WaitForSeconds(6f);
        if (gameObject.activeInHierarchy == true)
        {
            ProjectilePool.Instance.EnqueueProjecttile(gameObject);
        }
    } // EnqueueFireBall

    //! 파이어볼 폭발 코루틴함수
    private IEnumerator FireBallExplosion()
    {
        fireBallStart.gameObject.SetActive(false);
        fireBallEnd.gameObject.SetActive(true);
        fireBallEnd.Play();
        yield return new WaitForSeconds(fireBallEnd.main.duration + fireBallEnd.main.startLifetime.constant);
        gameObject.SetActive(false);
    } // FireBallExplosion
} // FireBall
