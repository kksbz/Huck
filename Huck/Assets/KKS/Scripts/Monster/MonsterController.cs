using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour, IDamageable
{
    //! 몬스터의 상태 종류
    public enum MonsterState
    {
        IDLE = 0,
        MOVE,
        SEARCH,
        ATTACK,
        SKILL,
        DELAY,
        HIT,
        DEAD
    }; //MonsterState

    private Dictionary<MonsterState, IMonsterState> dicState = new Dictionary<MonsterState, IMonsterState>(); // 몬스터의 상태를 담을 딕셔너리
    private MStateMachine mStateMachine; // 몬스터의 상태를 처리할 스테이트머신
    private bool isSpawn = true;
    public MStateMachine MStateMachine { get; private set; }
    [HideInInspector] public Monster monster;
    public MonsterState enumState = MonsterState.IDLE; // 몬스터의 현재 상태를 체크하기 위한 변수
    public float currentHp; // 몬스터의 현재 HP 변수
    [HideInInspector] public Rigidbody monsterRb = default;
    [HideInInspector] public Animator monsterAni = default;
    [HideInInspector] public AudioSource monsterAudio = default;
    [HideInInspector] public TargetSearchRay targetSearch = default;
    [HideInInspector] public NavMeshAgent mAgent = default;
    [HideInInspector] public bool isDelay = false;
    [HideInInspector] public bool isHit = false;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public GameObject attacker = default;
    // { Test
    public GameObject target;
    public float distance; // 타겟과의 거리 변수
    // } Test

    // Start is called before the first frame update
    void Start()
    {
        currentHp = monster.monsterHp;
        monsterRb = gameObject.GetComponent<Rigidbody>();
        monsterAni = gameObject.GetComponent<Animator>();
        monsterAudio = gameObject.GetComponent<AudioSource>();
        targetSearch = gameObject.GetComponent<TargetSearchRay>();
        mAgent = gameObject.GetComponent<NavMeshAgent>();
        mAgent.acceleration = 100f;
        mAgent.angularSpeed = 180f;
        mAgent.speed = monster.moveSpeed;

        // { 각 상태를 Dictionary에 저장
        IMonsterState idle = new MonsterIdle();
        IMonsterState move = new MonsterMove();
        IMonsterState search = new MonsterSearch();
        IMonsterState attack = new MonsterAttack();
        IMonsterState skill = new MonsterSkill();
        IMonsterState delay = new MonsterDelay();
        IMonsterState hit = new MonsterHit();
        IMonsterState dead = new MonsterDead();

        dicState.Add(MonsterState.IDLE, idle);
        dicState.Add(MonsterState.MOVE, move);
        dicState.Add(MonsterState.SEARCH, search);
        dicState.Add(MonsterState.ATTACK, attack);
        dicState.Add(MonsterState.SKILL, skill);
        dicState.Add(MonsterState.DELAY, delay);
        dicState.Add(MonsterState.HIT, hit);
        dicState.Add(MonsterState.DEAD, dead);
        // } 각 상태를 Dictionary에 저장

        // 입력받은 상태를 처리할 MStateMachine 초기화 
        MStateMachine = new MStateMachine(idle, this);
        // 매프레임마다 실행 시켜줄 필요가 없어서 0.5초마다 타겟 정보 갱신
        InvokeRepeating("GetTarget", 0f, 0.5f);
        StartCoroutine(Spawn());
    } // Start

    // Update is called once per frame
    void Update()
    {
        if (isSpawn == false)
        {
            MonsterSetState();
        }
        MStateMachine.DoUpdate();
    } // Update

    void FixedUpdate()
    {
        MStateMachine.DoFixedUpdate();
    } // FixedUpdate

    //! 타겟의 정보를 가져오는 함수
    private void GetTarget()
    {
        target = GameManager.Instance.playerObj;
    } // GetTarget

    //! 몬스터 스폰 코루틴함수
    private IEnumerator Spawn()
    {
        monsterAni.SetTrigger("isSpawn");
        isSpawn = true;
        // 기본상태가 Idle이기 때문에 현재Clip이 Spawn으로 갱신되도록 0.1초 기다림
        yield return null;
        //Debug.Log($"{monsterAni.GetCurrentAnimatorClipInfo(0)[0].clip.name}");
        //Debug.Log($"{monster.monsterName}, {monsterAni.GetCurrentAnimatorStateInfo(0).length}");
        yield return new WaitForSeconds(monsterAni.GetCurrentAnimatorStateInfo(0).length);
        isSpawn = false;
    } // Spawn

    public int hp;
    //! 공격받으면 처리하는 함수 (interface 상속)
    public void TakeDamage(GameObject _attacker, int _damage)
    {
        // 스폰상태일 때 무적처리
        if (isSpawn == true)
        {
            return;
        }
        // 공격중이 아닐 때만 Hit상태로 전환하기 위한 예외처리
        if (enumState != MonsterState.ATTACK && enumState != MonsterState.SKILL)
        {
            isHit = true;
        }
        monster.monsterHp -= _damage;
        hp = monster.monsterHp;
        attacker = _attacker;
        if (monster.monsterHp <= 0f)
        {
            isDead = true;
        }
        Debug.Log($"{_attacker.name}한테 {_damage} 피해입음! 현재체력:{monster.monsterHp}, {isHit}");
    } // TakeDamage

    //! interface를 상속받은 클래스는 MonoBehaviour를 상속 받지 못해서 코루틴을 대신 실행시켜줄 함수
    public void CoroutineDeligate(IEnumerator func)
    {
        StartCoroutine(func);
    } // CoroutineDeligate

    //! 코루틴을 대신 실행시켜주고 반환값 있는 함수
    public Coroutine CoroutineDeligateReturn(IEnumerator func)
    {
        Coroutine a = StartCoroutine(func);
        return a;
    }

    //! 코루틴을 대신 종료시켜줄 함수
    public void StopCoroutineDeligate(IEnumerator func)
    {
        StopCoroutine(func);
    } // StopCoroutineDeligate

    //! 몬스터 상태 정하는 함수
    private void MonsterSetState()
    {
        if (isDelay == true)
        {
            MStateMachine.SetState(dicState[MonsterState.DELAY]);
        }
        if (isHit == true && (enumState == MonsterState.ATTACK || enumState == MonsterState.SKILL))
        {
            isHit = false;
        }
        if (isHit == true)
        {
            MStateMachine.SetState(dicState[MonsterState.HIT]);
        }

        float _distance = Vector3.Distance(transform.position, target.transform.position);
        // 타겟이 몬스터의 탐색범위 밖에 있으면 추적
        if (_distance > monster.searchRange)
        {
            MStateMachine.SetState(dicState[MonsterState.SEARCH]);
            return;
        }

        // 타겟이 몬스터의 탐색범위 안에 있을 때 탐색 실행
        targetSearch.SearchTarget();
        distance = Vector3.Distance(this.transform.position, targetSearch.hit.gameObject.transform.position);

        // 공격, 스킬 상태가 아니면 이동상태로 전환
        if (enumState != MonsterState.ATTACK
            && enumState != MonsterState.SKILL
            && enumState != MonsterState.HIT
            && enumState != MonsterState.DELAY)
        {
            MStateMachine.SetState(dicState[MonsterState.MOVE]);
        }
        // { 타겟이 공격사거리 안에 있으면 공격 및 스킬 상태로 전환
        if (distance <= monster.attackRange && enumState != MonsterState.DELAY && enumState != MonsterState.HIT)
        {
            // 몬스터의 스킬이 사용가능할 때
            if (enumState != MonsterState.ATTACK && (monster.useSkillA == true || monster.useSkillB == true))
            {
                // 몬스터의 원거리 스킬 유무에 따라 실행
                switch (monster.isNoRangeSkill)
                {
                    case true:
                        if (distance <= monster.meleeAttackRange)
                        {
                            MStateMachine.SetState(dicState[MonsterState.SKILL]);
                        }
                        break;
                    case false:
                        if (distance > monster.meleeAttackRange)
                        {
                            MStateMachine.SetState(dicState[MonsterState.SKILL]);
                        }
                        break;
                } // switch end
            } // if end
            // if : 스킬이 모두 사용불가능하면 공격
            if (enumState != MonsterState.SKILL)
            {
                // 몬스터의 원거리 공격 유무에 따라 실행
                switch (monster.isNoRangeAttack)
                {
                    case true:
                        if (distance <= monster.meleeAttackRange)
                        {
                            MStateMachine.SetState(dicState[MonsterState.ATTACK]);
                        }
                        break;
                    case false:
                        if (distance > monster.meleeAttackRange)
                        {
                            MStateMachine.SetState(dicState[MonsterState.ATTACK]);
                        }
                        break;
                } // switch end
            } // if end
        }
        // } 타겟이 공격사거리 안에 있으면 공격 및 스킬 상태로 전환

    } // MonsterSetState
} // MonsterController
