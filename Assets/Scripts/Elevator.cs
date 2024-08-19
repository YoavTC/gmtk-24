using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Collections;


public class Elevator : MonoBehaviour
{
	[Header("Display")]
	[SerializeField] private float promptBobSpeed;
	[SerializeField] private float promptBobDistance;
    private Transform prompt;
	
	[Header("Settings")]
	[SerializeField] [Tag] private string npcTag;
	[SerializeField] private string[] unsafeLayerMasks;
	[SerializeField] private float unsafeCheckRadius;
	
	[Header("Components")]
	[SerializeField] private Elevator linkedElevator;
	[SerializeField] private Transform linkedElevatorObjectExit;
	[SerializeField] [ReadOnly] private bool canUse;
	
	private PlayerMovementController playerController;
	private Vector3 elevatorPosition;
	private LayerMask unsafeLayers;

	
    void Start()
    {
		playerController = FindFirstObjectByType<PlayerMovementController>();
		
        prompt = transform.GetChild(0);
		prompt.DOMoveY(prompt.position.y + promptBobDistance, promptBobSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
		prompt.gameObject.SetActive(false);
		
		elevatorPosition = linkedElevator.transform.position;
		unsafeLayers = LayerMask.GetMask(unsafeLayerMasks);
    }
	
	private void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.transform.root.gameObject.CompareTag("Player")) 
		{
			prompt.gameObject.SetActive(true);
			canUse = true;
		}
	}
	
	private void OnTriggerExit2D(Collider2D other) 
	{
		if (other.transform.root.gameObject.CompareTag("Player"))
		{
			prompt.gameObject.SetActive(false);
			canUse = false;
		}
	}
	
	private void Update()
	{
		if (canUse && Input.GetButtonDown("Use")) 
		{
			UseElevator();
			canUse = false;
		}
	}
	
	private void UseElevator() 
	{
		Transform playerTransform = playerController.transform;
		Grab[] hands = playerController.GetComponentsInChildren<Grab>();
		
		Vector2 safePosition = FindSafePosition(elevatorPosition);
		if (safePosition != Vector2.zero) 
		{
			SetRigidbody2DSleepState(playerTransform, true);
			playerTransform.position = safePosition;
			SetRigidbody2DSleepState(playerTransform, false);
		}
		

		foreach (Grab hand in hands)
		{
			if (hand.GetComponent<FixedJoint2D>()) 
			{
				Transform objectTransform = hand.GetComponent<FixedJoint2D>().connectedBody.transform;
				
				//Vector2 objectOffset =  hand.transform.position - objectTransform.position;
				//Vector2 elevatorOffset = transform.position - elevatorPosition;
				//Vector2 targetPosition = hand.transform.position + (Vector3) objectOffset + (Vector3) elevatorOffset;
				
				//Check position for other objects
				Vector2 objectSafePosition = FindSafePosition(linkedElevatorObjectExit.position);
				if (objectSafePosition != Vector2.zero)
				{
					SetRigidbody2DSleepState(objectTransform.root, true);
					objectTransform.root.position =objectSafePosition;
					SetRigidbody2DSleepState(objectTransform.root, false);
				}
				break;
			}
		}
	}
	
	private void SetRigidbody2DSleepState(Transform targetRb, bool sleep)
	{
		foreach (Rigidbody2D rb in targetRb.GetComponentsInChildren<Rigidbody2D>()) 
		{
			if (sleep) rb.Sleep();
			else rb.WakeUp();
		}
	}
	
	private Vector2 FindSafePosition(Vector2 targetPosition)
	{
		bool safe = false;
		int maxTries = 500;
		int currentTries = 0;
		
		while (!safe)
		{
			if (!Physics2D.OverlapCircle(targetPosition, unsafeCheckRadius ,unsafeLayers))
			{
				Debug.Log("Found safe space on the " + currentTries + " attempt!");
				safe = true;
				return targetPosition;
			}
			
			if (currentTries >= maxTries) 
			{
				Debug.Log("Reached max tries...");
				safe = true;
			}
			
			currentTries++;
			targetPosition.x += 0.1f;
		}
		
		return Vector2.zero;
	}
}
