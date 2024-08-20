using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomUnityEvent : UnityEvent
{
    private bool hasBeenInvoked = false;

    public bool HasBeenInvoked
    {
        get { return hasBeenInvoked; }
    }

    public new void Invoke()
    {
        base.Invoke();
        hasBeenInvoked = true;
    }

    public void ResetInvocation()
    {
        hasBeenInvoked = false;
    }
}