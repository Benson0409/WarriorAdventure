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
    public float jumpForce;
    public bool isCrouch;
    public bool isDead;
    private Vector2 initialColliderOffest;
    private Vector2 initialColliderSize;

    [Header("傷害擊退")]
    public float hurtForce;
    public bool isHurt;
    [Header("攻擊")]
    public bool isAttack;

    [Header("Physics Material")]
    public PhysicsMaterial2D nomal;
    public PhysicsMaterial2D wall;
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

        //鍵盤輸入線性判斷
        //讓currentInputVector 在指定時間內過度到 inputDirection的值
        //smoothInputVelocity 存儲過渡過程中的速度
        currentInputVector = Vector2.SmoothDamp(currentInputVector, inputDirection, ref smoothInputVelocity, smoothInputSpeed);
    }

    void FixedUpdate()
    {
        if (!isHurt && !isAttack)
        {
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
        else
        {
            speed = moveSpeed;
        }
        //如果鍵盤沒有被觸發，就將速度歸零
        if (MathF.Abs(inputDirection.x) > 0)
        {
            rb.velocity = new Vector2(currentInputVector.x * speed * Time.deltaTime, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
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

    //跳躍
    private void Jump(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGround)
        {
            rb.AddForce(jumpForce * transform.up, ForceMode2D.Impulse);
        }
    }
    //攻擊
    private void Attack(InputAction.CallbackContext context)
    {
        isAttack = true;
        playerAnimation.Attack();
    }

    //蹲下
    private void Crouch()
    {
        //按下Ｃ鍵，執行下蹲，並改變碰撞器大小
        if (inputDirection.y < 0)
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
