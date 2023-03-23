using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKing : Monster
{
    private MonsterController mController = default;
    [SerializeField] private MonsterData monsterData = default;
    [SerializeField] private GameObject weapon = default;
    [SerializeField] private bool useSkillA = default;
    [SerializeField] private bool useSkillB = default;
    [SerializeField] private float skillA_MaxCool = default;
    [SerializeField] private float skillB_MaxCool = default;
    private float skillACool = 0f;
    private float skillBCool = 0f;
    void Awake()
    {
        mController = gameObject.GetComponent<MonsterController>();
        InitMonsterData(MonsterType.MELEE, monsterData);
        mController.monster = this;
    } // Awake

    //! 공격 처리 이벤트함수 (Collider)
    private void EnableWeapon()
    {
        weapon.SetActive(true);
    } // EnableWeapon

    //! 해골왕 공격 오버라이드
    public override void Attack()
    {
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);
        // 모션 2개 중 랜덤으로 한개 실행
        int number = Random.Range(0, 10);
        if (number <= 6)
        {
            mController.monsterAni.SetBool("isAttackA", true);
        }
        else
        {
            mController.monsterAni.SetBool("isAttackB", true);
        }
    } // Attack

    //! 해골왕 스킬 오버라이드
    public override void Skill()
    {
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);
        if (useSkillA == true)
        {
            useSkillA = false;
            SkillA();
            CheckUseSkill();
            return;
        }
        if (useSkillB == true)
        {
            useSkillB = false;
            SkillB();
            CheckUseSkill();
            return;
        }
    } // Skill

    //! 사용가능한 스킬이 있는지 체크하는 함수 (몬스터컨트롤러에서 상태진입 체크하기 위함)
    private void CheckUseSkill()
    {
        if (useSkillA == false && useSkillB == false)
        {
            useSkill = false;
        }
        else
        {
            useSkill = true;
        }
    } // CheckUseSkill

    //! 해골왕 스킬A 함수 (소환 스킬)
    private void SkillA()
    {
        StartCoroutine(UseSkillA());
    } // SkillA

    //! 스킬A 공격 코루틴함수
    private IEnumerator UseSkillA()
    {
        StartCoroutine(SkillACooldown());
        mController.monsterAni.SetBool("isSkillA_Start", true);
        bool isStart = true;
        bool isSkillA = false;
        float timeCheck = 0f;
        while (isSkillA == false)
        {
            timeCheck += Time.deltaTime;
            if (mController.monsterAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && isStart == true)
            {
                // 소환 준비 모션 끝나면 소환 시작
                mController.monsterAni.SetBool("isSkillA_Start", false);
                mController.monsterAni.SetBool("isSkillA_Loop", true);
                isStart = false;
            }
            // 5초가 지나면 소환 마무리 시작
            if (isStart == false && timeCheck >= 5f)
            {
                mController.monsterAni.SetBool("isSkillA_Loop", false);
                mController.monsterAni.SetBool("isSkillA_End", true);
                isSkillA = true;
            }
            yield return null;
        }
    } // UseSkillA

    //! 해골왕 스킬B 함수 (도약 공격)
    private void SkillB()
    {
        // 포물선 이동함수를 사용하기 위한 Parabola 초기화
        Parabola parabola = new Parabola();
        // 몬스터가 타겟을 바라보는 방향의 반대방향을 구함
        Vector3 dir = -(mController.targetSearch.hit.transform.position - mController.transform.position).normalized;
        // 목표위치를 dir방향으로 meleeAttackRange만큼 이동된 좌표로 설정
        Vector3 targetPos = mController.targetSearch.hit.transform.position + dir * 1f;
        StartCoroutine(parabola.ParabolaMoveToTarget(mController.transform.position, targetPos, 1.5f, gameObject));
        mController.monsterAni.SetBool("isSkillB", true);
        StartCoroutine(SkillBCooldown());
    } // SkillB

    //! SkillB 사용 중 특정 구간에서 애니메이션 멈추는 이벤트함수
    private void StopSkillB_Ani()
    {
        StartCoroutine(PlaySkillB_Ani());
    } // StopSkillA_Ani
    //! SkillB 멈췄던 애니메이션 재생하는 코루틴함수
    private IEnumerator PlaySkillB_Ani()
    {
        mController.monsterAni.StartPlayback();
        yield return new WaitForSeconds(0.7f);
        mController.monsterAni.StopPlayback();
        yield return new WaitForSeconds(0.7f);
        mController.monsterAni.SetBool("isSkillB", false);
        float time = 0f;
        // 도약 공격 착지 후 2초간 Idle모션으로 타겟을 바라보게 함
        while(time <= 2f)
        {
            time += Time.deltaTime;
            Vector3 dir = (mController.targetSearch.hit.transform.position - mController.transform.position).normalized;
            mController.transform.rotation = Quaternion.Lerp(mController.transform.rotation, Quaternion.LookRotation(dir), 2f * Time.deltaTime);
            yield return null;
        }
        mController.isDelay = true;
    } // StopSkillB_Ani

    //! 공격종료 이벤트함수
    private void ExitAttack()
    {
        mController.monsterAni.SetBool("isAttackA", false);
        mController.monsterAni.SetBool("isAttackB", false);
        mController.monsterAni.SetBool("isSkillA_End", false);
        mController.monsterAni.SetBool("isSkillB", false);
        // 공격종료 후 딜레이 시작
        mController.isDelay = true;
    } // ExitAttack

    //! 스킬A 쿨다운 코루틴함수
    private IEnumerator SkillACooldown()
    {
        // 몬스터컨트롤러에서 상태진입 시 체크할 조건 : 원거리 스킬 쿨 적용
        while (true)
        {
            skillACool += Time.deltaTime;
            if (skillACool >= skillA_MaxCool)
            {
                skillACool = 0f;
                useSkillA = true;
                CheckUseSkill();
                yield break;
            }
            yield return null;
        }
    } // SkillACooldown

    //! 스킬B 쿨다운 코루틴함수
    private IEnumerator SkillBCooldown()
    {
        // 몬스터컨트롤러에서 상태진입 시 체크할 조건 : 원거리 스킬 쿨 적용
        while (true)
        {
            skillBCool += Time.deltaTime;
            if (skillBCool >= skillB_MaxCool)
            {
                skillBCool = 0f;
                useSkillB = true;
                CheckUseSkill();
                yield break;
            }
            yield return null;
        }
    } // SkillBCooldown
} // SkeletonKing
