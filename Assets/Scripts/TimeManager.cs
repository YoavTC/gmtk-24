using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

public class TimeManager : MonoBehaviour
{
	[Header("Settings")]
    [SerializeField] private int totalTime;
	[SerializeField] private float remainingTime;
	private bool gameActive;

	[Header("Events")]
	[SerializedDictionary("Time %", "Event")]
	public SerializedDictionary<float, CustomUnityEvent> timeEvents;
	
	private void Start()
	{
		remainingTime = totalTime;
		Time.timeScale = 0f;
	}
	
	private void Update()
	{
		if (!gameActive) return;
		CallEvents();
		remainingTime -= Time.deltaTime;
	}
	
	private void CallEvents()
	{
		float timeProgress = remainingTime / totalTime;
		foreach (KeyValuePair<float, CustomUnityEvent> timeEvent in timeEvents)
		{
			if (timeProgress <= timeEvent.Key && !timeEvent.Value.HasBeenInvoked) 
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
