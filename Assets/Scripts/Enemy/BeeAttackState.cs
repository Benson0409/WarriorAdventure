using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAttackState : BasicState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.attackSpeed;
    }
    public override void LogicUpdate()
    {
        //敵人在面前,發動攻擊
        if (currentEnemy.isChase && currentEnemy.FoundPlayer())
        {
            currentEnemy.anim.SetBool("attack", true);
        }

        //敵人還在範圍內,但不在面前,切換到追擊模式
        if (!currentEnemy.isChase && currentEnemy.FoundPlayer())
        {
            currentEnemy.anim.SetBool("attack", false);
            currentEnemy.SwitchState(NPCState.Chase);
        }
    }



    public override void PhysicsUpdate()
    {
    }
    public override void OnExit()
    {
    }
}
