using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CanvasScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    void Start()
    {

    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
    }


}
