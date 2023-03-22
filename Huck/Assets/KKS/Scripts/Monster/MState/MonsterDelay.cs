using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDelay : IMonsterState
{
    private MonsterController mController;
    private IEnumerator runningCoroutine = default;
    public void StateEnter(MonsterController _mController)
    {
        this.mController = _mController;
        mController.enumState = MonsterController.MonsterState.DELAY;
        runningCoroutine = AttackDelay(mController.monster.delayType);
        mController.CoroutineDeligate(runningCoroutine);
    } // StateEnter
    public void StateFixedUpdate()
    {
        /*Do Nothing*/
    } // StateFixedUpdate
    public void StateUpdate()
    {
        /*Do Nothing*/
    } // StateUpdate
    public void StateExit()
    {
        mController.monsterAni.SetBool("isBack", false);
        mController.monsterAni.SetBool("isRight", false);
        mController.monsterAni.SetBool("isLeft", false);
        mController.isDelay = false;
        mController.StopCoroutineDeligate(runningCoroutine);
        Debug.Log($"������ ���� Ż�� {runningCoroutine}");
    } // StateExit

    //! ���ݵ����� �ִ� �ڷ�ƾ�Լ�
    private IEnumerator AttackDelay(int _number)
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
                    mController.mAgent.Move(-dir * mController.monster.moveSpeed * Time.deltaTime);
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
                        mController.mAgent.Move(mController.transform.right.normalized * mController.monster.moveSpeed * Time.deltaTime);
                    }
                    else
                    {
                        mController.monsterAni.SetBool("isLeft", true);
                        mController.mAgent.Move(-mController.transform.right.normalized * mController.monster.moveSpeed * Time.deltaTime);
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
        Debug.Log($"������ ���� �ʱ�ȭ {nextState}");
    } // AttackDelay

} // MonsterDelay
