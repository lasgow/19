using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class InfoPanel : MonoBehaviour
{
    public static InfoPanel instance;
    void Start()
    {
        
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    public void OpenPanel()
    {
        transform.DOScale(Vector3.one, 0.5f).SetDelay(1).SetEase(Ease.OutBack).OnComplete(() => {

        });


    }

    public void ClosePanel()
    {
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            PlayerManager.instance.InfoPanelClose();
        });


    }
}
