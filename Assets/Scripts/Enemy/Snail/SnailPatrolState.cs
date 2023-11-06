using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailPatrolState : BasicState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }
    public override void LogicUpdate()
    {
        //發現玩家,切換模式,躲到殼裡面,關閉collide(利用動畫關閉)
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Hide);
        }
        //正常左右巡邏狀態
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
