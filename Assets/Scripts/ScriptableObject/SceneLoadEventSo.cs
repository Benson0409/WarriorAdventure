using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSo")]
public class SceneLoadEventSo : ScriptableObject
{

    public UnityAction<GameSceneSo, Vector3, bool> LoadRequestEvent;

    /// <summary>
    /// 場景載入需求
    /// </summary>
    /// <param name="locationToLoad">需要加載的場景</param>
    /// <param name="posToGo">玩家要移動的位置</param>
    /// <param name="fadeScene">是否需要淡入淡出</param> 
    public void RaiseLoadRequestEvent(GameSceneSo locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(locationToLoad, posToGo, fadeScreen);
    }
}

