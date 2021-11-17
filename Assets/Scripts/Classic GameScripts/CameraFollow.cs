using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 frontViewPos = new Vector3(47.4f, 14.527f, 8.7f);
    private Vector3 frontViewRot = Vector3.up*-90;
    public bool frontView;

    private Vector3 cullCubePosFrontView = new Vector3(-21.7f, -14.6f, 47f);
    private Vector3 cullCubeRotfrontView = Vector3.up * -90;

    private LevelGenerator levelGenerator;
    private PlayerBallControlScript playerBallControlScript;
    public CameraCullScript cameraCullScript;
    private GameManager gameManager;
    private PlayerBallScript playerBallScript;


    public Transform target;
    public Transform nearCullTransform;
    public Transform farCullTransform;
    private Vector3 offset;
    private Vector3 targetPos;
    public event System.Action CameraRotated;//

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    float initilFov;
    //public float a;
    Camera cam;
    private bool followTarget;
    public Transform platform;
    float platformSlant;
    float platformStartPoint;
    float tanAngle;
    public Transform finalCamPosition;
    public Transform lowestPoint;
    bool ascendCamera;
    private Vector3 finalRotate = new Vector3(19,-90,0);


    //private LevelGenerator levelGenerator;
    //private PlayerBallControlScript playerBallControlScript;
    public bool printSuccessType;
    private float initialY;
    public bool tweenMove;
    private void Awake()
    {
        
        initialY = transform.localPosition.y;
        Time.timeScale = 1f;
#if !UNITY_EDITOR
        GetComponent<Camera>().fieldOfView = 80;
        cameraCullScript.transform.localPosition = nearCullTransform.localPosition;
#endif
        Refs refs = FindObjectOfType<Refs>();
        gameManager = refs.gameManager;
        levelGenerator = refs.levelGenerator;
        playerBallControlScript = refs.playerBallControlScript;
        playerBallScript = refs.playerBallScript;

        if (frontView)
        {
            transform.position = frontViewPos;
            transform.localEulerAngles = frontViewRot;
            cameraCullScript.transform.localPosition = cullCubePosFrontView;
            cameraCullScript.transform.localEulerAngles = cullCubeRotfrontView;
        }
    }
    private void OnEnable()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        cam = GetComponent<Camera>();
        initilFov = cam.fieldOfView;
        levelGenerator.SetBasket += OnBasketSet;

        gameManager.LevelStartedEvent += OnLevelLoaded;
        gameManager.LevelOverEvent += OnLevelOver;
        playerBallControlScript.PlayerMoved += OnPlayerMoved;
        playerBallScript.PathEndReached += MoveToFinalPosition;


    }
    private void OnDisable()
    {
        gameManager.LevelStartedEvent -= OnLevelLoaded;
        gameManager.LevelOverEvent -= OnLevelOver;
        levelGenerator.SetBasket -= OnBasketSet;
        playerBallControlScript.PlayerMoved -= OnPlayerMoved;
        playerBallScript.PathEndReached -= MoveToFinalPosition;

    }
    Vector3 endPos;
    float zDist;
    private void OnBasketSet()
    {
        Vector3 startPos = target.position - offset;
        float cameraHeightFromPlatform = 20;//
        startPos.y = -tanAngle * (targetPos.z - platformStartPoint) + cameraHeightFromPlatform;
        //JU.DebugCube(Vector3.up * (-tanAngle * (targetPos.z - platformStartPoint)), Color.red);
        //startPos.y = 14.5f;
        transform.position = startPos;
        endPos =startPos;
        endPos.z = levelGenerator.lastPos.z - offset.z;
        endPos.y = -tanAngle * (endPos.z - platformStartPoint) + cameraHeightFromPlatform;
        zDist = (endPos.z - startPos.z);
    }
    void Start()
    {
        platformSlant = platform.eulerAngles.x;
        //platformStartPoint = platform.position.z;
        platformStartPoint = -50;
        tanAngle = Mathf.Tan(Mathf.Deg2Rad * platformSlant);
        tanAngle = (float)System.Math.Round((double)tanAngle, 2);
        targetPos = transform.position;
        //initialX = targetPos.x;
        //initialY = targetPos.y;
        offset = target.position - transform.position;
        //platformYOffest = transform.position.y - JU.RotatePointAroundPivot
        //(Vector3.forward*transform.position.z, platform.transform.position, Vector3.right*platform.transform.eulerAngles.x).y;
        //offsetRotation = target.eulerAngles - transform.eulerAngles;
    }

    int tweenId;
    private void OnPlayerMoved()
    {
        float tweenTime = 0;
        if (tweenMove)
        {
            tweenTime = (zDist * 10) / (playerBallControlScript.currentPlayerSpeed * 10f);
            tweenId = LeanTween.move(gameObject, endPos, tweenTime).id;//
        }
        //if(tweenTime == 0)
        //Rystart
    }
    private void OnLevelLoaded()
    {
        StopAllCoroutines();
        //gameFinished = false;

        transform.position = initialPosition;
        transform.rotation = initialRotation;//
        cam.fieldOfView = initilFov;
        followTarget = true;
    }
    //float camTransitTime = 1.5f;
    //private Transform lookAtRef;
    private void OnLevelOver(bool won, int multiplier)
    {
        //gameFinished = true;
        ascendCamera = false;
        if (!won)
        {
            StopFollow();
        }
    }
    Vector3 speed = Vector3.zero;
    //Vector3 speedRot = Vector3.zero;
    float yPos;
    void LateUpdate()
    {
        if (!tweenMove)
            if (followTarget)
            {
                //headMove = Vector3.forward * currentPlayerSpeed * Time.deltaTime;//
                yPos = -tanAngle * (targetPos.z - platformStartPoint) + 20;//
                targetPos = target.position - offset;
                targetPos.y = yPos;
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref speed, 0.2f);
                //GameObject obj = JU.DebugCube(transform.position, Color.yellow);
            }
        if (ascendCamera)//
        {
            transform.position += Vector3.up * Time.deltaTime * 3;
        }
    }
    public void MoveToFinalPosition()
    {
        LeanTween.cancel(tweenId);
        StopFollow();
        LeanTween.move(gameObject, finalCamPosition.position, 1f).setEase(LeanTweenType.linear).setOnComplete(() => {
            Invoke("MoveToLowestPosition", 1);
            CameraRotated?.Invoke();
            Time.timeScale = 2f;
        });
        LeanTween.rotate(gameObject, finalCamPosition.eulerAngles, 1).setEase(LeanTweenType.linear);
        LeanTween.value(gameObject, UpdateValueExampleCallback, initilFov, 60, 1).setEase(LeanTweenType.linear);
    }
    private void MoveToLowestPosition()
    {
        Vector3 targetPos = new Vector3(transform.position.x, lowestPoint.position.y, transform.position.z);
        //targetPos = new Vector3(46.3f, -70f, 241.6f);
        targetPos = new Vector3(66.3f, lowestPoint.position.y - 5, transform.position.z);
        LeanTween.move(gameObject, targetPos, 1).setEase(LeanTweenType.linear).setOnComplete(() => {
            //StartCoroutine("AscendCamera", true);
        });
    }
    //public IEnumerator AscendCamera(bool ascend)
    //{
    //    if (ascend)
    //    {
    //        yield return new WaitForSeconds(2);
    //        ascendCamera = true;
    //    }
    //    else
    //    {
    //        ascendCamera = false;
    //    }
    //}
    private void UpdateValueExampleCallback(float val, float ratio)
    {
        cam.fieldOfView = val;
    }
    public void StopFollow()
    {
        followTarget = false;
        LeanTween.cancel(tweenId);
    }
    public void FinalLookAt()//
    {
        //LeanTween.rotate(gameObject, finalRotate, 1);
        finalRotate = new Vector3(6.977f, -90, 0);
        GameObject fishTank = GameObject.FindGameObjectWithTag("FishTank");
        //if (fishTank != null)
        //    print("fishTank pos "+fishTank.transform.position);
        Vector3 finalPos/* = new Vector3(58.6f, -94.4f, transform.position.z)*/;
        //print("finalPos " + finalPos);
        Vector3 diff = new Vector3(53.6f, 17.6f,0);
        finalPos = fishTank.transform.position + diff;
        //print("diff "+(finalPos-fishTank.transform.position));
        LeanTween.rotate(gameObject, finalRotate, 2.5f);
        LeanTween.move(gameObject, finalPos, 2.5f);//
    }
}
