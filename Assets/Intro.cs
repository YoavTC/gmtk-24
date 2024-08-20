using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Intro : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private UnityEvent aliuaosiuhdaps;

    // Update is called once per frame
    private void Start()
    {
        playableDirector.stopped += PlayableDirectorOnstopped;
    }

    private void PlayableDirectorOnstopped(PlayableDirector obj)
    {
        aliuaosiuhdaps?.Invoke();
    }
}
