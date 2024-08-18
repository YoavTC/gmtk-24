using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

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
		foreach (Rigidbody2D rb in playerController.transform.GetComponentsInChildren<Rigidbody2D>()) 
		{
			rb.isKinematic = true;
		}
		
		playerController.transform.position = linkedElevator.transform.position;
		
		//foreach (Transform limb in playerController.GetComponentsInChildren<Transform>()) 
		//{
		//	limb.localPosition += playerController.transform.position;
		//}
		
		foreach (Rigidbody2D rb in playerController.transform.GetComponentsInChildren<Rigidbody2D>()) 
		{
			rb.isKinematic = false;
		}
	}
}
