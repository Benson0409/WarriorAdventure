using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{


    //註冊需要的狀態
    protected override void Awake()
    {
        base.Awake();
        patrolState = new SnailPatrolState();
        hideState = new SnailHideState();
        //用來防禦模式結束後進行
        recoverState = new SnailRecoverState();
    }


}
