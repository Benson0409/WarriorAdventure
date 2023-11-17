using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
public class Sign : MonoBehaviour
{
    public PlayerInputController playerInput;
    public Transform playerTrans;
    public GameObject signSprite;
    public bool canPress;

    void Awake()
    {
        playerInput = new PlayerInputController();
    }

    void OnEnable()
    {
        InputSystem.onActionChange += ActionChange;
    }


    void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;

        signSprite.transform.localScale = playerTrans.localScale * 0.05f;

    }

    //根據inputsystem來判斷當前的輸入設備為何
    private void ActionChange(object obj, InputActionChange actionChange)
    {
        if (actionChange == InputActionChange.ActionStarted)
        {
            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case Keyboard:
                    //顯示鍵盤按壓提示
                    print("目前使用鍵盤");
                    break;
                case XboxGamepadMacOS:
                    //顯示搖桿按壓提示
                    print("目前使用搖桿");
                    break;
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            canPress = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        canPress = false;
    }
}
