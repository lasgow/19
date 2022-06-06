using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class QuizCubes : MonoBehaviour
{
    [SerializeField] GameObject redCube;
    [SerializeField] GameObject greenCube;
    [SerializeField] GameObject googleLogo;
    [SerializeField] GameObject youtubeLogo;
    [SerializeField] GameObject itemCollectRef;
    // Start is called before the first frame update
    void Start()
    {
        googleLogo.transform.DOMove(greenCube.transform.position, 1f).
            SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        googleLogo.transform.DOScale(Vector3.one, 1f).
            SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        youtubeLogo.transform.DOJump(greenCube.transform.position, 3f, 1, 1f).
            SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        youtubeLogo.transform.DOScale(Vector3.zero, 1f).
            SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {

    }

}

