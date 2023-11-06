using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BasicState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }
    public override void LogicUpdate()
    {
        //切換追擊模式
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }

        //撞到牆後轉向  (撞到牆壁或偵測到懸崖就轉向）
        if ((!currentEnemy.physicsCheck.isGround) || (currentEnemy.physicsCheck.isLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.isRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.isWait = true;
            currentEnemy.anim.SetBool("walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("walk", true);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }

}
