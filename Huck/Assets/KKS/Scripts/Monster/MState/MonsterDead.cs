using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDead : IMonsterState
{
    private MonsterController mController;
    public void StateEnter(MonsterController _mController)
    {
        this.mController = _mController;
        mController.enumState = MonsterController.MonsterState.DEAD;
        mController.monsterAni.SetBool("isDead", true);
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
        /*Do Nothing*/
    } // StateExit
} // MonsterDead
