using UnityEngine.Events;
using UnityEngine;


[CreateAssetMenu(menuName = "Event/PlayAudioEventSo")]
public class PlayAudioEventSo : ScriptableObject
{
    public UnityAction<AudioClip> OnEventRaised;

    public void RaiseEvent(AudioClip audioClip)
    {
        OnEventRaised?.Invoke(audioClip);
    }
}
