using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonArcher : Monster
{
    private MonsterController mController = default;
    [SerializeField] private GameObject weapon = default;
    [SerializeField] private Transform arrowPos = default;
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private bool useSkillA = default;
    [SerializeField] private float skillA_MaxCool = default;
    private int defaultDamage = default;
    private float skillACool = 0f;
    private bool isAttackDelay = false;
    private void Awake()
    {
        mController = gameObject.GetComponent<MonsterController>();
        InitMonsterData(MonsterType.RANGE, monsterData);
        mController.monster = this;
        defaultDamage = damage;
        CheckUseSkill();
    } // Awake

    //! �ذ�ü� ���� �������̵�
    public override void Attack()
    {
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);
        if (mController.distance > meleeAttackRange)
        {
            mController.monsterAni.SetBool("isAttackB", true);
            StartCoroutine(LookAtTarget());
        }
        else
        {
            mController.monsterAni.SetBool("isAttackA", true);
        }
    } // Attack

    //! �ذ�ü� ��ų �������̵�
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
    } // Skill

    //! ��밡���� ��ų�� �ִ��� üũ�ϴ� �Լ� (������Ʈ�ѷ����� �������� üũ�ϱ� ����)
    private void CheckUseSkill()
    {
        if (useSkillA == false)
        {
            useSkill = false;
        }
        else
        {
            useSkill = true;
        }
    } // CheckUseSkill

    //! { �ذ�ü� �׸� region ����
    #region ���� ó�� (Collider)
    //! ���� ó�� �̺�Ʈ�Լ� (Collider)
    private void EnableWeapon()
    {
        weapon.SetActive(true);
    } // EnableWeapon

    //! ȭ�� ��� �Լ�
    private void ShootArrow()
    {
        GameObject arrow = ArrowPool.Instance.GetArrow();
        arrow.transform.position = arrowPos.position;
        Vector3 dir = ((mController.targetSearch.hit.transform.position + Vector3.up) - arrow.transform.position).normalized;
        arrow.transform.forward = dir;
        arrow.SetActive(true);
    } // ShootArrow

    //! �������� �̺�Ʈ�Լ�
    private void ExitAttack()
    {
        damage = defaultDamage;
        weapon.SetActive(false);
        mController.monsterAni.SetBool("isAttackA", false);
        mController.monsterAni.SetBool("isAttackB", false);
        mController.monsterAni.SetBool("isSkillA", false);
        // �������� �� ������ ����
        mController.isDelay = true;
    } // ExitAttack
    #endregion // ���� ó�� (Collider, RayCast)

    #region ��ųA (��� ���)
    //! ��ųA �Լ�
    private void SkillA()
    {
        mController.monsterAni.SetBool("isSkillA", true);
        StartCoroutine(LookAtTarget());
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
    #endregion // ��ųA (��� ���)

    #region Ÿ�� ����
    //! Ÿ���� �ٶ󺸴� �ڷ�ƾ�Լ�
    private IEnumerator LookAtTarget()
    {
        isAttackDelay = false;
        while (isAttackDelay == false)
        {
            mController.transform.LookAt(mController.targetSearch.hit.transform.position);
            yield return null;
        }
    } // LookTarget

    //! Ÿ�� �ٶ󺸱� �����ϴ� �̺�Ʈ�Լ�
    private void OffLookAtTarget()
    {
        isAttackDelay = true;
    } // OffLookAtTarget
    #endregion // Ÿ�� ����
    //! } �ذ�ü� �׸� region ����
} // SkeletonArcher
