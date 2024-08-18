using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;


public class Elevator : MonoBehaviour
{
	[SerializeField] private float promptBobSpeed;
	[SerializeField] private float promptBobDistance;
    private Transform prompt;
	
	private PlayerMovementController playerController;
	[SerializeField] private Elevator linkedElevator;
	[SerializeField] [ReadOnly] private bool canUse;
	
    void Start()
    {
		playerController = FindFirstObjectByType<PlayerMovementController>();
		
        prompt = transform.GetChild(0);
		prompt.DOMoveY(prompt.position.y + promptBobDistance, promptBobSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
		prompt.gameObject.SetActive(false);
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
		Vector3 elevatorPosition = linkedElevator.transform.position;
		Transform playerTransform = playerController.transform;
		
		foreach (Rigidbody2D rb in playerController.transform.GetComponentsInChildren<Rigidbody2D>()) 
		{
			//rb.isKinematic = true;
			rb.bodyType = RigidbodyType2D.Kinematic;
		}
		
		Grab[] hands = playerController.GetComponentsInChildren<Grab>();
		
		playerTransform.position = elevatorPosition;
		
		foreach (Grab hand in hands)
		{
			if (hand.GetComponent<FixedJoint2D>()) 
			{
				Transform objectTransform = hand.GetComponent<FixedJoint2D>().connectedBody.transform;
				
				Vector2 objectOffset =  hand.transform.position - objectTransform.position;
				Vector2 elevatorOffset = transform.position - elevatorPosition;
				objectTransform.position = hand.transform.position + (Vector3) objectOffset + (Vector3) elevatorOffset;
				break;
			}
		}
		
		foreach (Rigidbody2D rb in playerTransform.GetComponentsInChildren<Rigidbody2D>()) 
		{
			rb.bodyType = RigidbodyType2D.Dynamic;
			//rb.isKinematic = false;
		}
	}
}
