using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using Unity.VisualScripting;
=======
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
using UnityEngine;

public class SkeletonArcher : Monster
{
    private MonsterController mController = default;
    [SerializeField] private GameObject weapon = default;
<<<<<<< HEAD
    [SerializeField] private MonsterData monsterData;
=======
    [SerializeField] private Transform arrowPos = default;
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private AudioClip attackAClip = default;
    [SerializeField] private bool useSkillA = default;
    [SerializeField] private float skillA_MaxCool = default;
    private ProjectilePool arrowPool = default;
    private GameObject skillA_Prefab = default;
    private DamageMessage damageMessage = default;
    private int defaultDamage = default;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    private float skillACool = 0f;
    private bool isAttackDelay = false;
    private void Awake()
    {
        mController = gameObject.GetComponent<MonsterController>();
<<<<<<< HEAD
        InitMonsterData(MonsterType.RANGE, monsterData);
        mController.monster = this;
    } // Awake

    //! ���� ó�� �̺�Ʈ�Լ� (Collider)
    private void EnableWeapon()
    {
        weapon.SetActive(true);
    } // EnableWeapon

    //! ȭ�� ��� �Լ�
    private void ShootArrow()
    {
        Vector3 dir = (mController.targetSearch.hit.transform.position - mController.transform.position).normalized;
        ArrowPool.Instance.GetArrow(dir, weapon.transform.position);
    } // ShootArrow

=======
        InitMonsterData(MonsterType.NOMAL, monsterData);
        mController.monster = this;
        defaultDamage = damage;
        arrowPool = gameObject.GetComponent<ProjectilePool>();
        damageMessage = new DamageMessage(gameObject, damage);
        skillA_Prefab = Resources.Load("Prefabs/Monster/MonsterEffect/Skeleton_Archer_Effect/ArrowRain") as GameObject;
        CheckUseSkill();
    } // Awake

>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
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
<<<<<<< HEAD
        SkillA();
    } // Skill

    //! �������� �̺�Ʈ�Լ�
    private void ExitAttack()
    {
=======
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
        GameObject arrow = arrowPool.GetProjecttile();
        arrow.GetComponent<Arrow>().InitDamageMessage(arrowPool, gameObject, defaultDamage);
        if (arrow == null || arrow == default)
        {
            arrow = arrowPool.GetProjecttile();
        }
        arrow.transform.position = arrowPos.position;
        Vector3 dir = ((mController.targetSearch.hit.transform.position + Vector3.up) - arrow.transform.position).normalized;
        arrow.transform.forward = dir;
        arrow.SetActive(true);

    } // ShootArrow

    //! �������� �̺�Ʈ�Լ�
    public override void ExitAttack()
    {
        damage = defaultDamage;
        weapon.SetActive(false);
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        mController.monsterAni.SetBool("isAttackA", false);
        mController.monsterAni.SetBool("isAttackB", false);
        mController.monsterAni.SetBool("isSkillA", false);
        // �������� �� ������ ����
<<<<<<< HEAD
        StartCoroutine(AttackDelay(mController, 4));
    } // ExitAttack

    //! ��ųA �Լ�
=======
        mController.isDelay = true;
    } // ExitAttack
    #endregion // ���� ó�� (Collider, RayCast)

    #region ��ųA (ȭ���)
    //! ��ųA �Լ� (ȭ���)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    private void SkillA()
    {
        mController.monsterAni.SetBool("isSkillA", true);
        StartCoroutine(LookAtTarget());
        StartCoroutine(SkillACooldown());
    } // SkillA

<<<<<<< HEAD
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

=======
    //! ��ųA ��� �̺�Ʈ�Լ�
    private void UseSkillA()
    {
        StartCoroutine(OnEffectSkillA());
    } //UseSkillA

    //! ��ųA ���������� �Լ�
    private void SkillA_Damage(Vector3 _effectPos)
    {
        damageMessage.damageAmount = defaultDamage * 2;
        RaycastHit[] hits = Physics.SphereCastAll(_effectPos, 2.5f, Vector3.up, 0f, LayerMask.GetMask(GData.PLAYER_MASK, GData.BUILD_MASK));
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
        Vector3 pos = mController.targetSearch.hit.transform.position + new Vector3(0f, 0.1f, 0f);
        // ���ݹ��� ǥ��
        mController.attackIndicator.GetCircleIndicator(pos, 5f, 1.5f);
        yield return new WaitForSeconds(1.5f);
        GameObject effectObj = Instantiate(skillA_Prefab);
        ParticleSystem effect = effectObj.GetComponent<ParticleSystem>();
        effectObj.transform.position = pos;
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
    #endregion // ��ųA (��� ���)

    #region Ÿ�� ����
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    //! Ÿ���� �ٶ󺸴� �ڷ�ƾ�Լ�
    private IEnumerator LookAtTarget()
    {
        isAttackDelay = false;
<<<<<<< HEAD
        bool isLookAt = true;
        while (isLookAt == true)
        {
            // ���ݵ����̰� ���۵Ǹ� ����
            if (isAttackDelay == true)
            {
                isLookAt = false;
                yield break;
            }
            Vector3 dir = (mController.targetSearch.hit.transform.position - mController.transform.position).normalized;
            mController.transform.rotation = Quaternion.Lerp(mController.transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
=======
        while (isAttackDelay == false)
        {
            if (mController.enumState != MonsterController.MonsterState.SKILL
                && mController.enumState != MonsterController.MonsterState.ATTACK)
            {
                isAttackDelay = true;
                yield break;
            }
            mController.transform.LookAt(mController.targetSearch.hit.transform.position);
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            yield return null;
        }
    } // LookTarget

    //! Ÿ�� �ٶ󺸱� �����ϴ� �̺�Ʈ�Լ�
    private void OffLookAtTarget()
    {
        isAttackDelay = true;
    } // OffLookAtTarget
<<<<<<< HEAD
=======
    #endregion // Ÿ�� ����

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

    private void AttackASound()
    {
        mController.monsterAudio.clip = attackAClip;
        mController.monsterAudio.Play();
    } // AttackASound
    #endregion // ���� ����
    //! } �ذ�ü� �׸� region ����
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
} // SkeletonArcher
