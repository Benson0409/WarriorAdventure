using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/FadeEventSo")]
public class FadeEventSo : ScriptableObject
{

    //註冊一個事件
    public UnityAction<Color, float, bool> OnEventRaised;

    /// <summary>
    /// 場景淡入
    /// </summary>
    /// <param name="fadeDuration"></param>
    public void FadeIn(float fadeDuration)
    {
        RaisedEvent(Color.black, fadeDuration, true);
    }

    /// <summary>
    /// 場景淡出
    /// </summary>
    /// <param name="fadeDuration"></param>
    public void FadeOut(float fadeDuration)
    {
        RaisedEvent(Color.clear, fadeDuration, false);
    }

    public void RaisedEvent(Color target, float fadeDuration, bool fadeIn)
    {
        OnEventRaised?.Invoke(target, fadeDuration, fadeIn);
    }
}

