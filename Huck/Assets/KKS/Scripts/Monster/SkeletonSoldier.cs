using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
=======
using static UnityEngine.Rendering.DebugUI;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498

public class SkeletonSoldier : Monster
{
    private MonsterController mController = default;
    [SerializeField] private MonsterData monsterData = default;
    [SerializeField] private GameObject weapon = default;
    [SerializeField] private GameObject shield = default;
<<<<<<< HEAD
=======
    [SerializeField] private bool useSkillA = default;
    [SerializeField] private bool useSkillB = default;
    [SerializeField] private float skillA_MaxCool = default;
    [SerializeField] private float skillB_MaxCool = default;
    private int defaultDamage = default;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    private float skillACool = 0f;
    private float skillBCool = 0f;
    void Awake()
    {
        mController = gameObject.GetComponent<MonsterController>();
<<<<<<< HEAD
        InitMonsterData(MonsterType.MELEE, monsterData);
        mController.monster = this;
    } // Awake

    //! ���� ó�� �̺�Ʈ�Լ� (Collider)
    private void EnableWeapon()
    {
        weapon.SetActive(true);
    } // EnableWeapon

    //! ���� ó�� �̺�Ʈ�Լ� (RayCast)
    private void EnableAttack()
    {
        RaycastHit[] hits = Physics.BoxCastAll(shield.transform.position, new Vector3(1f, 1f, 0.3f) * 0.5f,
            Vector3.up, shield.transform.rotation, 0f, LayerMask.GetMask(GData.PLAYER_MASK));
        if (hits.Length > 0)
        {
            if (hits[0].collider.tag == GData.PLAYER_MASK)
            {
                Debug.Log("����转 ����!");
            }
        }
    } // EnableAttack

    //! EnableAttack() �����
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(shield.transform.position, new Vector3(1f, 1f, 0.3f));
    } // OnDrawGizmos

=======
        InitMonsterData(MonsterType.NOMAL, monsterData);
        mController.monster = this;
        defaultDamage = damage;
        CheckUseSkill();
    } // Awake

>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    //! �ذ񺴻� ���� �������̵�
    public override void Attack()
    {
        //��� 2�� �� �������� �Ѱ� ����
        int number = Random.Range(0, 10);
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);
        if (number <= 6)
        {
            mController.monsterAni.SetBool("isAttackA", true);
        }
        else
        {
            mController.monsterAni.SetBool("isAttackB", true);
        }
    } // Attack

    //! �ذ񺴻� ��ų �������̵�
    public override void Skill()
    {
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);

        if (useSkillA == true && mController.distance >= 13f)
        {
<<<<<<< HEAD
            SkillA();
            return;
        }
        else if (useSkillA == true && mController.distance > meleeAttackRange)
        {
            // ������ų�� ��밡�������� Ÿ���� �ּһ�Ÿ� �ȿ� ������ ������ųX Idle���·� ��ȯ
            StartCoroutine(CheckSkillADistance());
            IMonsterState nextState = new MonsterIdle();
            mController.MStateMachine.onChangeState?.Invoke(nextState);
            Debug.Log("���� �ּһ�Ÿ� �ȿ�����");
=======
            useSkillA = false;
            CheckUseSkill();
            SkillA();
            return;
        }
        else if (useSkillA == true && mController.distance < 13f)
        {
            useSkillA = false;
            CheckUseSkill();
            // ������ų�� ��밡�������� Ÿ���� �ּһ�Ÿ� �ȿ� ������ ������ų ���X Idle���·� �ʱ�ȭ
            StartCoroutine(CheckSkillADistance());
            IMonsterState nextState = new MonsterIdle();
            mController.MStateMachine.onChangeState?.Invoke(nextState);
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            return;
        }

        if (useSkillB == true)
        {
<<<<<<< HEAD
=======
            useSkillB = false;
            CheckUseSkill();
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            SkillB();
            return;
        }
    } // SKILL

<<<<<<< HEAD
    //! ��ųA ���� ��� �Ÿ�üũ�ϴ� �ڷ�ƾ�Լ�
    private IEnumerator CheckSkillADistance()
    {
        useSkillA = false;
        isNoRangeSkill = true;
        while (isNoRangeSkill == true)
        {
            float distance = Vector3.Distance(mController.targetSearch.hit.transform.position, mController.transform.position);
            // Ÿ���� ���� �ּһ�Ÿ� �ۿ� ������ ���� ��밡��
            if (distance >= 13f)
            {
                useSkillA = true;
                isNoRangeSkill = false;
                Debug.Log($"�Ÿ� : {distance}, ��ųA :{useSkillA}, {isNoRangeSkill}");
=======
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

    //! { �ذ񺴻� �׸� region ����
    #region ���� ó�� (Collider)
    //! ���� ���� ó�� �̺�Ʈ�Լ� (Collider)
    private void EnableWeapon()
    {
        weapon.SetActive(true);
    } // EnableWeapon

    //! ���� ���� ó�� �̺�Ʈ�Լ� (Collider)
    private void EnableShield()
    {
        shield.SetActive(true);
    } // EnableShield

    //! �������� �̺�Ʈ�Լ�
    public override void ExitAttack()
    {
        damage = defaultDamage;
        weapon.SetActive(false);
        shield.SetActive(false);
        mController.monsterAni.SetBool("isAttackA", false);
        mController.monsterAni.SetBool("isAttackB", false);
        mController.monsterAni.SetBool("isSkillA_Start", false);
        mController.monsterAni.SetBool("isSkillA_Loop", false);
        mController.monsterAni.SetBool("isSkillA_End", false);
        mController.monsterAni.SetBool("isSkillB", false);
        // �������� �� ������ ����
        mController.isDelay = true;
    } // ExitAttack
    #endregion // ���� ó�� (Collider, RayCast)

    #region ��ųA (����)
    //! ��ųA �Լ� (���� ����)
    private void SkillA()
    {
        StartCoroutine(UseSkillA());
    } // SkillA

    //! ��ųA ���� ��� �Ÿ�üũ�ϴ� �ڷ�ƾ�Լ�
    private IEnumerator CheckSkillADistance()
    {
        isNoRangeSkill = true;
        while (isNoRangeSkill == true)
        {
            // Ÿ���� ���� �ּһ�Ÿ� �ۿ� ������ ���� ��밡��
            if (mController.distance >= 13f)
            {
                useSkillA = true;
                isNoRangeSkill = false;
                CheckUseSkill();
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
                yield break;
            }
            yield return null;
        }
    } // CheckSkillADistance

<<<<<<< HEAD
    //! ��ųA �Լ�
    private void SkillA()
    {
        mController.monsterAni.SetBool("isSkillA_Start", true);
        StartCoroutine(SkillACooldown());
    } // SkillA

    //! ��ųA ���� ���� �̺�Ʈ�Լ�
    private void SkillA_Combo()
    {
        StartCoroutine(UseSkillA());
    } // SkillA_Combo

    //! ��ųB �Լ�
    private void SkillB()
    {
        mController.monsterAni.SetBool("isSkillB", true);
        StartCoroutine(SkillBCooldown());
    } // SkillB

    //! �������� �̺�Ʈ�Լ�
    private void ExitAttack()
    {
        mController.monsterAni.SetBool("isAttackA", false);
        mController.monsterAni.SetBool("isAttackB", false);
        mController.monsterAni.SetBool("isSkillA_End", false);
        mController.monsterAni.SetBool("isSkillB", false);
        weapon.SetActive(false);
        // �������� �� ������ ����
        StartCoroutine(AttackDelay(mController, 4));
    } // ExitAttack

    //! ��ųA ���� ���� �ڷ�ƾ�Լ�
    private IEnumerator UseSkillA()
    {
        // ���� �غ� ��� ������ ���� ����
        mController.monsterAni.SetBool("isSkillA_Start", false);
        mController.monsterAni.SetBool("isSkillA_Loop", true);
=======
    //! ��ųA ���� ���� �ڷ�ƾ�Լ�
    private IEnumerator UseSkillA()
    {
        // ���� ��Ÿ�� ����
        StartCoroutine(SkillACooldown());
        // ���� ���� �� �Լ� ����
        mController.monsterAni.SetTrigger("isRoar");
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(mController.monsterAni.GetCurrentAnimatorStateInfo(0).length);
        mController.monsterAni.SetBool("isSkillA_Start", true);
        // ���� �غ� ��� ������ ���� ����
        bool isStart = true;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        bool isSkillA = true;
        mController.mAgent.speed = moveSpeed * 2f;
        while (isSkillA == true)
        {
<<<<<<< HEAD
=======
            if (mController.targetSearch.hit == null)
            {
                mController.monsterAni.SetBool("isSkillA_Start", false);
                mController.monsterAni.SetBool("isSkillA_Loop", false);
                yield break;
            }
            if (mController.monsterAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && isStart == true)
            {
                // ���� ���� ��� ������ Loop������� ��ȯ
                mController.monsterAni.SetBool("isSkillA_Start", false);
                mController.monsterAni.SetBool("isSkillA_Loop", true);
                isStart = false;
            }
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            mController.mAgent.SetDestination(mController.targetSearch.hit.transform.position);
            // ���� �� Ÿ���� �������ݻ�Ÿ� ���̶�� ���� ������ ����
            if (mController.distance <= meleeAttackRange)
            {
<<<<<<< HEAD
                mController.mAgent.speed = moveSpeed;
                mController.mAgent.ResetPath();
=======
                damage = Mathf.FloorToInt(defaultDamage * 1.5f);
                mController.mAgent.speed = moveSpeed;
                mController.mAgent.ResetPath();
                mController.monsterAni.SetBool("isSkillA_Start", false);
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
                mController.monsterAni.SetBool("isSkillA_Loop", false);
                mController.monsterAni.SetBool("isSkillA_End", true);
                isSkillA = false;
            }
            yield return null;
        }
    } // UseSkillA

    //! ��ųA ��ٿ� �ڷ�ƾ�Լ�
    private IEnumerator SkillACooldown()
    {
<<<<<<< HEAD
        useSkillA = false;
        // ������Ʈ�ѷ����� �������� �� üũ�� ���� : ���Ÿ� ��ų ��밡��
        isNoRangeSkill = true;
        while (true)
        {
            skillACool += Time.deltaTime;
            if (skillACool >= skillA_MaxCool)
            {
                skillACool = 0f;
                useSkillA = true;
                isNoRangeSkill = false;
                yield break;
            }
            yield return null;
        }
    } // SkillACooldown
=======
        skillACool = 0f;
        // ������Ʈ�ѷ����� �������� �� üũ�� ���� : ���Ÿ� ��ų ��밡��
        isNoRangeSkill = true;
        while (skillACool < skillA_MaxCool)
        {
            skillACool += Time.deltaTime;
            yield return null;
        }
        skillACool = 0f;
        useSkillA = true;
        isNoRangeSkill = false;
        CheckUseSkill();
    } // SkillACooldown
    #endregion //��ųA (����)

    #region ��ųB (���� ����)
    //! ��ųB �Լ� (���� ����)
    private void SkillB()
    {
        // ��ų ������ ����
        damage = Mathf.FloorToInt(defaultDamage * 0.7f);
        mController.monsterAni.SetBool("isSkillB", true);
        StartCoroutine(SkillBCooldown());
    } // SkillB
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498

    //! ��ųB ��ٿ� �ڷ�ƾ�Լ�
    private IEnumerator SkillBCooldown()
    {
<<<<<<< HEAD
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
=======
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
    #endregion // ��ųB (���� ����)

    #region ���� ����
    private void RoarSound()
    {
        mController.monsterAudio.clip = roarClip;
        mController.monsterAudio.Play();
    } // RoarSound
    private void DeadSound()
    {
        mController.monsterAudio.clip = deadClip;
        mController.monsterAudio.Play();
    } // DeadSound
    private void MoveSound()
    {
        mController.monsterAudio.clip = moveClip;
        mController.monsterAudio.Play();
    } // MoveSound
    private void HitSound()
    {
        mController.monsterAudio.clip = hitClip;
        mController.monsterAudio.Play();
    } // HitSound
    private void WeaponSound()
    {
        mController.monsterAudio.clip = weaponClip;
        mController.monsterAudio.Play();
    } // WeaponSound
    #endregion // ���� ����
    //! } �ذ񺴻� �׸� region ����
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
} // SkeletonSoldier
