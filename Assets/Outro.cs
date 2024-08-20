using System;
using TMPro;
using UnityEngine;

public class Outro : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;
    private int score;

    private void Start()
    {
        score = PlayerPrefs.GetInt("score");
        displayText.text += score.ToString();
    }
}
