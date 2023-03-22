using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Object/MonsterData", order = int.MaxValue)]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private string monsterName; // ���� �̸�
    public string MonsterName { get { return monsterName; } }

    [SerializeField]
    private int monsterHp; // ���� HP
    public int MonsterHp { get { return monsterHp; } }

    [SerializeField]
    private int monsterMaxHp; // ���� MAX_HP
    public int MonsterMaxHp { get { return monsterMaxHp; } }

    [SerializeField]
    private float moveSpeed; // ���� �̵��ӵ�
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField]
    private int damage; // ���� �ּ� ���ݷ�
    public int Damage { get { return damage; } }

    [SerializeField]
    private bool isNoRangeAttack; // ���� ���Ÿ� ���� ����
    public bool IsNoRangeAttack { get { return isNoRangeAttack; } }

    [SerializeField]
    private bool isNoRangeSkill; // ���� ���Ÿ� ��ų ����
    public bool IsNoRangeSkill { get { return isNoRangeSkill; } }

    [SerializeField]
    private bool useSkillA; // ���� ��ųA ��밡�� üũ
    public bool UseSkillA { get { return useSkillA; } }

    [SerializeField]
    private bool useSkillB; // ���� ��ųB ��밡�� üũ
    public bool UseSkillB { get { return useSkillB; } }

    [SerializeField]
    private float skillA_MaxCooldown; // ���� ��ųA ��ٿ�
    public float SkillA_MaxCooldown { get { return skillA_MaxCooldown; } }

    [SerializeField]
    private float skillB_MaxCooldown; // ���� ��ųB ��ٿ�
    public float SkillB_MaxCooldown { get { return skillB_MaxCooldown; } }

    [SerializeField]
    private float searchRange; // ���� Ž�� ����
    public float SearchRange { get { return searchRange; } }

    [SerializeField]
    private float attackRange; // ���� ���� ��Ÿ�
    public float AttackRange { get { return attackRange; } }

    [SerializeField]
    private float meleeAttackRange; // ���� �������� ��Ÿ�
    public float MeleeAttackRange { get { return meleeAttackRange; } }

    [SerializeField]
    private int delayType; // ���� ���ݵ����� Ÿ��
    public int DelayType { get { return delayType; } }
} // MonsterData