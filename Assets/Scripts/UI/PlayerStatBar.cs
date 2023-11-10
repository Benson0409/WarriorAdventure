using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStatBar : MonoBehaviour
{
    public Image healthBarGreen;
    public Image healthBarRed;
    public Image powerBarYellow;


    void Update()
    {
        if (healthBarRed.fillAmount > healthBarGreen.fillAmount)
        {
            healthBarRed.fillAmount -= Time.deltaTime;
        }
    }
    //讀取血量百分比 改變血量變化
    public void OnHealthChange(float persentage)
    {
        healthBarGreen.fillAmount = persentage;
    }
}
