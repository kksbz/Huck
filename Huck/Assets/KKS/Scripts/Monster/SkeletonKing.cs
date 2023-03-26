using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKing : Monster
{
    private MonsterController mController = default;
    [SerializeField] private MonsterData monsterData = default;
    [SerializeField] private GameObject weapon = default;
    [SerializeField] private GameObject summonObjPrefab = default;
    [SerializeField] private bool useSkillA = default;
    [SerializeField] private bool useSkillB = default;
    [SerializeField] private float skillA_MaxCool = default;
    [SerializeField] private float skillB_MaxCool = default;
    private int summonCount = default;
    private float skillACool = 0f;
    private float skillBCool = 0f;
    void Awake()
    {
        mController = gameObject.GetComponent<MonsterController>();
        InitMonsterData(MonsterType.MELEE, monsterData);
        mController.monster = this;
        CheckUseSkill();
    } // Awake

    //! 해골왕 공격 오버라이드
    public override void Attack()
    {
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);
        // 모션 5개 중 랜덤으로 한개 실행
        if (mController.distance <= meleeAttackRange)
        {
            int number = Random.Range(0, 11);
            if (number > 8)
            {
                mController.monsterAni.SetBool("isAttackE", true);
            }
            else if (number > 6)
            {
                mController.monsterAni.SetBool("isAttackD", true);
            }
            else if (number > 4)
            {
                mController.monsterAni.SetBool("isAttackC", true);
                return;
            }
            else if (number > 2)
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
        if (useSkillB == true && mController.distance >= 13f)
        {
            useSkillB = false;
            SkillB();
            CheckUseSkill();
            return;
        }
        else if (useSkillB == true && mController.distance < 13f)
        {
            useSkillB = false;
            // 도약 공격 스킬이 사용가능하지만 타겟이 최소사거리 안에 있을때 스킬 사용X Idle상태로 초기화
            StartCoroutine(CheckSkillBDistance());
            CheckUseSkill();
            IMonsterState nextState = new MonsterIdle();
            mController.MStateMachine.onChangeState?.Invoke(nextState);
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

    //! { 해골왕 항목별 region 모음
    #region 공격 처리 (Collider, Raycast)
    //! 공격 처리 이벤트함수 (Collider)
    private void EnableWeapon()
    {
        weapon.SetActive(true);
    } // EnableWeapon

    //! 공격종료 이벤트함수
    public override void ExitAttack()
    {
        weapon.SetActive(false);
        mController.monsterAni.SetBool("isAttackA", false);
        mController.monsterAni.SetBool("isAttackB", false);
        mController.monsterAni.SetBool("isAttackC", false);
        mController.monsterAni.SetBool("isAttackD", false);
        mController.monsterAni.SetBool("isAttackE", false);
        mController.monsterAni.SetBool("isSkillA_End", false);
        mController.monsterAni.SetBool("isSkillB", false);
        // 공격종료 후 딜레이 상태로 전환
        mController.isDelay = true;
    } // ExitAttack
    #endregion // 공격 처리 (Collider, Raycast)

    #region 스킬A 해골그런트 소환
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
                Summon();
            }
            // 5초가 지나면 소환 마무리 시작
            if (isStart == false && timeCheck >= 4f)
            {
                mController.monsterAni.SetBool("isSkillA_Loop", false);
                mController.monsterAni.SetBool("isSkillA_End", true);
                isSkillA = true;
            }
            yield return null;
        }
    } // UseSkillA

    //! 스킬A 해골그런트 소환하는 함수
    private void Summon()
    {
        RaycastHit hit = default;
        // 해골왕 위로 5f 떨어진 곳에서 summonPos 방향으로 Raycast쏴서 소환좌표 구함
        Vector3 pos = transform.position + (Vector3.up * 5f);
        Vector3 summonPos = transform.position;
        // 삼항연산자로 targetPos 기준 5~10사이의 거리좌표를 구함
        int numberX = Random.Range(0, 2);
        summonPos.x = summonPos.x + (numberX == 0 ? Random.Range(-10, -4) : Random.Range(5, 11));
        int numberZ = Random.Range(0, 2);
        summonPos.z = summonPos.z + (numberZ == 0 ? Random.Range(-10, -4) : Random.Range(5, 11));
        Vector3 dir = (summonPos - pos).normalized;
        if (Physics.Raycast(pos, dir, out hit, 30f, LayerMask.GetMask(GData.TERRAIN_MASK)) == true)
        {
            Vector3 dirToTarget = (mController.targetSearch.hit.transform.position - hit.point).normalized;
            // 해골병사가 소환될 때 타겟을 바라보면서 소환되게 회전축 설정
            Instantiate(summonObjPrefab, hit.point, Quaternion.LookRotation(dirToTarget));
            return;
        }
        else
        {
            //Debug.Log("소환위치에 장애물 있음! 다른좌표 탐색시작");
            // 무한루프 예외처리 : 좌표탐색 20번 이상이면 해골왕 앞에 소환
            if (summonCount > 20)
            {
                Instantiate(summonObjPrefab, transform.position + (transform.forward * 2f), Quaternion.LookRotation(transform.forward));
                summonCount = 0;
                return;
            }
            else
            {
                summonCount += 1;
                // 소환할 좌표탐색을 위한 재귀함수 
                Summon();
            }
        }
    } // Summon

    //! 스킬A 쿨다운 코루틴함수
    private IEnumerator SkillACooldown()
    {
        skillACool = 0f;
        while (skillACool < skillA_MaxCool)
        {
            skillACool += Time.deltaTime;
            yield return null;
        }
        skillACool = 0f;
        useSkillA = true;
        CheckUseSkill();
    } // SkillACooldown
    #endregion // 스킬A 해골그런트 소환

    #region 스킬B 도약 공격
    //! 해골왕 스킬B 함수 (도약 공격)
    private void SkillB()
    {
        StartCoroutine(UseSkillB());
    } // SkillB

    //! 스킬B (도약 공격) 코루틴함수
    private IEnumerator UseSkillB()
    {
        mController.monsterAni.SetTrigger("isRoar");
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(mController.monsterAni.GetCurrentAnimatorStateInfo(0).length);
        StartCoroutine(SkillBCooldown());
        mController.monsterAni.SetBool("isSkillB", true);
        yield return new WaitForSeconds(0.8f);
        // 포물선 이동함수를 사용하기 위한 Parabola 초기화
        Parabola parabola = new Parabola();
        // 몬스터가 타겟을 바라보는 방향의 반대방향을 구함
        Vector3 dir = -(mController.targetSearch.hit.transform.position - mController.transform.position).normalized;
        // 목표위치를 dir방향으로 meleeAttackRange만큼 이동된 좌표로 설정
        Vector3 targetPos = mController.targetSearch.hit.transform.position + dir * meleeAttackRange;
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);
        StartCoroutine(parabola.ParabolaMoveToTarget(mController.transform.position, targetPos, 1f, gameObject));
        yield return new WaitForSeconds(mController.monsterAni.GetCurrentAnimatorStateInfo(0).length - 0.8f);
        mController.monsterAni.SetBool("isSkillB", false);
        mController.isDelay = true;
    } // UseSkillB

    //! 스킬B (도약 공격) 사용거리 체크하는 코루틴함수
    private IEnumerator CheckSkillBDistance()
    {
        isNoRangeSkill = true;
        while (mController.distance < 13f)
        {
            yield return null;
        }
        useSkillB = true;
        isNoRangeSkill = false;
        CheckUseSkill();
    } // CheckSkillBDistance

    //! 스킬B 쿨다운 코루틴함수
    private IEnumerator SkillBCooldown()
    {
        isNoRangeSkill = true;
        skillBCool = 0f;
        while (skillBCool < skillB_MaxCool)
        {
            skillBCool += Time.deltaTime;
            yield return null;
        }
        skillBCool = 0f;
        isNoRangeSkill = false;
        useSkillB = true;
        CheckUseSkill();
    } // SkillBCooldown
    #endregion // 스킬B 도약 공격
    //! } 해골왕 항목별 region 모음
} // SkeletonKing
