using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TrueDoor : MonoBehaviour
{
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
            transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => 
            {
            });
            
            other.GetComponent<PlayerManager>().TrueDoor();
        }
    }
}
