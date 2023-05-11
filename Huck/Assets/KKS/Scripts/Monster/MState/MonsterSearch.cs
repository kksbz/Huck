using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
=======
using UnityEngine;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498

public class MonsterSearch : IMonsterState
{
    private MonsterController mController;
<<<<<<< HEAD
    private bool exitState; // 코루틴 while문 조건
=======
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    public void StateEnter(MonsterController _mController)
    {
        this.mController = _mController;
        mController.enumState = MonsterController.MonsterState.SEARCH;
        Debug.Log($"추적상태 시작 : {mController.monster.monsterName}");
        mController.CoroutineDeligate(TargetChase());
    } // StateEnter
    public void StateFixedUpdate()
    {
        /*Do Nothing*/
    } // StateFixedUpdate
    public void StateUpdate()
    {
<<<<<<< HEAD
        /*Do Nothing*/
    } // StateUpdate
    public void StateExit()
    {
        exitState = true;
        mController.mAgent.ResetPath();
        mController.mAgent.speed = mController.monster.moveSpeed;
        mController.monsterAni.SetBool("isWalk", false);
=======
        mController.targetSearch.SearchTarget();
    } // StateUpdate
    public void StateExit()
    {
        mController.mAgent.ResetPath();
        mController.monsterAni.SetBool("isWalk", false);
        mController.mAgent.speed = mController.monster.moveSpeed;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    } // StateExit

    //! 타겟을 추적하는 코루틴함수
    private IEnumerator TargetChase()
    {
        mController.mAgent.speed = mController.monster.moveSpeed * 0.7f;
<<<<<<< HEAD
        exitState = false;
        mController.monsterAni.SetBool("isWalk", true);
        while (exitState == false)
        {
            mController.mAgent.SetDestination(mController.target.transform.position);
=======
        mController.monsterAni.SetBool("isWalk", true);
        GetRandomPosition getRandomPosition = new GetRandomPosition();
        Vector3 targetPos = getRandomPosition.GetRandomCirclePos(mController.transform.position, 20, 10);
        float time = 0f;
        bool isTargetToPlayer = false;
        mController.targetSearch.hit = null;
        while (mController.targetSearch.hit == null)
        {
            float distance = Vector3.Distance(targetPos, mController.transform.position);
            // 타겟위치와의 거리가 0.5f 이하면 새로운 타겟좌표를 정함
            if (distance <= 0.5f)
            {
                time += Time.deltaTime;
                mController.mAgent.ResetPath();
                mController.monsterAni.SetBool("isWalk", false);
                // 3초간 Idle 모션으로 대기
                if (time > 3f)
                {
                    // 60% 확률로 새로운 랜덤좌표, 40% 확률로 플레이어 좌표 선택
                    int number = Random.Range(0, 10);
                    if (number < 5)
                    {
                        isTargetToPlayer = false;
                        targetPos = getRandomPosition.GetRandomCirclePos(mController.transform.position, 20, 10);
                    }
                    else
                    {
                        isTargetToPlayer = true;
                        targetPos = mController.target.transform.position;
                    }
                    time = 0f;
                    mController.monsterAni.SetBool("isWalk", true);
                }
            }
            else
            {
                mController.mAgent.SetDestination(targetPos);
                // 새로 정한 좌표가 플레이어 좌표라면 4초간 이동
                if (isTargetToPlayer == true)
                {
                    yield return new WaitForSeconds(4f);
                    isTargetToPlayer = false;
                    targetPos = mController.transform.position;
                }
            }
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            yield return null;
        }
    } // TargetChase
} // MonsterSearch
