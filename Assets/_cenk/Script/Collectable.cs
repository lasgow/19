using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int touristValue;
    [SerializeField] private GameObject highlightPlane;
    public void Collect()
    {
        Instantiate(GameManager.instance.explosion, transform.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().Play("Pop");
        if (highlightPlane != null)
        {
            highlightPlane.SetActive(false);
        }

    }
}
