using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeCanva : MonoBehaviour
{
    [Header("事件監聽")]
    public FadeEventSo fadeEvent;
    public Image fadeImage;

    void OnEnable()
    {
        fadeEvent.OnEventRaised += OnFadeEvent;
    }

    void OnDisable()
    {
        fadeEvent.OnEventRaised -= OnFadeEvent;
    }

    //根據事件的監聽來改變透明度
    private void OnFadeEvent(Color target, float fadeDuration, bool fadeIn)
    {
        fadeImage.DOBlendableColor(target, fadeDuration);
    }
}
