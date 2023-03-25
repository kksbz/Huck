using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Monster;

public class SkeletonMage : Monster
{
    private MonsterController mController = default;
    [SerializeField] private MonsterData monsterData = default;
    [SerializeField] private bool useSkillA = default;
    [SerializeField] private bool useSkillB = default;
    [SerializeField] private float skillA_MaxCool = default;
    [SerializeField] private float skillB_MaxCool = default;
    private DamageMessage damageMessage = default;
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

    #region ��ųB
    private void SkillB()
    {
        mController.monsterAni.SetBool("isSkillB", true);
        StartCoroutine(SkillBCooldown());
    } // SkillB

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
