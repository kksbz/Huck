using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGrunt : Monster
{
    private MonsterController mController = default;
    [SerializeField] private MonsterData monsterData = default;
    [SerializeField] private GameObject weapon = default;
    [SerializeField] private GameObject shoulder = default;
    private float skillACool = 0f;
    private float skillBCool = 0f;
    private float rushCool = 0f;
    void Awake()
    {
        mController = gameObject.GetComponent<MonsterController>();
        InitMonsterData(MonsterType.MELEE, monsterData);
        mController.monster = this;
    } // Awake

    //! 무기 공격 처리 이벤트함수 (Collider)
    private void EnableWeapon()
    {
        weapon.SetActive(true);
    } // EnableWeapon

    //! 어깨 공격 처리 이벤트함수 (Collider)
    private void EnableShoulderAttack()
    {
        shoulder.SetActive(true);
    } // EnableShoulderAttack

    //! 해골그런트 공격 오버라이드
    public override void Attack()
    {
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);
        if (mController.distance >= 13f)
        {
            StartCoroutine(RushAttack());
            return;
        }
        else if (mController.distance > meleeAttackRange)
        {
            // 돌진공격이 사용가능하지만 타겟이 최소사거리 안에 있을때 돌진공격X Idle상태로 초기화
            StartCoroutine(CheckRushDistance());
            IMonsterState nextState = new MonsterIdle();
            mController.MStateMachine.onChangeState?.Invoke(nextState);
            return;
        }

        if (mController.distance <= meleeAttackRange)
        {
            int number = Random.Range(0, 10);
            if (number > 7)
            {
                mController.monsterAni.SetBool("isAttackC", true);
                return;
            }
            else if (number > 4)
            {
                mController.monsterAni.SetBool("isAttackB", true);
                return;
            }
            else
            {
                mController.monsterAni.SetBool("isAttackA", true);
            }
        }
    } // Attack

    //! 해골그런트 스킬 오버라이드
    public override void Skill()
    {
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);
        if (useSkillA == true)
        {
            SkillA();
            return;
        }

        if (useSkillB == true)
        {
            SkillB();
            return;
        }
    } // Skill

    //! 해골그런트 스킬A 함수
    private void SkillA()
    {
        Parabola parabola = new Parabola();
        float distance = 1f;
        // 몬스터가 타겟을 바라보는 방향의 반대방향을 구함
        Vector3 dir = -(mController.targetSearch.hit.transform.position - mController.transform.position).normalized;
        // 목표위치를 타겟위치에서 몬스터방향으로 distance만큼 이동된 좌표로 설정
        Vector3 targetPos = mController.targetSearch.hit.transform.position + dir * distance;
        StartCoroutine(parabola.ParabolaMoveToTarget(mController.transform.position, targetPos, 1.5f, gameObject));
        mController.monsterAni.SetBool("isSkillA", true);
        StartCoroutine(SkillACooldown());
    } // SkillA

    //! SkillA 사용 중 특정 구간에서 애니메이션 멈추는 이벤트함수
    private void StopSkillA_Ani()
    {
        mController.monsterAni.StartPlayback();
        StartCoroutine(PlaySkillA());
    } // StopSkillA_Ani
    //! SkillA 멈췄던 애니메이션 재생하는 코루틴함수
    private IEnumerator PlaySkillA()
    {
        yield return new WaitForSeconds(0.5f);
        mController.monsterAni.StopPlayback();
    } // PlaySkillA

    //! 스킬A 데미지판정 함수
    private void SkillA_Damage()
    {
        RaycastHit[] hits = Physics.SphereCastAll(weapon.transform.position, 3f, Vector3.up, 0f, LayerMask.GetMask(GData.PLAYER_MASK, GData.BUILD_MASK));
        if (hits.Length > 0)
        {
            foreach (var _hit in hits)
            {
                Debug.Log($"{_hit.collider.name} 맞춤!");
            }
        }
    } // SkillA_Damage

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weapon.transform.position, 3f);
    } // OnDrawGizmos

    //! 해골그런트 스킬B 함수
    private void SkillB()
    {
        mController.monsterAni.SetBool("isSkillB_Start", true);
        StartCoroutine(SkillBCooldown());
    } // SkillB

    //! 공격종료 이벤트함수
    private void ExitAttack()
    {
        weapon.SetActive(false);
        shoulder.SetActive(false);
        mController.monsterAni.SetBool("isAttackA", false);
        mController.monsterAni.SetBool("isAttackB", false);
        mController.monsterAni.SetBool("isAttackC", false);
        mController.monsterAni.SetBool("isRushAttack", false);
        mController.monsterAni.SetBool("isSkillA", false);
        mController.monsterAni.SetBool("isSkillB_End", false);
        // 공격종료 후 딜레이 시작
        mController.isDelay = true;
    } // ExitAttack

    //! 돌진 공격 사용 거리 체크하는 코루틴함수
    private IEnumerator CheckRushDistance()
    {
        isNoRangeAttack = true;
        while (isNoRangeAttack == true)
        {
            float distance = Vector3.Distance(mController.targetSearch.hit.transform.position, mController.transform.position);
            // 타겟이 돌진 최소사거리 밖에 있으면 돌진 사용가능
            if (distance >= 13f)
            {
                Debug.Log($"거리 : {distance}");
                isNoRangeAttack = false;
                yield break;
            }
            yield return null;
        }
    } // CheckRushDistance

    //! 돌진 공격 코루틴 함수
    private IEnumerator RushAttack()
    {
        StartCoroutine(RushCooldown());
        mController.monsterAni.SetBool("isRun", true);
        bool isRush = true;
        bool isFinishRush = false;
        float timeCheck = 0f;
        mController.mAgent.speed = moveSpeed * 2f;
        while (isRush == true)
        {
            // 돌진 마무리 공격 시작 전까지 타겟을 향하여 돌진
            if (isFinishRush == false)
            {
                mController.mAgent.SetDestination(mController.targetSearch.hit.transform.position);
            }
            else
            {
                // 돌진 마무리 공격 시작되면 돌진하던 방향 그대로 1초간 돌진
                timeCheck += Time.deltaTime;
                mController.mAgent.Move(mController.transform.forward * moveSpeed * Time.deltaTime);
                if (timeCheck >= 1f)
                {
                    isRush = false;
                }
            }
            // 돌진 마무리 공격 시작
            if (mController.distance <= meleeAttackRange && isFinishRush == false)
            {
                mController.mAgent.speed = moveSpeed;
                mController.mAgent.ResetPath();
                mController.monsterAni.SetBool("isRun", false);
                mController.monsterAni.SetBool("isRushAttack", true);
                isFinishRush = true;
            }
            yield return null;
        }
    } // RushAttack

    //! 스킬A 연계 공격 코루틴함수
    private IEnumerator UseSkillB()
    {
        mController.monsterAni.SetBool("isSkillB_Start", false);
        mController.monsterAni.SetBool("isSkillB_Loop", true);
        yield return new WaitForSeconds(2f);
        mController.monsterAni.SetBool("isSkillB_Loop", false);
        mController.monsterAni.SetBool("isSkillB_End", true);
    } // UseSkillB

    //! 돌진 쿨다운 코루틴함수
    private IEnumerator RushCooldown()
    {
        // 몬스터컨트롤러에서 상태진입 시 체크할 조건 : 원거리 스킬 쿨 적용
        isNoRangeAttack = true;
        while (isNoRangeAttack == true)
        {
            rushCool += Time.deltaTime;
            if (rushCool >= 20f)
            {
                rushCool = 0f;
                isNoRangeAttack = false;
                yield break;
            }
            yield return null;
        }
    } // RushCooldown

    //! 스킬A 쿨다운 코루틴함수
    private IEnumerator SkillACooldown()
    {
        useSkillA = false;
        // 몬스터컨트롤러에서 상태진입 시 체크할 조건 : 원거리 스킬 쿨 적용
        while (true)
        {
            skillACool += Time.deltaTime;
            if (skillACool >= skillA_MaxCool)
            {
                skillACool = 0f;
                useSkillA = true;
                yield break;
            }
            yield return null;
        }
    } // SkillACooldown

    //! 스킬B 쿨다운 코루틴함수
    private IEnumerator SkillBCooldown()
    {
        useSkillB = false;
        while (true)
        {
            skillBCool += Time.deltaTime;
            if (skillBCool >= skillB_MaxCool)
            {
                skillBCool = 0f;
                useSkillB = true;
                yield break;
            }
            yield return null;
        }
    } // SkillBCooldown
} // SkeletonGrunt
