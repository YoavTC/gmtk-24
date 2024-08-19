using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Collections;


public class Elevator : MonoBehaviour
{
	[SerializeField] private float promptBobSpeed;
	[SerializeField] private float promptBobDistance;
    private Transform prompt;
	
	[SerializeField] [Tag] private string npcTag;
	
	private PlayerMovementController playerController;
	[SerializeField] private Elevator linkedElevator;
	[SerializeField] [ReadOnly] private bool canUse;
	private Vector3 elevatorPosition;

	
    void Start()
    {
		playerController = FindFirstObjectByType<PlayerMovementController>();
		
        prompt = transform.GetChild(0);
		prompt.DOMoveY(prompt.position.y + promptBobDistance, promptBobSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
		prompt.gameObject.SetActive(false);
		
		elevatorPosition = linkedElevator.transform.position;
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
			Debug.Log("Used Elevator");
			UseElevator();
			canUse = false;
		}
	}
	
	private void UseElevator() 
	{
		Transform playerTransform = playerController.transform;
		Grab[] hands = playerController.GetComponentsInChildren<Grab>();
		
		SetRigidbody2DSleepState(playerTransform, true);
		//playerTransform.position = elevatorPosition;
		TeleportPlayer(playerTransform, elevatorPosition);
		SetRigidbody2DSleepState(playerTransform, false);
		

		foreach (Grab hand in hands)
		{
			if (hand.GetComponent<FixedJoint2D>()) 
			{
				Transform objectTransform = hand.GetComponent<FixedJoint2D>().connectedBody.transform;
				
				SetRigidbody2DSleepState(objectTransform.root, true);
				
				Vector2 objectOffset =  hand.transform.position - objectTransform.position;
				Vector2 elevatorOffset = transform.position - elevatorPosition;
				Vector2 targetPosition = hand.transform.position + (Vector3) objectOffset + (Vector3) elevatorOffset;
				
				
				//objectTransform.root.position = targetPosition;
				TeleportPlayer(objectTransform.root, targetPosition);
				
				SetRigidbody2DSleepState(objectTransform.root, false);
				
				break;
			}
		}
	}
	
	private Dictionary<Transform, Vector3> offsets = new Dictionary<Transform, Vector3>();
	
	private void TeleportPlayer(Transform playerTransform ,Vector3 newPosition)
	{
		// Record offsets before moving the parent
		RecordOffsets(playerTransform);

		// Move the parent to the new position
		playerTransform.position = newPosition;

		// Move each child based on its recorded offset
		foreach (var entry in offsets)
		{
			Transform child = entry.Key;
			Vector3 offset = entry.Value;
			child.position = playerTransform.position + offset;
		}
	}
	
	private void RecordOffsets(Transform parent)
	{
		offsets.Clear();  // Clear previous offsets

		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			Vector3 offset = child.position - parent.position;
			offsets[child] = offset;
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
}
