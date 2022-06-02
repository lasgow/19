using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ChooseDoor : MonoBehaviour
{
    [SerializeField] GameObject otherDoor;
    [SerializeField] GameObject selectText;
    [SerializeField] OnlineVideoLoader onlineVideoLoader;
    [SerializeField] string doorVideoUrl;
    [SerializeField] bool anyNextDoor;
    [SerializeField] GameObject nextDoor;
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
            otherDoor.GetComponent<BoxCollider>().enabled = false;
            selectText.gameObject.SetActive(false);
            if (anyNextDoor)
            {
                nextDoor.gameObject.SetActive(true);
            }
            transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {

            });
            onlineVideoLoader.videoUrl = doorVideoUrl;
            onlineVideoLoader.VideoPlayerFunction();
        }
    }
}
