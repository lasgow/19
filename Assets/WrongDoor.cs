using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class WrongDoor : MonoBehaviour
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
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerManager>().FalseDoor();
        }
    }
}
