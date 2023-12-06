using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public GameObject neGameBtn;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(neGameBtn);
    }

    public void ExitGame()
    {
        print("關閉遊戲");
        Application.Quit();
    }
}
