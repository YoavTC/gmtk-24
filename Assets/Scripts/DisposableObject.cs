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
	
	public void GetDisposed() 
	{
		GameManager.Instance.ObjectDisposed(this);
		Destroy(gameObject);
	}
}