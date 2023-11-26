using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    [Header("事件監聽")]
    public CharacterEventSo healthEvent;
    public SceneLoadEventSo loadEvent;

    void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        loadEvent.LoadRequestEvent += OnLoadEvent;
    }



    void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        loadEvent.LoadRequestEvent -= OnLoadEvent;
    }

    private void OnHealthEvent(Character character)
    {
        var persentage = character.currentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(persentage);
    }
    private void OnLoadEvent(GameSceneSo sceneToLoad, Vector3 arg1, bool arg2)
    {
        //如果當前場景是Menu的話就取消顯示人物狀態欄
        var isMenu = sceneToLoad.sceneType == Scenetype.menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }
}
