using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using static Monster;
=======
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498

public class SkeletonMage : Monster
{
    private MonsterController mController = default;
<<<<<<< HEAD
    private float skillACool = 0f;
    private float skillBCool = 0f;
    public MonsterData monsterData;
    public GameObject attackA_Effect;
    private void Awake()
    {
        mController = gameObject.GetComponent<MonsterController>();
        InitMonsterData(MonsterType.RANGE, monsterData);
        mController.monster = this;
=======
    [SerializeField] private MonsterData monsterData = default;
    [SerializeField] private GameObject summonObjPrefab = default;
    [SerializeField] private bool useSkillA = default;
    [SerializeField] private bool useSkillB = default;
    [SerializeField] private float skillA_MaxCool = default;
    [SerializeField] private float skillB_MaxCool = default;
    private ProjectilePool fireballPool = default;
    private DamageMessage damageMessage = default;
    private GameObject attackA_Prefab = default;
    private GameObject skillA_Prefab = default;
    private int defaultDamage = default;
    private float skillACool = 0f;
    private float skillBCool = 0f;
    private void Awake()
    {
        mController = gameObject.GetComponent<MonsterController>();
        InitMonsterData(MonsterType.NOMAL, monsterData);
        mController.monster = this;
        defaultDamage = damage;
        fireballPool = gameObject.GetComponent<ProjectilePool>();
        damageMessage = new DamageMessage(gameObject, damage);
        attackA_Prefab = Resources.Load("Prefabs/Monster/MonsterEffect/Skeleton_Mage_Effect/MageMelee") as GameObject;
        skillA_Prefab = Resources.Load("Prefabs/Monster/MonsterEffect/Skeleton_Mage_Effect/Summon_Thorn") as GameObject;
        CheckUseSkill();
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
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
<<<<<<< HEAD
=======
            useSkillA = false;
            CheckUseSkill();
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            SkillA();
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
    } // Skill

<<<<<<< HEAD
    //! �������� �̺�Ʈ�Լ�
    private void ExitAttack()
    {
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
                IDamageable damageable = _hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damageMessage);
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
        GameObject fireBall = fireballPool.GetProjecttile();
        if (fireBall == null || fireBall == default)
        {
            fireBall = fireballPool.GetProjecttile();
        }
        fireBall.GetComponent<FireBall>().InitDamageMessage(fireballPool, gameObject, defaultDamage, mController.targetSearch.hit.gameObject);
        fireBall.transform.position = transform.position + Vector3.up * 2f;
        fireBall.SetActive(true);
    } // ShootFireBall

    //! �������� �̺�Ʈ�Լ�
    public override void ExitAttack()
    {
        damage = defaultDamage;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        mController.monsterAni.SetBool("isAttackA", false);
        mController.monsterAni.SetBool("isAttackB", false);
        mController.monsterAni.SetBool("isSkillA", false);
        mController.monsterAni.SetBool("isSkillB", false);
        // �������� �� ������ ����
<<<<<<< HEAD
        StartCoroutine(AttackDelay(mController, 4));
    } // ExitAttack

    //! ��ųA �Լ�
    private void SkillA()
    {
        mController.monsterAni.SetBool("isSkillA", true);
        StartCoroutine(LookTarget());
        StartCoroutine(SkillACooldown());
    } // SkillA

    private void SkillB()
    {
        mController.monsterAni.SetBool("isSkillB", true);
        StartCoroutine(SkillBCooldown());
    } // SkillB

    //! ��ųA ��ٿ� �ڷ�ƾ�Լ�
    private IEnumerator SkillACooldown()
    {
        useSkillA = false;
        // ������Ʈ�ѷ����� �������� �� üũ�� ���� : ���Ÿ� ��ų �� ����
        while (true)
        {
            skillACool += Time.deltaTime;
            if (skillACool >= skillA_MaxCool)
            {
                skillACool = 0f;
                useSkillA = true;
                yield break;
            }
            yield return null;
        }
    } // SkillACooldown

    //! ��ųB ��ٿ� �ڷ�ƾ�Լ�
    private IEnumerator SkillBCooldown()
    {
        useSkillB = false;
        // ������Ʈ�ѷ����� �������� �� üũ�� ���� : ���Ÿ� ��ų �� ����
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
    } // SkillACooldown

=======
        mController.isDelay = true;
    } // ExitAttack

>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
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
<<<<<<< HEAD
            Vector3 dir = (mController.targetSearch.hit.transform.position - mController.transform.position).normalized;
            mController.transform.rotation = Quaternion.Lerp(mController.transform.rotation, Quaternion.LookRotation(dir), 2f * Time.deltaTime);
            yield return null;
        }
    } // LookTarget
=======
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

    //! ��ųA ��� �̺�Ʈ�Լ�
    private void UseSkillA()
    {
        StartCoroutine(OnEffectSkillA());
    } // UseSkillA

    //! ��ųA ���������� �Լ�
    private void SkillA_Damage(Vector3 _effectPos)
    {
        damageMessage.damageAmount = defaultDamage * 2;
        RaycastHit[] hits = Physics.SphereCastAll(_effectPos, 2f, Vector3.up, 0f, LayerMask.GetMask(GData.PLAYER_MASK, GData.BUILD_MASK));
        if (hits.Length > 0)
        {
            foreach (var _hit in hits)
            {
                IDamageable damageable = _hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damageMessage);
                }
            }
        }
        damageMessage.damageAmount = defaultDamage;
    } // SkillA_Damage

    //! ��ųA ����Ʈ �ڷ�ƾ�Լ�
    private IEnumerator OnEffectSkillA()
    {
        Vector3 targetPos = mController.targetSearch.hit.transform.position + new Vector3(0f, 0.1f, 0f);
        // ���ݹ��� ǥ��
        mController.attackIndicator.GetCircleIndicator(targetPos, 4f, 1.5f);
        yield return new WaitForSeconds(1.5f);
        GameObject effectObj = Instantiate(skillA_Prefab);
        ParticleSystem effect = effectObj.GetComponent<ParticleSystem>();
        effectObj.transform.position = targetPos;
        effect.Play();
        SkillA_Damage(effectObj.transform.position);
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
        GetRandomPosition getRandomPos = new GetRandomPosition();
        // �������� ��ֹ��� ���� ��ǥ�� ������
        Vector3 pos = getRandomPos.GetRandomCirclePos(transform.position, 10, 4);
        Vector3 dirToTarget = (mController.targetSearch.hit.transform.position - pos).normalized;
        // �ذ񺴻簡 ��ȯ�� �� Ÿ���� �ٶ󺸸鼭 ��ȯ�ǰ� ȸ���� ����
        Instantiate(summonObjPrefab, pos, Quaternion.LookRotation(dirToTarget));
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
    //! } �ذ񸶹��� �׸� region ����
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
} // SkeletonMage
