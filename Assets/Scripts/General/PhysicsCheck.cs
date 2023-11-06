using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;

    [Header("碰撞器自動對齊設定")]
    public bool manaul;

    [Header("參數變量")]
    public float checkRadius;
    public LayerMask groundLayer;
    public Vector2 buttomOffset;
    public Vector2 rightOffest;
    public Vector2 leftOffest;

    [Header("狀態")]
    public bool isGround;
    public bool isLeftWall;
    public bool isRightWall;

    void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        if (!manaul)
        {
            leftOffest = new Vector2((coll.offset.x + coll.bounds.size.x) / 2, coll.bounds.size.y / 2);
            rightOffest = new Vector2(-leftOffest.x, leftOffest.y);
        }
    }
    void Update()
    {
        Check();
    }

    private void Check()
    {
        //判斷是否在地面
        //isGround = Physics2D.OverlapCircle((Vector2)transform.position + buttomOffset, checkRadius, groundLayer);
        //根據物體面向方向的不同，來更新偵測點的位置
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(buttomOffset.x * transform.localScale.x, buttomOffset.y), checkRadius, groundLayer);
        isLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffest, checkRadius, groundLayer);
        isRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffest, checkRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(buttomOffset.x * transform.localScale.x, buttomOffset.y), checkRadius);
        //Gizmos.DrawWireSphere((Vector2)transform.position + buttomOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffest, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffest, checkRadius);
    }
}
