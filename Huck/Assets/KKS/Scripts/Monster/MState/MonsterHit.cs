using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHit : IMonsterState
{
    private MonsterController mController;
    public void StateEnter(MonsterController _mController)
    {
        this.mController = _mController;
        mController.enumState = MonsterController.MonsterState.HIT;
        Debug.Log($"Hit���� ���� : {mController.monster.monsterName}");
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
        mController.monster.isHit = false;
    } // StateExit

    //! ������ ������ �� ������ ó���ϴ� �Լ�
    private void HitProcess()
    {
        Vector3 dir = (mController.monster.attacker.transform.position - mController.transform.position).normalized;
        Debug.Log($"{dir}");
    } // HitProcess
}
