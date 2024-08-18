using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using System.Collections;
using NaughtyAttributes;

public class TransitionManager : Singleton<TransitionManager>
{
	[Header("Settings")]
	[SerializeField] private float defaultFadeTransitionTime;
	[SerializeField] private float defaultDoorTransitionTime, defaultDoorClosedTime;
	[SerializeField] private Ease easeType;
	
	[Header("Components")]
	[SerializeField] private Image blackScreen;
	[SerializeField] private RectTransform leftDoor, rightDoor;
	
	private void Start()
	{
		blackScreen.gameObject.SetActive(false);
	}
	
	[Button]
	public void TestFade() 
	{
		FadeTransition(1f, defaultFadeTransitionTime, (() => {
			Debug.Log("Callback!");
		}));
	}
	
	[Button]
	public void TestDoors()
	{
		ElevatorDoorTransition(defaultDoorClosedTime, defaultDoorTransitionTime, (() => {
			Debug.Log("Callback!");
		}));
	}
	
    public void FadeTransition(float targetAlpha, float transitionTime = -1f, Action callback = null)
	{
		blackScreen.gameObject.SetActive(true);
		if (transitionTime == -1) transitionTime = defaultFadeTransitionTime;
		blackScreen.DOFade(targetAlpha, transitionTime).OnComplete(() => 
		{
			if (callback != null) callback?.Invoke();
		});
	}
	
	public void ElevatorDoorTransition(float closedTime = -1f ,float transitionTime = -1f, Action callback = null)
	{
		if (transitionTime == -1) transitionTime = defaultDoorTransitionTime;
		if (closedTime == -1) closedTime = defaultDoorClosedTime;
		StartCoroutine(DoorCoroutine(closedTime, transitionTime, callback));
	}
	
	private IEnumerator DoorCoroutine(float closedTime ,float transitionTime , Action callback = null)
	{
		float leftDoorX = leftDoor.anchoredPosition.x;
		float rightDoorX = rightDoor.anchoredPosition.x;
		
		leftDoor.DOAnchorPosX(0f, transitionTime).SetEase(easeType);
		rightDoor.DOAnchorPosX(0f, transitionTime).SetEase(easeType);
		
		yield return HelperFunctions.GetWait(closedTime + transitionTime);
		
		leftDoor.DOAnchorPosX(leftDoorX, transitionTime).SetEase(easeType);
		rightDoor.DOAnchorPosX(rightDoorX, transitionTime).SetEase(easeType);
		
		yield return HelperFunctions.GetWait(transitionTime);
		
		if (callback != null) callback?.Invoke();
	}
}