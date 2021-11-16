using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HeadManager : MonoBehaviour
{
    private GameManager gameManager;
    private CameraFollow cameraFollow;
    [Tooltip("Speed of control ball in forward direction")]
    public float currentPlayerSpeed = 12.5f;
    [Tooltip("Speed of control ball in vertical direction")]
    public float gravityValue = -50;

    [Tooltip("currentPlayerSpeed = 9f, gravityValue = -25")]
    public bool slowBall = true;
    public bool seperateCutSound = true;
    [HideInInspector]  public bool headGrounded;
    public CharacterController headController;
    //private Vector3 headVelocity;
    [HideInInspector] public Transform target;
    //public float swipeSpeed = 0.5f;
    //public bool useEditorSwipeSpeed;
    public float editorSwipeSpeed = 0.5f;

    [HideInInspector] public Vector3 headMove;
    public float jumpHeight = 8f;
    public Transform path;
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    private Transform headTransform;

    private Vector3 headPositionInit;
    private State state;
    [HideInInspector] public bool stopMotion;//
    [HideInInspector] public float lastZ;
    public bool lookDirection;
    private bool isVisible =true;
    //private float gravityValue_X_deltaTime;
    float jumpUpValue;
    public event System.Action PlayerMoved;
    void Awake()
    {
#if UNITY_EDITOR
        if (slowBall)
        {
            currentPlayerSpeed = 9f;
            gravityValue = -25;
        }
#endif
        //currentPlayerSpeed = 10;
//#if UNITY_EDITOR
//        if (useEditorSwipeSpeed)
//            swipeSpeed = editorSwipeSpeed;
//#endif        
        Input.multiTouchEnabled = false;
        Refs refs = FindObjectOfType<Refs>();
        cameraFollow =refs.cameraFollow;
        initialPosition = transform.localPosition;
        prevPosition = initialPosition;

        initialRotation = transform.localEulerAngles;
        gameManager = refs.gameManager;
        headGrounded = true;
        headTransform = headController.transform;
        headPositionInit = headTransform.position;
        //gravityValue_X_deltaTime = gravityValue * 0.01f;
        jumpUpValue = Mathf.Sqrt(jumpHeight * -3 * gravityValue);
    }

    public void SetInitialRotation()
    {
        transform.localEulerAngles = initialRotation;
    }
    private void OnEnable()
    {
        gameManager.LevelStartedEvent += OnLevelStart;
        gameManager.LevelOverEvent += OnLevelOver;
        cameraFollow.CameraRotated += OnCameraRotated;
    }
    private void OnDisable()
    {
        gameManager.LevelStartedEvent -= OnLevelStart;
        gameManager.LevelOverEvent -= OnLevelOver;
        cameraFollow.CameraRotated -= OnCameraRotated;
    }

    private void OnCameraRotated()
    {
        stopMotion = true;
    }
    private void OnLevelStart()//
    {
        state = State.WaitingToStart;
        headTransform.position = headPositionInit;
        StopAllCoroutines();
        //cameraFollow.freezeX = true;
        UIManager.Instance.startScreen.SetActive(true);

        transform.localPosition = initialPosition;
        transform.localEulerAngles = initialRotation;
    }
    //private IEnumerator SetPlayerState()
    //{
    //    yield return null;
    //    state = State.WaitingToStart;//once
    //}
    private void OnLevelOver(bool won, int multiplier)
    {
        state = State.Dead;
    }
    Vector3 prevPosition;
    float moveY;
    private bool playerMoved;
    //float moveZ;
    void Update()
    {
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (TestInput())
                {
                    state = State.Playing;
                    UIManager.Instance.startScreen.SetActive(false);
                    Jump();
                }
                break;
            case State.Playing:
                if (!stopMotion)
                {
                    if (!playerMoved)
                    {
                        playerMoved = true;
                        PlayerMoved?.Invoke();
                    }
                    headGrounded = headController.isGrounded;
                    if (headGrounded && moveY <0)
                    {
                        moveY = 0;
                    }
                    headMove = Vector3.forward* currentPlayerSpeed * Time.deltaTime;
                    if (TestInput() && isVisible)
                    {
                        Jump();
                    }
                    moveY += gravityValue * Time.deltaTime;
                    headMove.y += moveY * Time.deltaTime;
                    headController.Move(headMove);
                    //Move();
                    //if(lookDirection)
                    //LookDirection();
                }
                break;
            case State.Dead:
                break;

        }
    }
    //private void Move()
    //{
    //    headMove.z = currentPlayerSpeed * Time.deltaTime;//
    //    moveY += gravityValue * Time.deltaTime;
    //    headMove.y += moveY * Time.deltaTime;
    //}
    TouchPhase touchBegan = TouchPhase.Began;
    private bool TestInput()
    {
        return
            (Input.touchCount > 0 && Input.GetTouch(0).phase == touchBegan) ||
            //Input.GetMouseButtonDown(0) ||
            Input.GetKeyDown(KeyCode.Space);
    }
    private void Jump()
    {
        AudioManager.Instance.PlayTapSound();
        //moveY += Mathf.Sqrt(jumpHeight * -3 * gravityValue);//150  = 3 / 0.02
        moveY = jumpUpValue;//150  = 3 / 0.02
    }
    private enum State
    {
        Dead,
        WaitingToStart,
        Playing,
    }
    //Vector3 direction;
    //Quaternion quat;
    //private void LookDirection()
    //{
    //    direction = (transform.position - prevPosition).normalized;
    //    quat = Quaternion.LookRotation(-direction, Vector3.right);
    //    transform.rotation = quat;
    //    prevPosition = transform.position;
    //}
    private void OnBecameVisible()
    {
        isVisible = true;
    }
    private void OnBecameInvisible()
    {
        isVisible = false;
    }
   
}
