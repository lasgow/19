using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finishline : MonoBehaviour
{
    [SerializeField] string cpiValue;
    [SerializeField] string retentionValue;
    [SerializeField] string totalDownloadValue;
    [SerializeField] string playtimeValue;
    [SerializeField] string revenueValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().Play("Win");
            CanvasManager.instance.cpiValue.SetText(cpiValue);
            CanvasManager.instance.retentionValue.SetText(retentionValue);
            CanvasManager.instance.totalDownloadValue.SetText(totalDownloadValue);
            CanvasManager.instance.playtimeValue.SetText(playtimeValue);
            CanvasManager.instance.revenueValue.SetText(revenueValue);
        }
    }
}
