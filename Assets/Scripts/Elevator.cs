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
	
	[SerializeField] [ReadOnly] private bool recursionDone;
	
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
			StartCoroutine(UseElevator());
			canUse = false;
		}
	}
	
	private IEnumerator UseElevator() 
	{
		Transform playerTransform = playerController.transform;
		Grab[] hands = playerController.GetComponentsInChildren<Grab>();
		
		ChangeRigidBody2DBodyTpe(playerTransform, RigidbodyType2D.Kinematic);
		
		playerTransform.position = elevatorPosition;
		
		foreach (Grab hand in hands)
		{
			if (hand.GetComponent<FixedJoint2D>()) 
			{
				Transform objectTransform = hand.GetComponent<FixedJoint2D>().connectedBody.transform;
				
				ChangeRigidBody2DBodyTpe(objectTransform.root, RigidbodyType2D.Kinematic);
				
				//Vector2 objectOffset =  hand.transform.position - objectTransform.position;
				//Vector2 elevatorOffset = transform.position - elevatorPosition;
				//Vector2 targetPosition = hand.transform.position + (Vector3) objectOffset + (Vector3) elevatorOffset;
				
				ToggleComponentsRecursively(objectTransform.root, false, hand.transform.position);
				
				yield return new WaitUntil(() => recursionDone);
				
				//objectTransform.position = targetPosition;
				
				ToggleComponentsRecursively(objectTransform.root, true);
				yield return new WaitUntil(() => recursionDone);
				ChangeRigidBody2DBodyTpe(objectTransform.root, RigidbodyType2D.Dynamic);
				
				break;
			}
		}
		
		ChangeRigidBody2DBodyTpe(playerTransform, RigidbodyType2D.Dynamic);
	}
	
	private void ChangeRigidBody2DBodyTpe(Transform targetRb, RigidbodyType2D bodyType)
	{
		Debug.Log("Changing " + targetRb + " to: " + bodyType);
		foreach (Rigidbody2D rb in targetRb.GetComponentsInChildren<Rigidbody2D>()) 
		{
			rb.bodyType = bodyType;
		}
	}
	
	private void ToggleComponentsRecursively(Transform targetTransform, bool enable)
	{
		recursionDone = false;
		foreach (Transform child in targetTransform)
		{
			foreach (var component in child.GetComponents<Component>())
			{
				if (component is Behaviour behaviour)
				{
					behaviour.enabled = enable;
				}
				else if (component is Renderer renderer)
				{
					renderer.enabled = enable;
				}
				// Add more component types if necessary
			}

			// Recursive call for any children of this child
			if (child.childCount == 0) 
			{
				Debug.Log("recursionDone");
				recursionDone = true;
			}
			ToggleComponentsRecursively(child, enable);
		}
	}
	
	private void ToggleComponentsRecursively(Transform targetTransform, bool enable, Vector3 handPosition)
	{
		recursionDone = false;
		foreach (Transform child in targetTransform)
		{
			Vector2 objectOffset =  handPosition - targetTransform.position;
			Vector2 elevatorOffset = targetTransform.position - elevatorPosition;
			Vector2 targetPosition = handPosition + (Vector3) objectOffset + (Vector3) elevatorOffset;
			
			foreach (var component in child.GetComponents<Component>())
			{
				if (component is Behaviour behaviour)
				{
					behaviour.enabled = enable;
				}
				else if (component is Renderer renderer)
				{
					renderer.enabled = enable;
				}
				// Add more component types if necessary
			}
			
			child.position = targetPosition;

			// Recursive call for any children of this child
			if (child.childCount == 0) {recursionDone = true; Debug.Log("recursionDone");}
			ToggleComponentsRecursively(child, enable);
		}
	}
}
