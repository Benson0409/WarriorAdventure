using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

public class Bee : Enemy
{
    [Header("玩家位置")]
    public GameObject player;
    [Header("蜜蜂位置判斷")]
    private Vector2 initialPosition;
    public bool isHere;
    public bool isGroud;
    public Vector2 targetPosition;
    protected override void Awake()
    {
        base.Awake();
        patrolState = new BeePatrolState();
        chaseState = new BeeChaseState();
        attackState = new BeeAttackState();

        initialPosition = transform.position;
        targetPosition = RandomBeeTargetPosition();
    }

    public override void Move()
    {
        if (currentState == attackState)
        {
            print("攻擊");
        }


        if (currentState == chaseState)
        {
            print("追擊");
        }

        if (currentState == patrolState)
        {
            print("巡邏");
        }

        if (currentState == chaseState || currentState == attackState)
        {
            //追擊玩家，以動到玩家身邊
            targetPosition = player.transform.position;

            //改變蜜蜂的面朝方向
            if (targetPosition.x - transform.position.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (targetPosition.x - transform.position.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            if (Vector2.Distance(transform.position, player.transform.position) < 2)
            {
                //追到敵人
                isChase = true;
                return;
            }
            else
            {
                isChase = false;
            }
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, normalSpeed * Time.deltaTime);
            return;
        }

        if (physicsCheck.isGround && !isGroud)
        {
            print("碰地");
            isGroud = true;
            return;
        }

        if (!physicsCheck.isGround)
        {
            isGroud = false;
        }

        if ((Vector2)transform.position == targetPosition)
        {
            isHere = true;
            return;
        }

        //改變蜜蜂的面朝方向
        if (targetPosition.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (targetPosition.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, normalSpeed * Time.deltaTime);
    }

    public override void TimeCounter()
    {
        //撞到牆蜜蜂轉向
        if (isWait || isHere || isGroud)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                waitTimeCounter = waitTime;
                isWait = false;
                isHere = false;
                targetPosition = RandomBeeTargetPosition();
            }
        }
    }

    Vector2 RandomBeeTargetPosition()
    {
        float randomX = Random.Range(-6, 6);
        float randomY = Random.Range(-6, 6);
        targetPosition = new Vector2(transform.position.x + randomX, transform.position.y + randomY);
        //在範圍內的目標點才設為目標點
        while (targetPosition.x > initialPosition.x + 8 || targetPosition.x < initialPosition.x - 8 || targetPosition.y > initialPosition.y + 4)
        {
            //在目標物的４＊４範圍內隨機移動
            randomX = Random.Range(-6, 6);
            randomY = Random.Range(-6, 6);
            targetPosition = new Vector2(transform.position.x + randomX, transform.position.y + randomY);
        }
        return targetPosition;
    }
}
