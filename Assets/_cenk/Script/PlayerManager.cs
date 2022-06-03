using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ElephantSDK;
using MoreMountains.NiceVibrations;
using TMPro;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    private PathCreation.Examples.PathFollower pathFollower;

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
    private bool canRotate;
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
    private bool isClick;
    private float mouseX;
    private Vector3 move;
    [SerializeField] GameObject rawImage;

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

                    //animator.SetBool("Walk", true);
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

            FillBar(other.GetComponent<Collectable>().touristValue);

            MMVibrationManager.Haptic(HapticTypes.LightImpact);

            other.transform.DOScale(0, 0.20f).SetEase(Ease.Linear);
            other.transform.DOMove(itemCollectRef.transform.position, 0.20f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(other.gameObject);
                
            });
        }

        if (other.CompareTag("BadCollect"))
        {
            other.GetComponent<Obstacle>().Collect();
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
            //CanvasManager.instance.CreateCollectTxt(transform.position, Color.red, "-" + 5);
            if(canRotate)
            {
            FillBar(-5);
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

            MMVibrationManager.Haptic(HapticTypes.LightImpact);

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
            rawImage.transform.DOScale(new Vector3(0.11f, 0.115f, 0.115f), 1f);
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
                //villager3D.GetComponent<Animator>().SetBool("Walk", true);

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
           // villager3D.SetActive(false);
           // villager3D.transform.DORotate((Vector3.zero), 0);
           // viking3D.SetActive(true);
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
            //viking3D.SetActive(false);
            //viking3D.transform.DORotate((Vector3.zero), 0);
            //ragnar3D.SetActive(true);
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
            //ragnar3D.SetActive(false);
            //ragnar3D.transform.DORotate((Vector3.zero), 0);
            //thor3D.SetActive(true);
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
        //Camera.main.transform.SetAsLastSibling();
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
        //animator = currentModel.GetComponent<Animator>();
        //animator.SetBool("Walk", true);
        //animator.SetTrigger("Spin");
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

    private void ClampPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -4f, 4f);
        transform.position = clampedPosition;
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
        pathFollower.speed = 8.5f;
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
        Debug.Log("aaa");
    }

}
