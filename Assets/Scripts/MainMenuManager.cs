using System;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scenes")] 
    [SerializeField] [Scene] private string introScene;
    [SerializeField] [Scene] private string gameScene;
    [SerializeField] [Scene] private string aboutScene;
    [SerializeField] private float sceneTransitionDuration;
    
    [Header("Components")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Image fade;
    private float volume;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(delegate { AdjustVolume(); });
    }

    public void GoToIntroScene()
    {
        fade.DOKill();
        fade.DOFade(0f, 0f);
        fade.DOFade(1f, sceneTransitionDuration).OnComplete(() =>
        {
            SceneManager.LoadScene(introScene);
        });
    }
    
    public void GoToGameScene()
    {
        SceneManager.LoadScene(gameScene);
    }
    
    public void GoToAboutScene()
    {
        SceneManager.LoadScene(aboutScene);
    }

    private void AdjustVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }
}
