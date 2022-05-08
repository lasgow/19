using DG.Tweening;
using ElephantSDK;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using MoreMountains.NiceVibrations;
using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;

public class CompletePanel : MonoBehaviour
{
    public static CompletePanel instance;

    [Header("Props")]
    public Button nextButton;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        transform.localScale = Vector3.zero;

    }

    public void OpenPanel(int levelID)
    {
        Elephant.LevelCompleted((LevelManager.GetLevelID() + 1));
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, (LevelManager.GetLevelID() + 1).ToString());

        nextButton.enabled = true;

        transform.DOScale(Vector3.one, 0.5f).SetDelay(1).SetEase(Ease.OutBack).OnComplete(() => {

        });

        
    }

    public void ClosePanel()
    {
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {

            nextButton.transform.parent.gameObject.SetActive(true);
        });


    }

    public void NextLevelButtonAction()
    {

        nextButton.enabled = false;
    }

    public void LoadNewLevel()
    {
         
        LevelManager.LoadNextLevel();
        ClosePanel();
    }

    
}
