using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    //! ���� Ÿ��
    public enum MonsterType
    {
        MELEE = 0,
        RANGE
    } // MonsterType

    [HideInInspector] public MonsterType monsterType;
    [HideInInspector] public string monsterName;
    [HideInInspector] public int monsterHp;
    [HideInInspector] public int monsterMaxHp;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public int damage;
    [HideInInspector] public bool isNoRangeAttack;
    [HideInInspector] public bool isNoRangeSkill;
    [HideInInspector] public bool useSkillA;
    [HideInInspector] public bool useSkillB;
    [HideInInspector] public float skillA_MaxCool;
    [HideInInspector] public float skillB_MaxCool;
    [HideInInspector] public float searchRange;
    [HideInInspector] public float attackRange;
    [HideInInspector] public float meleeAttackRange;
    [HideInInspector] public int delayType;

    //! ���� ������ �ʱ�ȭ�ϴ� �Լ�
    public void InitMonsterData(MonsterType _monsterType, MonsterData monsterData)
    {
        this.monsterType = _monsterType;
        this.monsterName = monsterData.MonsterName;
        this.monsterHp = monsterData.MonsterHp;
        this.monsterMaxHp = monsterData.MonsterMaxHp;
        this.moveSpeed = monsterData.MoveSpeed;
        this.damage = monsterData.Damage;
        this.isNoRangeAttack = monsterData.IsNoRangeAttack;
        this.isNoRangeSkill = monsterData.IsNoRangeSkill;
        this.useSkillA = monsterData.UseSkillA;
        this.useSkillB = monsterData.UseSkillB;
        this.skillA_MaxCool = monsterData.SkillA_MaxCooldown;
        this.skillB_MaxCool = monsterData.SkillB_MaxCooldown;
        this.searchRange = monsterData.SearchRange;
        this.attackRange = monsterData.AttackRange;
        this.meleeAttackRange = monsterData.MeleeAttackRange;
        this.delayType = monsterData.DelayType;
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

    
} // Monster
