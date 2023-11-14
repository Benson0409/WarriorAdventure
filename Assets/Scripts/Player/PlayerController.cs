using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider2D;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    public PlayerInputController playerInputController;
    public Vector2 inputDirection;

    //鍵盤輸入線性控制判斷
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    private float smoothInputSpeed = 0.2f;

    [Header("人物操控變量")]
    private int speed;
    public int moveSpeed;
    public int crouchSpeed;
    public int slideSpeed;
    public float slideJumpForce;
    public float jumpForce;
    public bool isCrouch;
    public bool isSlide;
    public bool isDead;
    private Vector2 initialColliderOffest;
    private Vector2 initialColliderSize;
    [Header("牆壁狀態")]
    public float jumpWallForce;
    //可以進行蹬牆跳
    public bool isJumpWall;
    private float gravityStore;
    public float jumpWallTime;
    private float jumpWallTimeCounter = 0;

    [Header("傷害擊退")]
    public float hurtForce;
    public bool isHurt;

    [Header("攻擊")]
    public bool isAttack;

    [Header("Physics Material")]
    public PhysicsMaterial2D nomal;
    public PhysicsMaterial2D wall;
    public PhysicsMaterial2D jumpWall;
    void Awake()
    {

        //套件讀取
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerInputController = new PlayerInputController();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();

        //紀錄碰撞器位置
        initialColliderOffest = capsuleCollider2D.offset;
        initialColliderSize = capsuleCollider2D.size;

        //跳躍判斷
        playerInputController.GamePlay.Jump.started += Jump;
        //攻擊判斷
        playerInputController.GamePlay.Attack.started += Attack;

        gravityStore = rb.gravityScale;
    }
    void OnEnable()
    {
        playerInputController.Enable();
    }

    void OnDisable()
    {
        playerInputController.Disable();
    }

    void Update()
    {
        inputDirection = playerInputController.GamePlay.Move.ReadValue<Vector2>();
        //判斷物理材質
        CheckState();
        JumpWall();
        //鍵盤輸入線性判斷
        //讓currentInputVector 在指定時間內過度到 inputDirection的值
        //smoothInputVelocity 存儲過渡過程中的速度
        currentInputVector = Vector2.SmoothDamp(currentInputVector, inputDirection, ref smoothInputVelocity, smoothInputSpeed);
    }

    void FixedUpdate()
    {
        if (!isHurt && !isAttack)
        {
            Slide();
            Crouch();
            Move();
        }
    }

    //移動
    public void Move()
    {

        if (isCrouch)
        {
            speed = crouchSpeed;
        }

        else if (isSlide)
        {
            speed = slideSpeed;
        }

        else
        {
            speed = moveSpeed;
        }

        //如果鍵盤沒有被觸發，就將速度歸零
        if (!isJumpWall)
        {
            if (MathF.Abs(inputDirection.x) > 0)
            {
                rb.velocity = new Vector2(currentInputVector.x * speed * Time.deltaTime, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        //人物翻轉
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
        {
            faceDir = 1;
        }
        if (inputDirection.x < 0)
        {
            faceDir = -1;
        }
        transform.localScale = new Vector3(faceDir, 1, 1);

    }

    //跳躍 每觸發一次跳躍鍵就會觸發一次
    private void Jump(InputAction.CallbackContext context)
    {

        if (physicsCheck.isGround)
        {
            //滑行時跳躍可以跳得更高
            if (isSlide)
            {
                rb.AddForce(slideJumpForce * transform.up, ForceMode2D.Impulse);
                return;
            }

            rb.AddForce(jumpForce * transform.up, ForceMode2D.Impulse);
        }

        //執行向反方向跳躍並解處爬牆狀態
        if (isJumpWall && jumpWallTimeCounter <= 0)
        {
            print("蹬牆跳");
            //避免連跳產生
            jumpWallTimeCounter = jumpWallTime;

            //給予一個反方向的力 並解除爬牆狀態
            rb.AddForce(jumpWallForce * new Vector2(-transform.localScale.x * 2, 2), ForceMode2D.Impulse);
            //rb.velocity = new Vector2(-transform.localScale.x * 10, rb.velocity.y);
            rb.gravityScale = gravityStore;
            isJumpWall = false;
        }
    }

    //蹬牆跳
    public void JumpWall()
    {
        //碰觸地面解除爬牆狀態
        if (physicsCheck.isGround)
        {
            isJumpWall = false;
            rb.gravityScale = gravityStore;
        }

        //判斷是否可以進行蹬牆跳
        //有接觸到牆壁＆不在地面上
        if (!physicsCheck.isGround)
        {
            if ((inputDirection.x > 0.1f && physicsCheck.isRightWall) || (inputDirection.x < -0.1f && physicsCheck.isLeftWall))
            {
                //計時器倒數
                jumpWallTimeCounter -= Time.deltaTime;

                isJumpWall = true;
                //rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.gravityScale = 1.5f;
                print("can jump");
            }
            //沒有持續按壓就解除狀態
            else
            {
                isJumpWall = false;
                rb.gravityScale = gravityStore;
            }
        }

    }

    //攻擊
    private void Attack(InputAction.CallbackContext context)
    {
        isAttack = true;
        playerAnimation.Attack();
    }

    public void Slide()
    {
        //有左右的位移速度
        if (Mathf.Abs(rb.velocity.x) > 0.1 && inputDirection.y < 0 && !isCrouch)
        {
            isSlide = true;
            capsuleCollider2D.offset = new Vector2(0.1f, 0.5f);
            capsuleCollider2D.size = new Vector2(1f, 1f);
        }
        else
        {
            isSlide = false;
            capsuleCollider2D.offset = initialColliderOffest;
            capsuleCollider2D.size = initialColliderSize;
        }
    }

    //蹲下
    private void Crouch()
    {
        //按下Ｃ鍵，執行下蹲，並改變碰撞器大小
        if (inputDirection.y < 0 && !isSlide)
        {
            isCrouch = true;
            capsuleCollider2D.offset = new Vector2(capsuleCollider2D.offset.x, 0.7f);
            capsuleCollider2D.size = new Vector2(capsuleCollider2D.size.x, 1.4f);
        }

        else
        {
            isCrouch = false;
            capsuleCollider2D.offset = initialColliderOffest;
            capsuleCollider2D.size = initialColliderSize;
        }
    }

    private void CheckState()
    {
        // if (isJumpWall)
        // {
        //     capsuleCollider2D.sharedMaterial = jumpWall;
        //     return;
        // }
        capsuleCollider2D.sharedMaterial = physicsCheck.isGround ? nomal : wall;
    }
    //受傷
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    //死亡
    public void OnDead()
    {
        isDead = true;
        playerInputController.GamePlay.Disable();
    }
}
