using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Character : MonoBehaviour
{
    [Header("人物資訊")]
    public float maxHealth;
    public float currentHealth;

    [Header("受傷無敵")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;
    public bool defence;

    [Header("事件觸發")]
    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDead;
    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
    }
    void Update()
    {
        //無敵時間倒計時
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            currentHealth = 0;
            OnDead?.Invoke();
            OnHealthChange?.Invoke(this);
        }
    }

    //接受其他人的傷害資訊後，開始判斷當前血量以及無敵狀況
    public void TakeDamage(Attack attacker)
    {
        if (invulnerable || defence)
        {
            return;
        }
        if (currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            //執行受傷事件
            OnTakeDamage?.Invoke(attacker.transform);
            TriggerInvulnerable();
        }
        else
        {
            currentHealth = 0;
            //觸發死亡動畫
            OnDead?.Invoke();
        }
        OnHealthChange?.Invoke(this);
    }

    //無敵狀態觸發
    public void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}
