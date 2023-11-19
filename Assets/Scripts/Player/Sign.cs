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
    public IInteractable targetItem;
    public bool canPress;

    void Awake()
    {
        playerInput = new PlayerInputController();
        playerInput.Enable();
    }

    void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        playerInput.GamePlay.Confirm.started += OnConfirm;
    }


    void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;

        signSprite.transform.localScale = playerTrans.localScale * 0.05f;

    }
    private void OnConfirm(InputAction.CallbackContext obj)
    {
        if (canPress)
        {
            targetItem.TriggerAction();
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }

    //根據inputsystem來判斷當前的輸入設備為何
    private void OnActionChange(object obj, InputActionChange actionChange)
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
            targetItem = other.GetComponent<IInteractable>();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        canPress = false;
    }
}
