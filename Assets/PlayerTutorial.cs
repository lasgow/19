using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerTutorial : MonoBehaviour
{
    [SerializeField] GameObject tutorialRef;
    [SerializeField] GameObject googleLogo;
    [SerializeField] Vector3 tutorialFirstRef;
    void Start()
    {
        transform.DOLocalMoveX(googleLogo.transform.position.x, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        tutorialFirstRef = googleLogo.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Collect"))
        {
            Debug.Log("calisyo");
            Instantiate(GameManager.instance.explosion, other.transform.position, Quaternion.identity);
            other.transform.DOLocalMove(tutorialRef.transform.localPosition, 0.5f).SetEase(Ease.Linear).OnComplete(() => 
            {
                other.transform.position = tutorialFirstRef;
            });
            other.transform.DOScale(Vector3.zero,0.5f).SetLoops(1, LoopType.Yoyo).SetEase(Ease.Linear).OnComplete(() =>
            {
                other.transform.DOScale(Vector3.one,0f);
            }); ;
        }
    }
}
