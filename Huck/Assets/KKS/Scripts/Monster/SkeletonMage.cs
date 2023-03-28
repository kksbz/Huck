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
        StartCoroutine(OnEffectAttackA());
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up, 2.5f, Vector3.up, 0f, LayerMask.GetMask(GData.PLAYER_MASK, GData.BUILD_MASK));
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

    //! AttackA �������� ����Ʈ �ڷ�ƾ�Լ�
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

    //! �������� ���������� ���� �����
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, 2.5f);
    } // OnDrawGizmos

    //! AttackB ���̾ ��� �Լ�
    private void ShootFireBall()
    {
        GameObject fireBall = ProjectilePool.Instance.GetProjecttile();
        fireBall.GetComponent<FireBall>().InitDamageMessage(gameObject, defaultDamage, mController.targetSearch.hit.gameObject);
        fireBall.transform.position = transform.position + Vector3.up * 2f;
        fireBall.SetActive(true);
    } // ShootFireBall

    //! �������� �̺�Ʈ�Լ�
    public override void ExitAttack()
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

    #region ��ųA ���� ��ȯ
    //! ��ųA �Լ� (���� ��ȯ)
    private void SkillA()
    {
        mController.monsterAni.SetBool("isSkillA", true);
        StartCoroutine(LookTarget());
        StartCoroutine(SkillACooldown());
    } // SkillA

    //! ��ųA ���������� �̺�Ʈ�Լ�
    private void SkillA_Damage()
    {
        StartCoroutine(OnEffectSkillA());
    } // SkillA_Damage

    //! ��ųA ����Ʈ �ڷ�ƾ�Լ�
    private IEnumerator OnEffectSkillA()
    {
        GameObject effectObj = Instantiate(skillA_Prefab);
        ParticleSystem effect = effectObj.GetComponent<ParticleSystem>();
        effectObj.transform.position = mController.targetSearch.hit.transform.position;
        effectObj.transform.forward = transform.forward;
        yield return new WaitForSeconds(1f);
        effect.Play();
        // ��ųA ����Ʈ ����ɶ� ���������� ����
        damageMessage.damageAmount = defaultDamage * 2f;
        RaycastHit[] hits = Physics.SphereCastAll(effectObj.transform.position, 2.5f, Vector3.up, 0f, LayerMask.GetMask(GData.PLAYER_MASK, GData.BUILD_MASK));
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
        damageMessage.damageAmount = defaultDamage;
        yield return new WaitForSeconds(effect.main.duration + effect.main.startLifetime.constant);
        Destroy(effectObj);
    } // OnEffectSkillA

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
            Instantiate(summonObjPrefab, hit.point, Quaternion.LookRotation(dirToTarget));
            return;
        }
        else
        {
            //Debug.Log("��ȯ��ġ�� ��ֹ� ����! �ٸ���ǥ Ž������");
            // ���ѷ��� ����ó�� : ��ǥŽ�� 20�� �̻��̸� �ذ񸶹��� �տ� ��ȯ
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
