using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BasicState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("run", true);
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        if ((!currentEnemy.physicsCheck.isGround) || (currentEnemy.physicsCheck.isLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.isRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("run", false);
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
    }

}
