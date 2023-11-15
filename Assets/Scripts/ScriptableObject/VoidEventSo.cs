using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/VoidEventSo")]
public class VoidEventSo : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}

