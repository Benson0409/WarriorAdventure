using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailHideState : BasicState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.hideSpeed;
        //躲藏動畫設定
        currentEnemy.anim.SetBool("hide", true);
    }
    public override void LogicUpdate()
    {
        if (!currentEnemy.FoundPlayer())
        {
            if (currentEnemy.lostTimeCounter <= 0)
            {
                currentEnemy.SwitchState(NPCState.Recover);
            }
        }
    }

    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("hide", false);
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
    }
}
