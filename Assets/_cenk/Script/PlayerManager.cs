using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ElephantSDK;
using TMPro;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public PathCreation.Examples.PathFollower pathFollower;

    public Animator animator;

    public int touristCounter;
    public int collectedTouristNO;
    public TextMeshProUGUI nameTag;
    public float totalAmount = 50f;
    public Image filledBar;
    public GameObject barCanvas;

    [SerializeField] GameObject itemCollectRef;
    [Header("Movement Settings")]
    public float xSpeed;
    private Touch touch;

    [Header("Conditions")]
    public bool isFinished = false;

    [Header("Particles")]
    public GameObject[] changeParticle;
    public GameObject rainParticle;
    public GameObject confParticle;
    public bool canRotate;
    [Header("Chars")]
    public GameObject activeModel;
    public GameObject wheelLeft;
    public GameObject wheelRight;
    public GameObject intern;
    public GameObject jrDeveloper;
    public GameObject developer;
    public GameObject srDeveloper;
    public GameObject srDeveloperParticle;
    public GameObject gameGuru;
    public GameObject gameGuruParticle;

    [SerializeField]
    public GameObject leftSlot;
    public GameObject rightSlot;

    private bool isClick;
    private float mouseX;
    private Vector3 move;
    [SerializeField] GameObject rawImage;
    [SerializeField] bool isLevel3;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        pathFollower = transform.parent.GetComponent<PathCreation.Examples.PathFollower>();
        activeModel = intern.gameObject;
        canRotate = true;
    }
    void Update()
    {
        MouseInputAndClickCheck();
        Movement();
        MovementAndSpeedControl();
        //Debug.Log(canRotate);       
    }
    public void Movement()
    {
        if (isFinished)
            return;

        if (Input.touchCount > 0)
        {
            
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                if (CanvasManager.instance.guide.activeInHierarchy)
                {
                    MoveForward();

                    CanvasManager.instance.guide.SetActive(false);

                    Elephant.LevelStarted((LevelManager.GetLevelID() + 1));
                }

                var pos = transform.localPosition;
                transform.localPosition = new Vector3(Mathf.Clamp(pos.x + touch.deltaPosition.x * xSpeed * Time.deltaTime, -4, 4), 0, pos.z);

            }

            if (touch.phase==TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if(canRotate)
                {
                    activeModel.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f).SetEase(Ease.Linear);
                    activeModel.transform.DOLocalMoveX(0, 0.1f).SetEase(Ease.Linear);
                }

            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Collect"))
        {
            other.GetComponent<Collectable>().Collect();
            other.GetComponent<BoxCollider>().enabled = false;
            FillBar(other.GetComponent<Collectable>().touristValue);

            
            other.transform.DOScale(0, 0.20f).SetEase(Ease.Linear);
            other.transform.DOMove(itemCollectRef.transform.position, 0.20f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(other.gameObject);
                
            });
        }

        if (other.CompareTag("BadCollect"))
        {
            other.GetComponent<Obstacle>().Collect();
            if(canRotate)
            {
            //FillBar(-5);
            }
            
            other.transform.DOScale(0, 0.20f).SetEase(Ease.Linear);
            other.transform.DOMove(itemCollectRef.transform.position, 0.20f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(other.gameObject);

            });
        }

        if (other.CompareTag("CollectInfo"))
        {
            other.GetComponent<Collectable>().Collect();

            FillBar(other.GetComponent<Collectable>().touristValue);
            other.transform.DOScale(0, 0.20f).SetEase(Ease.Linear);
            other.transform.DOMove(itemCollectRef.transform.position, 0.20f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(other.gameObject);

            });
            InfoPanelOpen();
            InfoPanel.instance.OpenPanel();
        }

        if(other.CompareTag("Finish"))
        {
            //transform.DOMoveX(0f, 0.25f);
            animator.enabled = false;
            isFinished = true;
            canRotate = false;
            pathFollower.speed = 0;
            transform.DOLocalMoveX(0, 0.25f);
            Camera.main.transform.DOLocalRotate(new Vector3(15, 0, 0), 1f);
            CompletePanel.instance.OpenPanel();
            rawImage.SetActive(true);
            if(!isLevel3)
            {
                rawImage.transform.DOScale(new Vector3(0.11f, 0.115f, 0.115f), 1f);
            }
            else
            {
                rawImage.transform.DOScale(new Vector3(0.11f, 0.1555f, 0.115f), 1f);
            }

        }

        if (other.CompareTag("QuizTime"))
        {
            Destroy(other.gameObject);
        }

        if(other.CompareTag("QuizQM"))
        {
            xSpeed = 0;
            pathFollower.speed = 0;
            transform.DOLocalMoveX(0, 0.25f);
            canRotate = false;
            other.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() => 
            {
                Destroy(other.gameObject);
            });
            Instantiate(GameManager.instance.explosion, other.transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("Pop");
            QuizSection.instance.questionUIUp();            
        }

        if(other.CompareTag("LeftSlot"))
        {
            FindObjectOfType<AudioManager>().Play("Pop");
            other.transform.GetComponent<BoxCollider>().enabled = false;
            other.transform.DOMoveZ(other.transform.position.z - 3f, 0.25f).SetEase(Ease.Linear).OnComplete(() => 
            {
                other.transform.parent = transform;
                other.transform.DORotate(leftSlot.transform.eulerAngles, 1f);
                other.transform.DOLocalMoveX(-1.75f, 0.5f).SetEase(Ease.Linear);
                other.transform.DOLocalMoveZ(-2.5f, 0.5f).SetEase(Ease.Linear).OnComplete(() => 
                { 
                
                });
                canRotate = true;
                xSpeed = 1.2f;

            });
        }

        if (other.CompareTag("RightSlot"))
        {
            FindObjectOfType<AudioManager>().Play("Pop");
            other.transform.GetComponent<BoxCollider>().enabled = false;
            other.transform.DOMoveZ(other.transform.position.z + 3f, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
            {
                other.transform.parent = transform;
                other.transform.DORotate(rightSlot.transform.eulerAngles, 1f);
                other.transform.DOLocalMoveZ(-2.5f, 0.5f).SetEase(Ease.Linear);
                other.transform.DOLocalMoveX(1.75f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {

                });
            });
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }

    public void FillBar(int value)
    {
        touristCounter += value;
        collectedTouristNO += value;
        if (touristCounter <= 0)
        {
            touristCounter = 0;

            if (jrDeveloper.activeInHierarchy)
            {
                
                jrDeveloper.SetActive(false);
                jrDeveloper.transform.DORotate((Vector3.zero), 0);
                intern.SetActive(true);
                activeModel = intern.gameObject;
                var transformParticle = Instantiate(changeParticle[Random.Range(0, 1)], itemCollectRef.transform.position, Quaternion.identity);
                transformParticle.transform.parent = itemCollectRef.transform;
                Destroy(transformParticle, 1f);
                //
                pathFollower.speed = 7f;
                nameTag.SetText("INTERN");

                touristCounter = (int)totalAmount - 5;
            }

            if (developer.activeInHierarchy)
            {
                StartCoroutine(ChangeActiveModel(jrDeveloper,developer));
                var transformParticle = Instantiate(changeParticle[Random.Range(0, 1)], itemCollectRef.transform.position, Quaternion.identity);
                transformParticle.transform.parent = itemCollectRef.transform;
                Destroy(transformParticle, 1f);
                //
                pathFollower.speed = 7.5f;
                nameTag.SetText("JR. DEV");

                touristCounter = (int)totalAmount - 5;
            }

            if (srDeveloper.activeInHierarchy)
            {

                StartCoroutine(ChangeActiveModel(developer,srDeveloper));
                nameTag.SetText("GAME DEV.");
                pathFollower.speed = 8f;
                touristCounter = (int)totalAmount - 5;
            }

            if (gameGuru.activeInHierarchy)
            {
                StartCoroutine(ChangeActiveModel(srDeveloper,gameGuru));
                nameTag.SetText("SR. DEV");
                pathFollower.speed = 9f;
                touristCounter = (int)totalAmount - 5;
            }
        }

        if (touristCounter >= totalAmount && intern.activeInHierarchy)
        {
            StartCoroutine(ChangeActiveModel(jrDeveloper,intern));
            var transformParticle = Instantiate(changeParticle[0], itemCollectRef.transform.position,Quaternion.identity);
            transformParticle.transform.parent = itemCollectRef.transform;
            Destroy(transformParticle, 1f);
            pathFollower.speed = 7f;
            //
            nameTag.SetText("JR. DEV");
            touristCounter = 0;
        }
        if (touristCounter >= totalAmount && jrDeveloper.activeInHierarchy)
        {
            StartCoroutine(ChangeActiveModel(developer,jrDeveloper));
            var transformParticle = Instantiate(changeParticle[Random.Range(0, 1)], itemCollectRef.transform.position, Quaternion.identity);
            transformParticle.transform.parent = itemCollectRef.transform;
            Destroy(transformParticle, 1f);
            //
            pathFollower.speed = 7.5f;
            nameTag.SetText("GAME DEV.");
            touristCounter = 0;
        }

        if (touristCounter >= totalAmount && developer.activeInHierarchy)
        {
            StartCoroutine(ChangeActiveModel(srDeveloper,developer));
            var transformParticle = Instantiate(srDeveloperParticle, itemCollectRef.transform.position, Quaternion.identity);
            transformParticle.transform.parent = itemCollectRef.transform;
            Destroy(transformParticle, 1f);
            //
            
            nameTag.SetText("SR. DEV");
            pathFollower.speed = 8f;
            touristCounter = 0;
        }
        filledBar.fillAmount = touristCounter / totalAmount;
    }
    


    public void MoveForward()
    {
        Camera.main.transform.SetParent(transform.parent);
        CameraFollow.instance.target = transform;
        CameraFollow.instance.enabled = true;
        pathFollower.enabled = true;
    }

    IEnumerator ChangeActiveModel(GameObject currentModel,GameObject lastModel)
    {
        lastModel.gameObject.SetActive(false);
        canRotate = false;
        activeModel = currentModel;
        activeModel.SetActive(true);
        lastModel.transform.DOLocalRotate(Vector3.zero, 0.1f);
        lastModel.transform.DOLocalMove(Vector3.zero, 0.1f);
        // Debug.Log(lastModel + "CHANGED and ROTATION IS " + lastModel.transform.rotation);
        yield return new WaitForSeconds(1f);
        canRotate = true;
        activeModel.transform.DOLocalMove(new Vector3(0, 0, 0), 0.1f).SetEase(Ease.Linear);
    }

    private void MovementAndSpeedControl()
    {

        if (isClick) // If Player Clicking Set Position X
        {

            if (move.x != 0)
            {
                if(canRotate)
                {
                    activeModel.transform.localRotation = Quaternion.Slerp(activeModel.transform.localRotation, Quaternion.LookRotation(new Vector3(move.x / 2, 0, pathFollower.speed)), 5 * Time.deltaTime);
                    wheelLeft.transform.localRotation = Quaternion.Slerp(wheelLeft.transform.localRotation, Quaternion.LookRotation(new Vector3(move.x / 2, 0, pathFollower.speed)), 5 * Time.deltaTime);
                    wheelRight.transform.localRotation = Quaternion.Slerp(wheelRight.transform.localRotation, Quaternion.LookRotation(new Vector3(move.x / 2, 0, pathFollower.speed)), 5 * Time.deltaTime);

                }
            }
            else
            {
                if(canRotate)
                {
                    activeModel.transform.localRotation = Quaternion.Slerp(activeModel.transform.localRotation, Quaternion.LookRotation(new Vector3(0, 0, pathFollower.speed)), 5 * Time.deltaTime);
                    wheelLeft.transform.localRotation = Quaternion.Slerp(wheelLeft.transform.localRotation, Quaternion.LookRotation(new Vector3(0, 0, pathFollower.speed)), 5 * Time.deltaTime);
                    wheelRight.transform.localRotation = Quaternion.Slerp(wheelRight.transform.localRotation, Quaternion.LookRotation(new Vector3(0, 0, pathFollower.speed)), 5 * Time.deltaTime);
                }
            }
        }


        else
        {
            if(canRotate)
            {
                activeModel.transform.localRotation = Quaternion.Slerp(activeModel.transform.localRotation, Quaternion.LookRotation(new Vector3(0, 0, pathFollower.speed)), 5 * Time.deltaTime);
                wheelLeft.transform.localRotation = Quaternion.Slerp(wheelLeft.transform.localRotation, Quaternion.LookRotation(new Vector3(0, 0, pathFollower.speed)), 5 * Time.deltaTime);
                wheelRight.transform.localRotation = Quaternion.Slerp(wheelRight.transform.localRotation, Quaternion.LookRotation(new Vector3(0, 0, pathFollower.speed)), 5 * Time.deltaTime);

            }
        }
    }
    private void MouseInputAndClickCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isClick = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isClick = false;
        }

        if (isClick) // If Player Clicking Get Clicking Positions and Set Them.
        {
            mouseX += Input.GetAxis("Mouse X");
            float pointer_x = Input.GetAxis("Mouse X");
            //float pointer_y = Input.GetAxis("Mouse Y");
            if (Input.touchCount > 0)
            {
                pointer_x = Input.touches[0].deltaPosition.x;
                //pointer_y = Input.touches[0].deltaPosition.y;
            }
            move = new Vector3(pointer_x, 0, 0);

        }
        else // If Player Isn't Clicking Set Rotation to 0
        {
            
        }
    }

    public void InfoPanelOpen()
    {
        pathFollower.speed = 0f;
        xSpeed = 0f;
        transform.DOLocalMoveX(0, 0.25f);
        canRotate = false;
        Camera.main.GetComponent<Camera>().DOFieldOfView(0, 1f);

    }
    public void InfoPanelClose()
    {
        xSpeed = 1.2f;
        pathFollower.speed = 12.5f;
        canRotate = true;
        Camera.main.GetComponent<Camera>().DOFieldOfView(60, 1f);
    }

    public void DoTweenIncrease(float increasedValue, float addNumber, float timer)
    {
        DOTween.To(() => increasedValue, x => increasedValue = x, addNumber, timer).SetEase(Ease.OutCubic).OnUpdate(() => 
        {
            pathFollower.speed = increasedValue;
        }).OnComplete(() => {
            pathFollower.speed = 9.5f;
            canRotate = true;
        });

    }

    public void TrueDoor()
    {
        pathFollower.speed = 12.5f;
        xSpeed = 0f;
        transform.DOLocalMoveX(0, 2.5f);
        canRotate = false;
    }

    public void FalseDoor()
    {
        canRotate = false;
        pathFollower.speed = -12.5f;
        DoTweenIncrease(pathFollower.speed, -8, 1f);
    }

}
