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
    [SerializeField] private bool useSkillC = default;
    [SerializeField] private bool useSkillD = default;
    [SerializeField] private float skillA_MaxCool = default;
    [SerializeField] private float skillB_MaxCool = default;
    [SerializeField] private float skillC_MaxCool = default;
    [SerializeField] private float skillD_MaxCool = default;
    [HideInInspector] public bool is2Phase = false;
    private int summonCount = default;
    private float skillACool = 0f;
    private float skillBCool = 0f;
    private float skillCCool = 0f;
    private float skillDCool = 0f;
    private float slideAttackCool = 0f;
    void Awake()
    {
        mController = gameObject.GetComponent<MonsterController>();
        InitMonsterData(MonsterType.BOSS, monsterData);
        mController.monster = this;
        CheckUseSkill();
    } // Awake

    //! 해골왕 공격 오버라이드
    public override void Attack()
    {
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);
        if (slideAttackCool == 0f && isNoRangeAttack == false && mController.distance >= 10f)
        {
            SlideAttack();
        }
        else if (slideAttackCool == 0f && isNoRangeAttack == false && mController.distance < 10f)
        {
            StartCoroutine(CheckSlideDistance());
            // 슬라이드 공격이 사용가능하지만 타겟이 최소사거리 안에 있을때 사용X Idle상태로 초기화
            IMonsterState nextState = new MonsterIdle();
            mController.MStateMachine.onChangeState?.Invoke(nextState);
            return;
        }
        // 모션 5개 중 랜덤으로 한개 실행
        if (mController.distance <= meleeAttackRange)
        {
            int number = default;
            if (is2Phase == true)
            {
                // 2페이즈에선 공격 모션 E, F 타입 추가
                number = Random.Range(0, 13);
            }
            else
            {
                number = Random.Range(0, 9);
            }

            if (number > 10)
            {
                StartCoroutine(UseAttackF());
            }
            else if (number > 8)
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
            CheckUseSkill();
            SkillA();
            return;
        }

        if (useSkillB == true && mController.distance >= 13f)
        {
            useSkillB = false;
            CheckUseSkill();
            SkillB();
            return;
        }
        else if (useSkillB == true && mController.distance < 13f)
        {
            useSkillB = false;
            CheckUseSkill();
            // 도약 공격 스킬이 사용가능하지만 타겟이 최소사거리 안에 있을때 스킬 사용X Idle상태로 초기화
            StartCoroutine(CheckSkillBDistance());
            IMonsterState nextState = new MonsterIdle();
            mController.MStateMachine.onChangeState?.Invoke(nextState);
            return;
        }

        if (useSkillC == true && mController.distance <= meleeAttackRange)
        {
            useSkillC = false;
            CheckUseSkill();
            SkillC();
            return;
        }

        if (useSkillD == true && mController.distance <= meleeAttackRange)
        {
            useSkillD = false;
            CheckUseSkill();
            SkillD();
            return;
        }
    } // Skill

    //! 사용가능한 스킬이 있는지 체크하는 함수 (몬스터컨트롤러에서 상태진입 체크하기 위함)
    private void CheckUseSkill()
    {
        if (useSkillA == false && useSkillB == false && useSkillC == false && useSkillD == false)
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
        mController.monsterAni.SetBool("isSlideAttack", false);
        mController.monsterAni.SetBool("isAttackA", false);
        mController.monsterAni.SetBool("isAttackB", false);
        mController.monsterAni.SetBool("isAttackC", false);
        mController.monsterAni.SetBool("isAttackD", false);
        mController.monsterAni.SetBool("isAttackE", false);
        mController.monsterAni.SetBool("isAttackF", false);
        mController.monsterAni.SetBool("isSkillA_End", false);
        mController.monsterAni.SetBool("isSkillB", false);
        mController.monsterAni.SetBool("isSkillC", false);
        mController.monsterAni.SetBool("isSkillD", false);
        // 공격종료 후 딜레이 상태로 전환
        mController.isDelay = true;
    } // ExitAttack

    //! 공격F타입 코루틴함수
    private IEnumerator UseAttackF()
    {
        mController.monsterAni.SetBool("isAttackF", true);
        yield return new WaitForSeconds(1f);
        float time = 0f;
        // 2페이즈 : 애니메이션 속도가 1.2배 증가한만큼 시간변경
        float maxTime = 1.5f / mController.monsterAni.speed;
        while (time < maxTime)
        {
            time += Time.deltaTime;
            mController.mAgent.Move(mController.transform.forward * moveSpeed * 0.5f * Time.deltaTime);
            yield return null;
        }
    } // UseAttackF
    #endregion // 공격 처리 (Collider, Raycast)

    #region 보스몬스터 죽음 처리
    //! 보스몬스터 죽음 처리 함수
    public override void BossDead()
    {
        StartCoroutine(Dead());
    } // BossDead

    //! 보스몬스터 페이즈별 죽음 처리 함수
    private IEnumerator Dead()
    {
        mController.monsterAni.speed = 1f;
        mController.monsterAni.SetBool("isDead", true);
        // 죽는 모션 되감기를 위한 float트리거
        mController.monsterAni.SetFloat("RewindDead", 1f);
        yield return null;
        float deadAniTime = mController.monsterAni.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(deadAniTime);
        mController.monsterAni.SetFloat("RewindDead", 0f);
        yield return new WaitForSeconds(2f);
        if (is2Phase == false)
        {
            // 1페이즈에서 죽었을 경우 부활하고 2페이즈로 전환
            mController.monsterAni.speed = 0.5f;
            mController.monsterAni.SetFloat("RewindDead", -1f);
            yield return null;
            yield return new WaitForSeconds(deadAniTime * 2f);
            mController.monsterAni.SetBool("isDead", false);
            mController.monsterAni.SetTrigger("isRoar");
            yield return new WaitForSeconds(0.1f);
            float time = 0f;
            float endTime = mController.monsterAni.GetCurrentAnimatorStateInfo(0).length - 2f;
            while (time < endTime)
            {
                // endTime 까지 scale을 1에서 1.5까지 늘림
                time += Time.deltaTime;
                mController.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f, time / endTime);
                yield return null;
            }
            yield return new WaitForSeconds(2f);
            mController.isDead = false;
            is2Phase = true;
            // 2페이즈 전환시 애니메이션 속도 1.2배로 설정
            mController.monsterAni.speed = 1.2f;
        }
        else
        {
            // 2페이즈에서 완전히 죽음
            // 밑으로 시체가 내려가게 하기위해 네비매쉬 비활성화
            mController.mAgent.enabled = false;
            // 4초에 걸쳐 총 2f만큼 밑으로 내려간 뒤에 디스트로이
            float deadTime = 0f;
            while (deadTime < 4f)
            {
                deadTime += Time.deltaTime;
                float deadSpeed = Time.deltaTime * 0.5f;
                mController.transform.position += Vector3.down * deadSpeed;
                yield return null;
            }
            Destroy(mController.gameObject);
        }
    } // Dead
    #endregion // 보스몬스터 죽음 처리

    #region 슬라이드 공격(원거리)
    //! 슬라이드 공격 함수
    private void SlideAttack()
    {
        StartCoroutine(UseSlideAttack());
    } // SlideAttack

    //! 슬라이드 공격 코루틴함수
    private IEnumerator UseSlideAttack()
    {
        StartCoroutine(SlideAttackCooldown());
        mController.monsterAni.SetBool("isSlideAttack", true);
        yield return null;
        float time = 0f;
        float speed = default;
        if (is2Phase == false)
        {
            speed = moveSpeed;
        }
        else
        {
            speed = moveSpeed * 1.2f;
        }
        while (time < mController.monsterAni.GetCurrentAnimatorStateInfo(0).length)
        {
            time += Time.deltaTime;
            mController.mAgent.Move(mController.transform.forward * speed * Time.deltaTime);
            yield return null;
        }
    } // UseSlideAttack

    //! 슬라이드 공격 사용 거리 체크하는 코루틴함수
    private IEnumerator CheckSlideDistance()
    {
        isNoRangeAttack = true;
        while (isNoRangeAttack == true)
        {
            // 타겟이 슬라이드 최소사거리 밖에 있으면 슬라이드 공격 사용가능
            if (mController.distance >= 10f)
            {
                isNoRangeAttack = false;
                yield break;
            }
            yield return null;
        }
    } // CheckSlideDistance
    private IEnumerator SlideAttackCooldown()
    {
        slideAttackCool = 0f;
        isNoRangeAttack = true;
        while (slideAttackCool < 20f)
        {
            slideAttackCool += Time.deltaTime;
            yield return null;
        }
        slideAttackCool = 0f;
        isNoRangeAttack = false;
    } // SlideAttackCooldown
    #endregion // 슬라이드 공격(원거리)

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
        yield return null;
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
        float waitTime = default;
        float leapTime = default;
        if (is2Phase == true)
        {
            // 2페이즈 : 애니메이션 속도가 1.2배 증가한만큼 시간변경
            waitTime = 0.8f / mController.monsterAni.speed;
            leapTime = 1f / mController.monsterAni.speed;
        }
        else
        {
            waitTime = 0.8f;
            leapTime = 1f;
        }
        yield return new WaitForSeconds(waitTime);
        // 포물선 이동함수를 사용하기 위한 Parabola 초기화
        Parabola parabola = new Parabola();
        // 몬스터가 타겟을 바라보는 방향의 반대방향을 구함
        Vector3 dir = -(mController.targetSearch.hit.transform.position - mController.transform.position).normalized;
        // 목표위치를 dir방향으로 meleeAttackRange만큼 이동된 좌표로 설정
        Vector3 targetPos = mController.targetSearch.hit.transform.position + dir * meleeAttackRange;
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);
        StartCoroutine(parabola.ParabolaMoveToTarget(mController.transform.position, targetPos, leapTime, gameObject));
        yield return new WaitForSeconds(mController.monsterAni.GetCurrentAnimatorStateInfo(0).length - waitTime);
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

    #region 스킬C 연속 베기
    //! 해골왕 스킬C 함수 (연속 베기)
    private void SkillC()
    {
        StartCoroutine(UseSkillC());
    } // SkillC

    //! 스킬C (연속 베기) 코루틴함수
    private IEnumerator UseSkillC()
    {
        StartCoroutine(SkillCCooldown());
        mController.monsterAni.SetBool("isSkillC", true);
        yield return null;
        float time = 0f;
        while (time < mController.monsterAni.GetCurrentAnimatorStateInfo(0).length)
        {
            time += Time.deltaTime;
            mController.transform.LookAt(mController.targetSearch.hit.transform.position);
            yield return null;
        }
    } // UseSkillC

    //! 스킬C 쿨다운 코루틴함수
    private IEnumerator SkillCCooldown()
    {
        skillCCool = 0f;
        while (skillCCool < skillC_MaxCool)
        {
            skillCCool += Time.deltaTime;
            yield return null;
        }
        skillCCool = 0f;
        useSkillC = true;
        CheckUseSkill();
    } // SkillCCooldown
    #endregion // 스킬C 연속 베기

    #region 스킬D 타겟유도 연속 베기
    //! 해골왕 스킬D 함수 (타겟유도 연속 베기)
    private void SkillD()
    {
        StartCoroutine(UseSkillD());
    } // SkillD

    //! 스킬D 코루틴함수
    private IEnumerator UseSkillD()
    {
        StartCoroutine(SkillDCooldown());
        mController.monsterAni.SetBool("isSkillD", true);
        yield return null;
        float time = 0f;
        if (is2Phase == false)
        {
            mController.mAgent.speed = 1f;
        }
        else
        {
            mController.mAgent.speed = 2f;
        }
        while (time < mController.monsterAni.GetCurrentAnimatorStateInfo(0).length)
        {
            time += Time.deltaTime;
            mController.mAgent.SetDestination(mController.targetSearch.hit.transform.position);
            yield return null;
        }
        mController.mAgent.speed = moveSpeed;
        mController.mAgent.ResetPath();
    } // UseSkillD

    //! 스킬D 쿨다운 코루틴함수
    private IEnumerator SkillDCooldown()
    {
        skillDCool = 0f;
        while (skillDCool < skillD_MaxCool)
        {
            skillDCool += Time.deltaTime;
            yield return null;
        }
        skillDCool = 0f;
        useSkillD = true;
        CheckUseSkill();
    } // SkillDCooldown
    #endregion // 스킬D 타겟유도 연속 베기
    //! } 해골왕 항목별 region 모음
} // SkeletonKing
