using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class QuizSection : MonoBehaviour
{
    public static QuizSection instance;

    [SerializeField] GameObject[] questions;
    [SerializeField] GameObject[] tutorials;
    [SerializeField] GameObject questionUpRef;
    [SerializeField] GameObject questionDownRef;
    [SerializeField] GameObject JoystickUpRef;
    [SerializeField] GameObject JoystickDownRef;
    [SerializeField] GameObject page1;
    [SerializeField] GameObject page2;
    int questionInt;
    private void Awake()
    {
        instance = this;
        questionInt = 0;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void questionUIUp()
    {
        if(questionInt < 2)
        {
            questions[questionInt].transform.DOMove(questionUpRef.transform.position, 0.5f).SetEase(Ease.Linear);
        }
        else
        {
            questions[questionInt].transform.DOMove(JoystickUpRef.transform.position, 0.5f).SetEase(Ease.Linear);
            questionDownRef = JoystickDownRef;
        }
        
    }

    public void questionUIDown()
    {
        FindObjectOfType<AudioManager>().Play("Win");
        questions[questionInt].transform.DOMove(questionDownRef.transform.position, 0.5f).SetEase(Ease.Linear).OnComplete(() => 
        {
            PlayerManager.instance.xSpeed = 1.2f;
            PlayerManager.instance.canRotate = true;
            PlayerManager.instance.pathFollower.speed = 12.5f;
            Instantiate(GameManager.instance.explosion, PlayerManager.instance.transform.position, Quaternion.identity);
            questions[questionInt].SetActive(false);
            tutorials[questionInt].SetActive(false);
            if (questionInt < questions.Length)
            {
                questionInt++;
            }
        });

    }

    public void NextPage()
    {
        page1.SetActive(false);
        page2.SetActive(true);
    }
}
