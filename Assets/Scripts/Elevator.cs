using UnityEngine;
using DG.Tweening;

public class Elevator : MonoBehaviour
{
	[SerializeField] private float promptBobSpeed;
	[SerializeField] private float promptBobDistance;
    private Transform prompt;
	
    void Start()
    {
        prompt = transform.GetChild(0);
		prompt.DOMoveY(prompt.position.y + promptBobDistance, promptBobSpeed).SetEase(Ease.InOutSine).SetLoops(-1);
    }
	
	private void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.CompareTag("Player")) 
		{
			prompt.gameObject.SetActive(true);
		}
	}
}
