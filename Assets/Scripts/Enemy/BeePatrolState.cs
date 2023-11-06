using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;

public class BeePatrolState : BasicState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }
    public override void LogicUpdate()
    {
        //發現敵人 移動到敵人旁邊
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }

        //在空間中任意移動只要不要碰撞到牆壁,地板就好，給他限制一個高度讓他不要亂飛
        if ((currentEnemy.physicsCheck.isLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.isRightWall && currentEnemy.faceDir.x > 0))
        {
            //尋找新的飛行點
            currentEnemy.isWait = true;
            currentEnemy.anim.SetBool("fly", false);
        }
        else
        {
            currentEnemy.anim.SetBool("fly", true);
        }

    }

    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {

    }
}
