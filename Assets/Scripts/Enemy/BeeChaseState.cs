using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//偵測到玩家後將玩家設為目標點，移動到距離玩家很近的距離
public class BeeChaseState : BasicState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
    }
    public override void LogicUpdate()
    {
        //敵人離開偵測區,退回Patrol模式
        if (!currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Patrol);
            currentEnemy.isChase = false;
            currentEnemy.isWait = true;
            return;
        }

        //已經追到敵人
        if (currentEnemy.isChase)
        {
            currentEnemy.SwitchState(NPCState.Attack);
        }

    }

    public override void PhysicsUpdate()
    {
    }
    public override void OnExit()
    {
    }
}
