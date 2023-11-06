using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailRecoverState : BasicState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.hideSpeed;
        currentEnemy.anim.SetBool("recover", true);
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
    }



    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("recover", false);
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
    }
}
