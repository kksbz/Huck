using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Monster;

public class SkeletonMage : Monster
{
    private MonsterController mController = default;
    [SerializeField] private MonsterData monsterData = default;
    [SerializeField] private GameObject SkeletonSoldierPrefab = default;
    [SerializeField] private bool useSkillA = default;
    [SerializeField] private bool useSkillB = default;
    [SerializeField] private float skillA_MaxCool = default;
    [SerializeField] private float skillB_MaxCool = default;
    private DamageMessage damageMessage = default;
    private int summonCount = default;
    private int defaultDamage = default;
    private float skillACool = 0f;
    private float skillBCool = 0f;
    public GameObject attackA_Effect;
    private void Awake()
    {
        mController = gameObject.GetComponent<MonsterController>();
        InitMonsterData(MonsterType.RANGE, monsterData);
        mController.monster = this;
        defaultDamage = damage;
        damageMessage = new DamageMessage(gameObject, damage);
        CheckUseSkill();
    } // Awake

    //! �ذ񸶹��� ���� �������̵�
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

    //! �ذ񸶹��� ��ų �������̵�
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

    //! ��밡���� ��ų�� �ִ��� üũ�ϴ� �Լ� (������Ʈ�ѷ����� �������� üũ�ϱ� ����)
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

    //! { �ذ񸶹��� �׸� region ����
    #region ���� ó��
    //! �������� ������ ó�� �Լ�
    private void AttackA()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up, 3f, Vector3.up, 0f, LayerMask.GetMask(GData.PLAYER_MASK, GData.BUILD_MASK));
        if (hits.Length > 0)
        {
            foreach (var _hit in hits)
            {
                // if : �÷��̾� �Ǵ� ���๰�� ��
                if (_hit.collider.tag == GData.PLAYER_MASK || _hit.collider.tag == GData.BUILD_MASK)
                {
                    _hit.collider.gameObject.GetComponent<IDamageable>().TakeDamage(damageMessage);
                }
            }
        }
    } // AttackA

    //! �������� ���������� ���� �����
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, 3f);
    } // OnDrawGizmos

    //! �������� �̺�Ʈ�Լ�
    private void ExitAttack()
    {
        damage = defaultDamage;
        mController.monsterAni.SetBool("isAttackA", false);
        mController.monsterAni.SetBool("isAttackB", false);
        mController.monsterAni.SetBool("isSkillA", false);
        mController.monsterAni.SetBool("isSkillB", false);
        // �������� �� ������ ����
        mController.isDelay = true;
    } // ExitAttack

    //! Ÿ���� �ٶ󺸴� �ڷ�ƾ�Լ�
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
    #endregion // ���� ó��

    #region ��ųA
    //! ��ųA �Լ�
    private void SkillA()
    {
        mController.monsterAni.SetBool("isSkillA", true);
        StartCoroutine(LookTarget());
        StartCoroutine(SkillACooldown());
    } // SkillA

    //! ��ųA ��ٿ� �ڷ�ƾ�Լ�
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
    #endregion // ��ųA

    #region ��ųB �ذ񺴻� ��ȯ
    //! ��ųB �Լ� (�ذ񺴻� ��ȯ)
    private void SkillB()
    {
        mController.monsterAni.SetBool("isSkillB", true);
        StartCoroutine(SkillBCooldown());
    } // SkillB

    //! ��ųB �ذ񺴻� ��ȯ�ϴ� �Լ�
    private void Summon()
    {
        // �ذ񸶹��� ���� 10f ������ ������ summonPos �������� Raycast���� ��ȯ��ǥ ����
        Vector3 pos = transform.position + (Vector3.up * 10f);
        Vector3 summonPos = transform.position;
        // ���׿����ڷ� targetPos ���� 5~10������ �Ÿ���ǥ�� ����
        int numberX = Random.Range(0, 2);
        summonPos.x = summonPos.x + (numberX == 0 ? Random.Range(-10, -4) : Random.Range(5, 11));
        int numberZ = Random.Range(0, 2);
        summonPos.z = summonPos.z + (numberZ == 0 ? Random.Range(-10, -4) : Random.Range(5, 11));
        Vector3 dir = (summonPos - pos).normalized;
        RaycastHit hit = default;
        if (Physics.Raycast(pos, dir, out hit, 30f, LayerMask.GetMask(GData.TERRAIN_MASK)) == true)
        {
            // �ذ񺴻簡 ��ȯ�� �� Ÿ���� �ٶ󺸸鼭 ��ȯ�ǰ� ȸ���� ����
            Vector3 dirToTarget = (mController.targetSearch.hit.transform.position - hit.point).normalized;
            Instantiate(SkeletonSoldierPrefab, hit.point, Quaternion.LookRotation(dirToTarget));
            return;
        }
        else
        {
            // ���ѷ��� ����ó�� : ��ǥŽ�� 20�� �̻��̸� ��ȯX
            //Debug.Log("��ȯ��ġ�� ��ֹ� ����! �ٸ���ǥ Ž������");
            if (summonCount > 20)
            {
                summonCount = 0;
                return;
            }
            else
            {
                summonCount += 1;
                // ��ȯ�� ��ǥŽ���� ���� ����Լ� 
                Summon();
            }
        }
    } // Summon

    //! ��ųB ��ٿ� �ڷ�ƾ�Լ�
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
    #endregion // ��ųB
    //! } �ذ񸶹��� �׸� region ����
} // SkeletonMage
