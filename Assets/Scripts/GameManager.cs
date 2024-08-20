using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

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
	
	private int barTargetWeight, barTargetWealth;
	
	[Header("Components")]
	[SerializeField] private Slider moneyBar;
	[SerializeField] private Slider weightBar;
	[SerializeField] private float barLerpSpeed;
	
	private void Start()
	{
		GetWeightAndMoneyValues();
		
		moneyBar.maxValue = shipWealth;
		moneyBar.value = 0;
		
		weightBar.maxValue = shipWeight;
		weightBar.value = 0;
	}
	
	private void Update() 
	{
		int moneyValue = (int) moneyBar.value;
		int weightValue = (int) weightBar.value;
		
		if (moneyValue != barTargetWealth) 
		{
			moneyBar.value = Mathf.Lerp(moneyValue, barTargetWealth, barLerpSpeed * Time.deltaTime);
			if (moneyValue == barTargetWealth - 1) moneyBar.value = barTargetWealth;
		}
		if (weightValue != barTargetWeight) 
		{
			weightBar.value = Mathf.Lerp(weightValue, barTargetWeight, barLerpSpeed * Time.deltaTime);
			if (weightValue == barTargetWeight - 1) weightBar.value = barTargetWeight;
		}
	}
	
	public void ObjectDisposed(DisposableObject disposedObject) 
	{
		disposedMoney += disposedObject._price;
		disposedWeight += disposedObject._weight;
		
		UpdateBars();
	}
	
	private void UpdateBars() 
	{
		barTargetWealth = disposedMoney;
		barTargetWeight = disposedWeight;
		//moneyBar.value = disposedMoney;
		//weightBar.value = disposedWeight;
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

	public void THATSIT()
	{
		int score = shipWealth - disposedMoney;
		PlayerPrefs.SetInt("score", score);
		SceneManager.LoadScene("Outro");
	}
}
