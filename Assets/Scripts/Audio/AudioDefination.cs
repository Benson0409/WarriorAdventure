using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    public PlayAudioEventSo playAudioEventSo;
    public AudioClip audioClip;
    public bool playOnable;

    void OnEnable()
    {
        if (playOnable)
        {
            PlayAudioClip();
        }
    }

    public void PlayAudioClip()
    {
        playAudioEventSo.RaiseEvent(audioClip);
    }
}
