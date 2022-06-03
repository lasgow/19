using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using GameAnalyticsSDK;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    private int reactIndex = 0;

    [Header("GUI")]
    public TextMeshProUGUI levelTxt;
    public GameObject guide;
    [Header("TotalPopulationText")]


    [Header("UI Prefabs")]
    public TextMeshProUGUI collectValue;
    [SerializeField] GameObject cpiBackground;
    [SerializeField] GameObject cpiBackgroundDown;
    [SerializeField] GameObject cpiBackgroundUp;

    [SerializeField] public TextMeshProUGUI cpiValue;
    [SerializeField] public TextMeshProUGUI retentionValue;
    [SerializeField] public TextMeshProUGUI totalDownloadValue;
    [SerializeField] public TextMeshProUGUI playtimeValue;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        GameAnalytics.Initialize();
        Debug.Log("GameAnalytics initialized!");

    }


    public void CreateCollectTxt(Vector3 pos,Color color,string amount)
    {
        var txt = Instantiate(collectValue, transform);

        txt.color = color;
        txt.text =amount;

        pos.y += 0.7f;
        pos.x += 1.2f;
        var convertedPos = Camera.main.WorldToScreenPoint(pos);
        txt.transform.position = convertedPos;

        txt.GetComponent<TextMeshProUGUI>().DOColor(new Color(1, 1, 1, 0), 4f);
        txt.transform.DOLocalMoveY(convertedPos.y + 600, 4f).SetEase(Ease.Linear);

        Destroy(txt.gameObject, 4f);

    }

    public void CpiBackgroundUp()
    {
        cpiBackground.transform.DOMove(cpiBackgroundUp.transform.position, 0.5f);
    }

    public void CpiBackgroundDown()
    {
        cpiBackground.transform.DOMove(cpiBackgroundDown.transform.position, 0.5f);
    }
}
