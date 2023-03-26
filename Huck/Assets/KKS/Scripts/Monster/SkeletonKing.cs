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

    //! �ذ�� ���� �������̵�
    public override void Attack()
    {
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);
        // ��� 5�� �� �������� �Ѱ� ����
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

    //! �ذ�� ��ų �������̵�
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
            // ���� ���� ��ų�� ��밡�������� Ÿ���� �ּһ�Ÿ� �ȿ� ������ ��ų ���X Idle���·� �ʱ�ȭ
            StartCoroutine(CheckSkillBDistance());
            CheckUseSkill();
            IMonsterState nextState = new MonsterIdle();
            mController.MStateMachine.onChangeState?.Invoke(nextState);
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

    //! { �ذ�� �׸� region ����
    #region ���� ó�� (Collider, Raycast)
    //! ���� ó�� �̺�Ʈ�Լ� (Collider)
    private void EnableWeapon()
    {
        weapon.SetActive(true);
    } // EnableWeapon

    //! �������� �̺�Ʈ�Լ�
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
        // �������� �� ������ ���·� ��ȯ
        mController.isDelay = true;
    } // ExitAttack
    #endregion // ���� ó�� (Collider, Raycast)

    #region ��ųA �ذ�׷�Ʈ ��ȯ
    //! �ذ�� ��ųA �Լ� (��ȯ ��ų)
    private void SkillA()
    {
        StartCoroutine(UseSkillA());
    } // SkillA

    //! ��ųA ���� �ڷ�ƾ�Լ�
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
                // ��ȯ �غ� ��� ������ ��ȯ ����
                mController.monsterAni.SetBool("isSkillA_Start", false);
                mController.monsterAni.SetBool("isSkillA_Loop", true);
                isStart = false;
                Summon();
            }
            // 5�ʰ� ������ ��ȯ ������ ����
            if (isStart == false && timeCheck >= 4f)
            {
                mController.monsterAni.SetBool("isSkillA_Loop", false);
                mController.monsterAni.SetBool("isSkillA_End", true);
                isSkillA = true;
            }
            yield return null;
        }
    } // UseSkillA

    //! ��ųA �ذ�׷�Ʈ ��ȯ�ϴ� �Լ�
    private void Summon()
    {
        RaycastHit hit = default;
        // �ذ�� ���� 5f ������ ������ summonPos �������� Raycast���� ��ȯ��ǥ ����
        Vector3 pos = transform.position + (Vector3.up * 5f);
        Vector3 summonPos = transform.position;
        // ���׿����ڷ� targetPos ���� 5~10������ �Ÿ���ǥ�� ����
        int numberX = Random.Range(0, 2);
        summonPos.x = summonPos.x + (numberX == 0 ? Random.Range(-10, -4) : Random.Range(5, 11));
        int numberZ = Random.Range(0, 2);
        summonPos.z = summonPos.z + (numberZ == 0 ? Random.Range(-10, -4) : Random.Range(5, 11));
        Vector3 dir = (summonPos - pos).normalized;
        if (Physics.Raycast(pos, dir, out hit, 30f, LayerMask.GetMask(GData.TERRAIN_MASK)) == true)
        {
            Vector3 dirToTarget = (mController.targetSearch.hit.transform.position - hit.point).normalized;
            // �ذ񺴻簡 ��ȯ�� �� Ÿ���� �ٶ󺸸鼭 ��ȯ�ǰ� ȸ���� ����
            Instantiate(summonObjPrefab, hit.point, Quaternion.LookRotation(dirToTarget));
            return;
        }
        else
        {
            //Debug.Log("��ȯ��ġ�� ��ֹ� ����! �ٸ���ǥ Ž������");
            // ���ѷ��� ����ó�� : ��ǥŽ�� 20�� �̻��̸� �ذ�� �տ� ��ȯ
            if (summonCount > 20)
            {
                Instantiate(summonObjPrefab, transform.position + (transform.forward * 2f), Quaternion.LookRotation(transform.forward));
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
    #endregion // ��ųA �ذ�׷�Ʈ ��ȯ

    #region ��ųB ���� ����
    //! �ذ�� ��ųB �Լ� (���� ����)
    private void SkillB()
    {
        StartCoroutine(UseSkillB());
    } // SkillB

    //! ��ųB (���� ����) �ڷ�ƾ�Լ�
    private IEnumerator UseSkillB()
    {
        mController.monsterAni.SetTrigger("isRoar");
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(mController.monsterAni.GetCurrentAnimatorStateInfo(0).length);
        StartCoroutine(SkillBCooldown());
        mController.monsterAni.SetBool("isSkillB", true);
        yield return new WaitForSeconds(0.8f);
        // ������ �̵��Լ��� ����ϱ� ���� Parabola �ʱ�ȭ
        Parabola parabola = new Parabola();
        // ���Ͱ� Ÿ���� �ٶ󺸴� ������ �ݴ������ ����
        Vector3 dir = -(mController.targetSearch.hit.transform.position - mController.transform.position).normalized;
        // ��ǥ��ġ�� dir�������� meleeAttackRange��ŭ �̵��� ��ǥ�� ����
        Vector3 targetPos = mController.targetSearch.hit.transform.position + dir * meleeAttackRange;
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);
        StartCoroutine(parabola.ParabolaMoveToTarget(mController.transform.position, targetPos, 1f, gameObject));
        yield return new WaitForSeconds(mController.monsterAni.GetCurrentAnimatorStateInfo(0).length - 0.8f);
        mController.monsterAni.SetBool("isSkillB", false);
        mController.isDelay = true;
    } // UseSkillB

    //! ��ųB (���� ����) ���Ÿ� üũ�ϴ� �ڷ�ƾ�Լ�
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

    //! ��ųB ��ٿ� �ڷ�ƾ�Լ�
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
    #endregion // ��ųB ���� ����
    //! } �ذ�� �׸� region ����
} // SkeletonKing
