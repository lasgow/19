using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int touristValue;
    [SerializeField] private GameObject highlightPlane;
    public void Collect()
    {
        highlightPlane.SetActive(false);
        //CanvasManager.instance.CreateCollectTxt(LevelManager.instance.GetActiveLevel().player.transform.position,Color.green,"+"+touristValue);
    }
}
