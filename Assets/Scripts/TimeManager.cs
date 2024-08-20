using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

public class TimeManager : MonoBehaviour
{
	[Header("Settings")]
    [SerializeField] private int totalTime;
	[SerializeField] private float remainingTime;
	[SerializeField] [ReadOnly] private bool gameActive;
	
	[Header("Components")]
	[SerializeField] private Slider waterBar;

	[Header("Events")]
	[SerializedDictionary("Time %", "Event")]
	public SerializedDictionary<float, CustomUnityEvent> timeEvents;
	
	private void Start()
	{
		remainingTime = totalTime - 0.5f;
		Time.timeScale = 0f;
	}
	
	private void Update()
	{
		if (!gameActive) return;
		
		float timeProgress = remainingTime / totalTime;
		UpdateBar(timeProgress);
		CallEvents(timeProgress);
		
		remainingTime -= Time.deltaTime;
	}
	
	private void UpdateBar(float progress)
	{
		waterBar.value = progress;
	}
	
	private void CallEvents(float progress)
	{
		foreach (KeyValuePair<float, CustomUnityEvent> timeEvent in timeEvents)
		{
			if (progress <= timeEvent.Key && !timeEvent.Value.HasBeenInvoked) 
			{
				timeEvent.Value?.Invoke();
			}
		}
	}
	
	public void GameDone()
	{
		gameActive = false;
	}
	
	public void StartGame()
	{
		gameActive = true;
		Time.timeScale = 1f;
	}
}
