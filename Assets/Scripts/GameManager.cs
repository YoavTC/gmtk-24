using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[SerializeField] private int disposedMoney;
	[SerializeField] private int disposedWeight;
	
	public void ObjectDisposed(DisposableObject disposedObject) 
	{
		disposedMoney += disposedObject._price;
		disposedWeight += disposedObject._weight;
	}
}
