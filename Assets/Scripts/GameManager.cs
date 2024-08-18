using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class GameManager : Singleton<GameManager>
{
	[Header("Ship Values")]
	[SerializeField] private int shipWealth;
	[SerializeField] private int shipWeight;
	
	[Header("Information")]
	[ProgressBar("Disposed Money", "shipWealth", EColor.Yellow)]
	[SerializeField] private int disposedMoney;
	[ProgressBar("Disposed Weight", "shipWeight", EColor.Green)]
	[SerializeField] private int disposedWeight;
	
	[Header("Components")]
	[SerializeField] private Slider moneyBar;
	[SerializeField] private Slider weightBar;
	
	private void Start()
	{
		GetWeightAndMoneyValues();
		
		moneyBar.maxValue = shipWealth;
		moneyBar.value = 0f;
		
		weightBar.maxValue = shipWeight;
		weightBar.value = 0f;
	}
	
	public void ObjectDisposed(DisposableObject disposedObject) 
	{
		disposedMoney += disposedObject._price;
		disposedWeight += disposedObject._weight;
		
		UpdateBars();
	}
	
	private void UpdateBars() 
	{
		moneyBar.value = disposedMoney;
		weightBar.value = disposedWeight;
	}
	
	private void GetWeightAndMoneyValues() 
	{
		DisposableObject[] disposableObjects = FindObjectsByType<DisposableObject>(FindObjectsSortMode.None);
		
		shipWealth = 0;
		shipWeight = 0;
		
		for (int i = 0; i < disposableObjects.Length; i++) 
		{
			Debug.Log("Adding " + disposableObjects[i] + " To the sum of ship wealth & weight!", disposableObjects[i]);
			shipWealth += disposableObjects[i]._price;
			shipWeight += disposableObjects[i]._weight;
		}
	}
}
