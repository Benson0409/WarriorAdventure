using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PhysicsCheck physicsCheck;

    [HideInInspector] public Transform attacker;

    [Header("基本資訊")]
    public int normalSpeed;
    public int chaseSpeed;
    public int hideSpeed;
    public int attackSpeed;
    public int currentSpeed;
    public float hurtForce;
    public float hurtTime;



    [Header("狀態")]
    public bool isChase;

    public bool isHurt;
    public bool isDead;
    //面朝方向
    public Vector3 faceDir;
    [Header("玩家偵測")]
    public LayerMask attackerLayer;
    public float checkDistance;
    public Vector2 centerOffest;
    public Vector2 checkSize;

    [Header("等待時間計時器")]
    public float waitTime;
    [HideInInspector] public float waitTimeCounter;
    public bool isWait;
    [Header("丟失敵人計時器")]
    public float lostTime;
    public float lostTimeCounter;


    //設立各種敵人的狀態
    protected BasicState currentState;
    protected BasicState patrolState;
    protected BasicState chaseState;
    protected BasicState hideState;
    protected BasicState recoverState;
    protected BasicState attackState;
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;

    }
    void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }
    void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        currentState.LogicUpdate();

        TimeCounter();
    }

    void FixedUpdate()
    {
        if (!isHurt && !isDead && !isWait)
        {
            Move();
        }
        currentState.PhysicsUpdate();
    }
    void OnDisable()
    {
        currentState.OnExit();
    }

    //virtual 讓他可以更改
    public virtual void Move()
    {
        rb.velocity = new Vector2(faceDir.x * currentSpeed * Time.deltaTime, rb.velocity.y);
    }


    public virtual void TimeCounter()
    {
        if (isWait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                waitTimeCounter = waitTime;
                isWait = false;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }

        if (!FoundPlayer() && lostTimeCounter > 0)
        {
            lostTimeCounter -= Time.deltaTime;
        }
    }


    //尋找玩家,回傳布林值,再根據不同的敵人狀態做改變
    public bool FoundPlayer()
    {
        if (this.gameObject.name == "Bee")
        {
            return Physics2D.OverlapCircle(transform.position, checkDistance, attackerLayer);
        }
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffest, checkSize, 0, faceDir, checkDistance, attackerLayer);
    }

    //狀態切換
    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Hide => hideState,
            NPCState.Recover => recoverState,
            NPCState.Attack => attackState,
            _ => null
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }
    // Event 事件
    //受傷判斷
    public void OnTakeDamage(Transform attackTrans)
    {

        attacker = attackTrans;

        //面向玩家
        //玩家在右邊
        if (attacker.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        //玩家在左邊
        if (attacker.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        //受傷擊退
        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attacker.transform.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(onHurt(dir));
    }

    //死亡判斷
    public void OnDead()
    {
        isDead = true;
        gameObject.layer = 2;
        anim.SetBool("dead", isDead);
    }
    //破壞物體
    public void DestroyObjectAnimation()
    {
        Destroy(this.gameObject);
    }

    IEnumerator onHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(hurtTime);
        isHurt = false;

    }

    private void OnDrawGizmosSelected()
    {
        if (this.gameObject.name == "Bee")
        {
            Gizmos.DrawWireSphere(transform.position, checkDistance);
        }
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffest + new Vector3(checkDistance * -transform.localScale.x, 0), 0.2f);
    }

}
