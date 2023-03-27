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

    //! �ذ�� ���� �������̵�
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
            // �����̵� ������ ��밡�������� Ÿ���� �ּһ�Ÿ� �ȿ� ������ ���X Idle���·� �ʱ�ȭ
            IMonsterState nextState = new MonsterIdle();
            mController.MStateMachine.onChangeState?.Invoke(nextState);
            return;
        }
        // ��� 5�� �� �������� �Ѱ� ����
        if (mController.distance <= meleeAttackRange)
        {
            int number = default;
            if (is2Phase == true)
            {
                // 2������� ���� ��� E, F Ÿ�� �߰�
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

    //! �ذ�� ��ų �������̵�
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
            // ���� ���� ��ų�� ��밡�������� Ÿ���� �ּһ�Ÿ� �ȿ� ������ ��ų ���X Idle���·� �ʱ�ȭ
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

    //! ��밡���� ��ų�� �ִ��� üũ�ϴ� �Լ� (������Ʈ�ѷ����� �������� üũ�ϱ� ����)
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
        // �������� �� ������ ���·� ��ȯ
        mController.isDelay = true;
    } // ExitAttack

    //! ����FŸ�� �ڷ�ƾ�Լ�
    private IEnumerator UseAttackF()
    {
        mController.monsterAni.SetBool("isAttackF", true);
        yield return new WaitForSeconds(1f);
        float time = 0f;
        // 2������ : �ִϸ��̼� �ӵ��� 1.2�� �����Ѹ�ŭ �ð�����
        float maxTime = 1.5f / mController.monsterAni.speed;
        while (time < maxTime)
        {
            time += Time.deltaTime;
            mController.mAgent.Move(mController.transform.forward * moveSpeed * 0.5f * Time.deltaTime);
            yield return null;
        }
    } // UseAttackF
    #endregion // ���� ó�� (Collider, Raycast)

    #region �������� ���� ó��
    //! �������� ���� ó�� �Լ�
    public override void BossDead()
    {
        StartCoroutine(Dead());
    } // BossDead

    //! �������� ����� ���� ó�� �Լ�
    private IEnumerator Dead()
    {
        mController.monsterAni.speed = 1f;
        mController.monsterAni.SetBool("isDead", true);
        // �״� ��� �ǰ��⸦ ���� floatƮ����
        mController.monsterAni.SetFloat("RewindDead", 1f);
        yield return null;
        float deadAniTime = mController.monsterAni.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(deadAniTime);
        mController.monsterAni.SetFloat("RewindDead", 0f);
        yield return new WaitForSeconds(2f);
        if (is2Phase == false)
        {
            // 1������� �׾��� ��� ��Ȱ�ϰ� 2������� ��ȯ
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
                // endTime ���� scale�� 1���� 1.5���� �ø�
                time += Time.deltaTime;
                mController.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f, time / endTime);
                yield return null;
            }
            yield return new WaitForSeconds(2f);
            mController.isDead = false;
            is2Phase = true;
            // 2������ ��ȯ�� �ִϸ��̼� �ӵ� 1.2��� ����
            mController.monsterAni.speed = 1.2f;
        }
        else
        {
            // 2������� ������ ����
            // ������ ��ü�� �������� �ϱ����� �׺�Ž� ��Ȱ��ȭ
            mController.mAgent.enabled = false;
            // 4�ʿ� ���� �� 2f��ŭ ������ ������ �ڿ� ��Ʈ����
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
    #endregion // �������� ���� ó��

    #region �����̵� ����(���Ÿ�)
    //! �����̵� ���� �Լ�
    private void SlideAttack()
    {
        StartCoroutine(UseSlideAttack());
    } // SlideAttack

    //! �����̵� ���� �ڷ�ƾ�Լ�
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

    //! �����̵� ���� ��� �Ÿ� üũ�ϴ� �ڷ�ƾ�Լ�
    private IEnumerator CheckSlideDistance()
    {
        isNoRangeAttack = true;
        while (isNoRangeAttack == true)
        {
            // Ÿ���� �����̵� �ּһ�Ÿ� �ۿ� ������ �����̵� ���� ��밡��
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
    #endregion // �����̵� ����(���Ÿ�)

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
        yield return null;
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
        float waitTime = default;
        float leapTime = default;
        if (is2Phase == true)
        {
            // 2������ : �ִϸ��̼� �ӵ��� 1.2�� �����Ѹ�ŭ �ð�����
            waitTime = 0.8f / mController.monsterAni.speed;
            leapTime = 1f / mController.monsterAni.speed;
        }
        else
        {
            waitTime = 0.8f;
            leapTime = 1f;
        }
        yield return new WaitForSeconds(waitTime);
        // ������ �̵��Լ��� ����ϱ� ���� Parabola �ʱ�ȭ
        Parabola parabola = new Parabola();
        // ���Ͱ� Ÿ���� �ٶ󺸴� ������ �ݴ������ ����
        Vector3 dir = -(mController.targetSearch.hit.transform.position - mController.transform.position).normalized;
        // ��ǥ��ġ�� dir�������� meleeAttackRange��ŭ �̵��� ��ǥ�� ����
        Vector3 targetPos = mController.targetSearch.hit.transform.position + dir * meleeAttackRange;
        mController.transform.LookAt(mController.targetSearch.hit.transform.position);
        StartCoroutine(parabola.ParabolaMoveToTarget(mController.transform.position, targetPos, leapTime, gameObject));
        yield return new WaitForSeconds(mController.monsterAni.GetCurrentAnimatorStateInfo(0).length - waitTime);
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

    #region ��ųC ���� ����
    //! �ذ�� ��ųC �Լ� (���� ����)
    private void SkillC()
    {
        StartCoroutine(UseSkillC());
    } // SkillC

    //! ��ųC (���� ����) �ڷ�ƾ�Լ�
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

    //! ��ųC ��ٿ� �ڷ�ƾ�Լ�
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
    #endregion // ��ųC ���� ����

    #region ��ųD Ÿ������ ���� ����
    //! �ذ�� ��ųD �Լ� (Ÿ������ ���� ����)
    private void SkillD()
    {
        StartCoroutine(UseSkillD());
    } // SkillD

    //! ��ųD �ڷ�ƾ�Լ�
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

    //! ��ųD ��ٿ� �ڷ�ƾ�Լ�
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
    #endregion // ��ųD Ÿ������ ���� ����
    //! } �ذ�� �׸� region ����
} // SkeletonKing
