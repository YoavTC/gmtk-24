using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Outro : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private Image guy;
    
    private int score;

    private void Start()
    {
        score = PlayerPrefs.GetInt("score");
        displayText.text += score.ToString();

        guy.transform.DOScale(guy.transform.localScale * 1.2f, .2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        displayText.transform.DOScale(displayText.transform.localScale * 1.3f, 2f);
        displayText.DOFade(1f, 2f);
    }
}
