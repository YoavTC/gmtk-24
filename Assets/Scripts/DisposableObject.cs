using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class DisposableObject : MonoBehaviour
{
    [Range(0, 1000)]
    [SerializeField] private int weight;
    [Range(0, 100000)]
    [SerializeField] private int price;
	
	public int _price 
	{
		get { return price; }
	}
	
	public int _weight 
	{
		get { return weight; }
	}
	
	[SerializeField] [ReadOnly] private bool isDisposable;
	
	public UnityEvent OnGrabbedEvent;
	public UnityEvent OnReleasedEvent;
	
	public void GetGrabbed() 
	{
		OnGrabbedEvent?.Invoke();
	}
	
	public void GetReleased() 
	{
		OnReleasedEvent?.Invoke();
	}
	
	private void GetDisposed() 
	{
		GameManager.Instance.ObjectDisposed(this);
		Destroy(gameObject);
	}
	
	private void Update() 
	{
		float y = transform.position.y;
		if (isDisposable) 
		{
			if (y <= -50f)  GetDisposed();
		} else if (y >= 0f) isDisposable = true;
	}
}