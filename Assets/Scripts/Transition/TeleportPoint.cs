using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public SceneLoadEventSo loadEventSo;
    public GameSceneSo SceneToGo;
    public Vector3 positionToGo;
    public void TriggerAction()
    {
        print("傳送");
        loadEventSo.RaiseLoadRequestEvent(SceneToGo, positionToGo, true);
    }
}
