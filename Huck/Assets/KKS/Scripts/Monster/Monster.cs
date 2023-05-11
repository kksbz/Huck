using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    //! ���� Ÿ��
    public enum MonsterType
    {
<<<<<<< HEAD
        MELEE = 0,
        RANGE
=======
        NOMAL = 0,
        NAMEED,
        BOSS
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    } // MonsterType

    [HideInInspector] public MonsterType monsterType;
    [HideInInspector] public string monsterName;
<<<<<<< HEAD
    [HideInInspector] public float monsterHp;
    [HideInInspector] public float monsterMaxHp;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float minDamage;
    [HideInInspector] public float maxDamage;
    [HideInInspector] public bool isNoRangeAttack;
    [HideInInspector] public bool isNoRangeSkill;
    [HideInInspector] public bool useSkillA;
    [HideInInspector] public bool useSkillB;
    [HideInInspector] public float skillA_MaxCool;
    [HideInInspector] public float skillB_MaxCool;
    [HideInInspector] public float searchRange;
    [HideInInspector] public float attackRange;
    [HideInInspector] public float meleeAttackRange;
=======
    [HideInInspector] public int monsterHp;
    [HideInInspector] public int monsterMaxHp;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public int damage;
    [HideInInspector] public bool isNoRangeAttack;
    [HideInInspector] public bool isNoRangeSkill;
    [HideInInspector] public bool useSkill;
    [HideInInspector] public float searchRange;
    [HideInInspector] public float attackRange;
    [HideInInspector] public float meleeAttackRange;
    [HideInInspector] public AudioClip roarClip;
    [HideInInspector] public AudioClip deadClip;
    [HideInInspector] public AudioClip moveClip;
    [HideInInspector] public AudioClip hitClip;
    [HideInInspector] public AudioClip weaponClip;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498

    //! ���� ������ �ʱ�ȭ�ϴ� �Լ�
    public void InitMonsterData(MonsterType _monsterType, MonsterData monsterData)
    {
        this.monsterType = _monsterType;
        this.monsterName = monsterData.MonsterName;
        this.monsterHp = monsterData.MonsterHp;
        this.monsterMaxHp = monsterData.MonsterMaxHp;
        this.moveSpeed = monsterData.MoveSpeed;
<<<<<<< HEAD
        this.minDamage = monsterData.MinDamage;
        this.maxDamage = monsterData.MaxDamage;
        this.isNoRangeAttack = monsterData.IsNoRangeAttack;
        this.isNoRangeSkill = monsterData.IsNoRangeSkill;
        this.useSkillA = monsterData.UseSkillA;
        this.useSkillB = monsterData.UseSkillB;
        this.skillA_MaxCool = monsterData.SkillA_MaxCooldown;
        this.skillB_MaxCool = monsterData.SkillB_MaxCooldown;
        this.searchRange = monsterData.SearchRange;
        this.attackRange = monsterData.AttackRange;
        this.meleeAttackRange = monsterData.MeleeAttackRange;
=======
        this.damage = monsterData.Damage;
        this.isNoRangeAttack = monsterData.IsNoRangeAttack;
        this.isNoRangeSkill = monsterData.IsNoRangeSkill;
        this.useSkill = monsterData.UseSkill;
        this.searchRange = monsterData.SearchRange;
        this.attackRange = monsterData.AttackRange;
        this.meleeAttackRange = monsterData.MeleeAttackRange;
        this.roarClip = monsterData.RoarAudio;
        this.deadClip = monsterData.DeadAudio;
        this.moveClip = monsterData.MoveAudio;
        this.hitClip = monsterData.HitAudio;
        this.weaponClip = monsterData.WeaponAudio;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    } // InitMonsterData

    //! ���� �Լ�
    public virtual void Attack()
    {
        /* Do Nothing */
    } // Attack

    //! ��ų���� �Լ�
    public virtual void Skill()
    {
        /* Do Nothing */
    } // Skill

<<<<<<< HEAD
    //! ���ݵ����� �ִ� �ڷ�ƾ�Լ�
    protected IEnumerator AttackDelay(MonsterController mController, int _number)
    {
        int number = Random.Range(0, _number);
        switch (number)
        {
            case 0:
                float checkTime = 0f;
                bool isBackMove = false;
                //Debug.Log($"�鹫�� ����");
                mController.monsterAni.SetBool("isBack", true);
                while (isBackMove == false)
                {
                    checkTime += Time.deltaTime;
                    if (checkTime >= 1.5f)
                    {
                        isBackMove = true;
                    }
                    Vector3 dir = (mController.targetSearch.hit.transform.position - mController.transform.position).normalized;
                    mController.transform.rotation = Quaternion.Lerp(mController.transform.rotation, Quaternion.LookRotation(dir), 2f * Time.deltaTime);
                    mController.mAgent.Move(-dir * moveSpeed * Time.deltaTime);
                    yield return null;
                }
                //Debug.Log($"�鹫�� ����");
                mController.monsterAni.SetBool("isBack", false);
                break;
            case 1:
                float checkTime2 = 0f;
                bool isIdle = false;
                //Debug.Log($"��� ����");
                while (isIdle == false)
                {
                    checkTime2 += Time.deltaTime;
                    if (checkTime2 >= 2f)
                    {
                        isIdle = true;
                    }
                    Vector3 dir = (mController.targetSearch.hit.transform.position - mController.transform.position).normalized;
                    mController.transform.rotation = Quaternion.Lerp(mController.transform.rotation, Quaternion.LookRotation(dir), 2f * Time.deltaTime);

                    yield return null;
                }
                //Debug.Log($"��� ����");
                break;
            case 2:
                float checkTime3 = 0f;
                bool isSideMove = false;
                int sideNumber = Random.Range(0, 2);
                //Debug.Log("���̵幫�� ����");
                while (isSideMove == false)
                {
                    checkTime3 += Time.deltaTime;
                    if (checkTime3 >= 2f)
                    {
                        isSideMove = true;
                    }
                    Vector3 dir = (mController.targetSearch.hit.transform.position - mController.transform.position).normalized;
                    mController.transform.rotation = Quaternion.Lerp(mController.transform.rotation, Quaternion.LookRotation(dir), 2f * Time.deltaTime);
                    if (sideNumber == 0)
                    {
                        mController.monsterAni.SetBool("isRight", true);
                        mController.mAgent.Move(mController.transform.right.normalized * moveSpeed * Time.deltaTime);
                    }
                    else
                    {
                        mController.monsterAni.SetBool("isLeft", true);
                        mController.mAgent.Move(-mController.transform.right.normalized * moveSpeed * Time.deltaTime);
                    }
                    yield return null;
                }
                mController.monsterAni.SetBool("isRight", false);
                mController.monsterAni.SetBool("isLeft", false);
                //Debug.Log($"���̵幫�� ����");
                break;
            case 3:
                mController.monsterAni.SetTrigger("isRoar");
                yield return new WaitForSeconds(0.1f);
                yield return new WaitForSeconds(mController.monsterAni.GetCurrentAnimatorStateInfo(0).length);
                break;
        }
        // ���ݵ����̰� �������� Idle���·� �ʱ�ȭ
        IMonsterState nextState = new MonsterIdle();
        mController.MStateMachine.onChangeState?.Invoke(nextState);
    } // AttackDelay
=======
    //! �������� �Լ�
    public virtual void ExitAttack()
    {
        /* Do Nothing */
    } // ExitAttack

    //! �������� ����ó�� �Լ�
    public virtual void BossDead()
    {
        /* Do Nothing */
    } // BossDead
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
} // Monster
