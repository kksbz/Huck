using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Monster;

public class SkeletonMage : Monster
{
    private MonsterController mController = default;
    [SerializeField] private MonsterData monsterData = default;
    [SerializeField] private GameObject summonObjPrefab = default;
    [SerializeField] private bool useSkillA = default;
    [SerializeField] private bool useSkillB = default;
    [SerializeField] private float skillA_MaxCool = default;
    [SerializeField] private float skillB_MaxCool = default;
    private DamageMessage damageMessage = default;
    private GameObject attackA_Prefab = default;
    private GameObject attackB_Prefab = default;
    private GameObject skillA_Prefab = default;
    private int summonCount = default;
    private int defaultDamage = default;
    private float skillACool = 0f;
    private float skillBCool = 0f;
    private void Awake()
    {
        mController = gameObject.GetComponent<MonsterController>();
        InitMonsterData(MonsterType.NOMAL, monsterData);
        mController.monster = this;
        defaultDamage = damage;
        damageMessage = new DamageMessage(gameObject, damage);
        attackA_Prefab = Resources.Load("Prefabs/Monster/MonsterEffect/Skeleton_Mage_Effect/MageMelee") as GameObject;
        attackB_Prefab = Resources.Load("Prefabs/Monster/MonsterEffect/Skeleton_Mage_Effect/FireBall") as GameObject;
        skillA_Prefab = Resources.Load("Prefabs/Monster/MonsterEffect/Skeleton_Mage_Effect/Summon_Thorn") as GameObject;
        CheckUseSkill();
    } // Awake

    //! 해골마법사 공격 오버라이드
    public override void Attack()
    {
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);
        if (mController.distance > meleeAttackRange)
        {
            mController.monsterAni.SetBool("isAttackB", true);
            StartCoroutine(LookTarget());
        }
        else
        {
            mController.monsterAni.SetBool("isAttackA", true);
        }
    } // Attack

    //! 해골마법사 스킬 오버라이드
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

        if (useSkillB == true)
        {
            useSkillB = false;
            CheckUseSkill();
            SkillB();
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

    //! { 해골마법사 항목별 region 모음
    #region 공격 처리
    //! 근접공격 데미지 처리 함수
    private void AttackA()
    {
        StartCoroutine(OnEffectAttackA());
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up, 2.5f, Vector3.up, 0f, LayerMask.GetMask(GData.PLAYER_MASK, GData.BUILD_MASK));
        if (hits.Length > 0)
        {
            foreach (var _hit in hits)
            {
                // if : 플레이어 또는 건축물일 때
                if (_hit.collider.tag == GData.PLAYER_MASK || _hit.collider.tag == GData.BUILD_MASK)
                {
                    _hit.collider.gameObject.GetComponent<IDamageable>().TakeDamage(damageMessage);
                }
            }
        }
    } // AttackA

    //! AttackA 근접공격 이펙트 코루틴함수
    private IEnumerator OnEffectAttackA()
    {
        GameObject effectObj = Instantiate(attackA_Prefab);
        ParticleSystem effect = effectObj.GetComponent<ParticleSystem>();
        effectObj.transform.position = transform.position + Vector3.up;
        effectObj.transform.forward = transform.forward;
        effect.Play();
        yield return new WaitForSeconds(effect.main.duration + effect.main.startLifetime.constant);
        Destroy(effectObj);
    } // OnEffectAttackA

    //! 근접공격 데미지판정 범위 기즈모
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, 2.5f);
    } // OnDrawGizmos

    //! AttackB 파이어볼 쏘는 함수
    private void ShootFireBall()
    {
        GameObject fireBall = ProjectilePool.Instance.GetProjecttile();
        fireBall.GetComponent<FireBall>().InitDamageMessage(gameObject, defaultDamage, mController.targetSearch.hit.gameObject);
        fireBall.transform.position = transform.position + Vector3.up * 2f;
        fireBall.SetActive(true);
    } // ShootFireBall

    //! 공격종료 이벤트함수
    public override void ExitAttack()
    {
        damage = defaultDamage;
        mController.monsterAni.SetBool("isAttackA", false);
        mController.monsterAni.SetBool("isAttackB", false);
        mController.monsterAni.SetBool("isSkillA", false);
        mController.monsterAni.SetBool("isSkillB", false);
        // 공격종료 후 딜레이 시작
        mController.isDelay = true;
    } // ExitAttack

    //! 타겟을 바라보는 코루틴함수
    private IEnumerator LookTarget()
    {
        bool isLookAt = true;
        while (isLookAt == true)
        {
            if (mController.enumState != MonsterController.MonsterState.SKILL
                && mController.enumState != MonsterController.MonsterState.ATTACK)
            {
                isLookAt = false;
                yield break;
            }
            mController.transform.LookAt(mController.targetSearch.hit.transform.position);
            yield return null;
        }
    } // LookTarget
    #endregion // 공격 처리

    #region 스킬A 가시 소환
    //! 스킬A 함수 (가시 소환)
    private void SkillA()
    {
        mController.monsterAni.SetBool("isSkillA", true);
        StartCoroutine(LookTarget());
        StartCoroutine(SkillACooldown());
    } // SkillA

    //! 스킬A 데미지판정 이벤트함수
    private void SkillA_Damage()
    {
        StartCoroutine(OnEffectSkillA());
    } // SkillA_Damage

    //! 스킬A 이펙트 코루틴함수
    private IEnumerator OnEffectSkillA()
    {
        GameObject effectObj = Instantiate(skillA_Prefab);
        ParticleSystem effect = effectObj.GetComponent<ParticleSystem>();
        effectObj.transform.position = mController.targetSearch.hit.transform.position;
        effectObj.transform.forward = transform.forward;
        yield return new WaitForSeconds(1f);
        effect.Play();
        // 스킬A 이펙트 실행될때 데미지판정 시작
        damageMessage.damageAmount = defaultDamage * 2f;
        RaycastHit[] hits = Physics.SphereCastAll(effectObj.transform.position, 2.5f, Vector3.up, 0f, LayerMask.GetMask(GData.PLAYER_MASK, GData.BUILD_MASK));
        if (hits.Length > 0)
        {
            foreach (var _hit in hits)
            {
                // if : 플레이어 또는 건축물일 때
                if (_hit.collider.tag == GData.PLAYER_MASK || _hit.collider.tag == GData.BUILD_MASK)
                {
                    _hit.collider.gameObject.GetComponent<IDamageable>().TakeDamage(damageMessage);
                }
            }
        }
        damageMessage.damageAmount = defaultDamage;
        yield return new WaitForSeconds(effect.main.duration + effect.main.startLifetime.constant);
        Destroy(effectObj);
    } // OnEffectSkillA

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
    #endregion // 스킬A

    #region 스킬B 해골병사 소환
    //! 스킬B 함수 (해골병사 소환)
    private void SkillB()
    {
        mController.monsterAni.SetBool("isSkillB", true);
        StartCoroutine(SkillBCooldown());
    } // SkillB

    //! 스킬B 해골병사 소환하는 함수
    private void Summon()
    {
        // 해골마법사 위로 10f 떨어진 곳에서 summonPos 방향으로 Raycast쏴서 소환좌표 구함
        Vector3 pos = transform.position + (Vector3.up * 10f);
        Vector3 summonPos = transform.position;
        // 삼항연산자로 targetPos 기준 5~10사이의 거리좌표를 구함
        int numberX = Random.Range(0, 2);
        summonPos.x = summonPos.x + (numberX == 0 ? Random.Range(-10, -4) : Random.Range(5, 11));
        int numberZ = Random.Range(0, 2);
        summonPos.z = summonPos.z + (numberZ == 0 ? Random.Range(-10, -4) : Random.Range(5, 11));
        Vector3 dir = (summonPos - pos).normalized;
        RaycastHit hit = default;
        if (Physics.Raycast(pos, dir, out hit, 30f, LayerMask.GetMask(GData.TERRAIN_MASK)) == true)
        {
            // 해골병사가 소환될 때 타겟을 바라보면서 소환되게 회전축 설정
            Vector3 dirToTarget = (mController.targetSearch.hit.transform.position - hit.point).normalized;
            Instantiate(summonObjPrefab, hit.point, Quaternion.LookRotation(dirToTarget));
            return;
        }
        else
        {
            //Debug.Log("소환위치에 장애물 있음! 다른좌표 탐색시작");
            // 무한루프 예외처리 : 좌표탐색 20번 이상이면 해골마법사 앞에 소환
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

    //! 스킬B 쿨다운 코루틴함수
    private IEnumerator SkillBCooldown()
    {
        skillBCool = 0f;
        while (skillBCool < skillB_MaxCool)
        {
            skillBCool += Time.deltaTime;
            yield return null;
        }
        skillBCool = 0f;
        useSkillB = true;
        CheckUseSkill();
    } // SkillBCooldown
    #endregion // 스킬B
    //! } 해골마법사 항목별 region 모음
} // SkeletonMage
