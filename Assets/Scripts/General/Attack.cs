using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("傷害資訊")]
    public float damage;
    public float attackRange;
    public float attackRate;

    void OnTriggerStay2D(Collider2D other)
    {
        //產生接觸後，將自己本身的傷害資訊傳遞給對方
        other.GetComponent<Character>()?.TakeDamage(this);
    }
}
