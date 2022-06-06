using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizQM : MonoBehaviour
{
    [SerializeField] GameObject nextObject;
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
            if(nextObject != null)
            {
                nextObject.SetActive(true);
            }
            
        }

    }
}
