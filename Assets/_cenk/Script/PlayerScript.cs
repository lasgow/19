using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Components")]
    public Animator animator;
    [Header("Movement Settings")]
    public float xSpeed = 0.8f;
    [SerializeField] private float moveSpeed;
    private Touch touch;
    [Header("Conditions")]
    private bool canRotate = true;
    private GameObject activeModel;
    public bool isFinished = false;
    private bool isClick;
    private float mouseX;
    private Vector3 move;
    private float clampMin = -5.6f;
    private float clampMax = 5.6f;

    private void Start()
    {
        activeModel = transform.gameObject;
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveSpeed);
        MovementAndSpeedControl();
        MouseInputAndClickCheck();
        Movement();
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
                /*
                 
                if (CanvasManager.instance.guide.activeInHierarchy)
                {
                    //MoveForward();

                    CanvasManager.instance.guide.SetActive(false);

                    //Elephant.LevelStarted((LevelManager.GetLevelID() + 1));


                }
                */
                var pos = transform.localPosition;

                transform.localPosition = new Vector3(Mathf.Clamp(pos.x + touch.deltaPosition.x * xSpeed * Time.deltaTime, clampMin, clampMax), pos.y, pos.z);

            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                /*
                 
                if (canRotate)
                {
                    activeModel.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f).SetEase(Ease.Linear);
                    activeModel.transform.DOLocalMoveX(0, 0.1f).SetEase(Ease.Linear);
                }
                */
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

        if (isClick) // If Player Clicking Set Player's Rotation
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

    private void MovementAndSpeedControl()
    {

        if (isClick) // If Player Clicking Set Position X
        {

            if (move.x != 0)
            {
                if (canRotate)
                {
                    activeModel.transform.localRotation = Quaternion.Slerp(activeModel.transform.localRotation, Quaternion.LookRotation(new Vector3(move.x / 2, 0, 5)), 5 * Time.deltaTime);

                }
            }
            else
            {
                if (canRotate)
                {
                    activeModel.transform.localRotation = Quaternion.Slerp(activeModel.transform.localRotation, Quaternion.LookRotation(new Vector3(0, 0, 5)), 5 * Time.deltaTime);

                }
            }
        }


        else
        {
            if (canRotate)
            {
                activeModel.transform.localRotation = Quaternion.Slerp(activeModel.transform.localRotation, Quaternion.LookRotation(new Vector3(0, 0, 5)), 5 * Time.deltaTime);

            }
        }
    }
}
