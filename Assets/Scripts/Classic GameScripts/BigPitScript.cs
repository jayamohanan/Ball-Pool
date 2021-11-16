using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//x into the paper
//z horizontal

public class BigPitScript : MonoBehaviour
{
    public int multiplierCount;
    public Transform topMostPosition;
    //private Vector3 centrePos;
    public GameObject multiplierPrefab;
    public GameObject starCanvasPrefab;
    public ParticleSystem goldRainConfetti;

    public float frontX = 7;
    public float xWidth = 7;//
    //private float backX;//frontX -xWidth 

    public float leftZ;
    public float zWidth;
    private float rightZ;

    public float topY;
    public float height = 40;
    private float bottomY;

    public Transform left;
    public Transform right;
    public Transform back;
    public Transform front;
    public Transform bottom;
    public Transform lowestPoint;

    public Transform leftSlant;
    public Transform rightSlant;
    public Transform middleSlant;

    LevelGenerator levelGenerator;
    HeadScript headScript;
    //private GameManager gameManager;
    public GameObject[] bottomPlates;
    public GameObject[] bottomPlateTriggers;
    private WeightOverTrigger[] wotArray = new WeightOverTrigger[3];

    public ParticleSystem clouds;
    private BottomTriggerScript bts;
    CameraFollow cameraFollow;
    public GameObject finalBottom;
    //public Transform[] stacks;
    public bool increaseWeightArea;
    private int lessFactor;
    private GameManager gameManager;
    void Awake()
    {
        Refs refs = FindObjectOfType<Refs>();
        gameManager = FindObjectOfType<GameManager>();
        levelGenerator = refs.levelGenerator;
        headScript = refs.headScript;
        //headScript.lessFactor = lessFactor;
        //gameManager = refs.gameManager;
        bts = refs.bottomTriggerScript;
        cameraFollow = refs.cameraFollow;
        WeightOverTrigger.lessFactor = lessFactor;
    }
    private void Start()
    {
       lessFactor =  GameDataClass.Instance.lessFactor;
    }
    private void OnEnable()
    {
        levelGenerator.SetBasket += OnBasketSet;
        bts.firstBallEvent += OnFirstBallMet;
        cameraFollow.CameraRotated += OnCameraRotated;//
    }
    private void OnDisable()
    {
        levelGenerator.SetBasket -= OnBasketSet;
        bts.firstBallEvent -= OnFirstBallMet;
        cameraFollow.CameraRotated -= OnCameraRotated;//

    }
    private void OnCameraRotated()
    {
        UIManager.Instance.InitialiseStarCanvas1();
        wotArray[0].index  = 0;
        wotArray[1].index = 1;
        wotArray[2].index = 2;
        
        wotArray[0].updateStar = true;
        wotArray[1].updateStar = true;
        wotArray[2].updateStar = true;
        
        
        wotArray[0].bendCount = 15;
        wotArray[1].bendCount = 12;
        wotArray[2].bendCount = 10;
        //if(index == 2)
        //{

        //    WeightOverTrigger.totalBallsFallen = headScript.totalBallsFallen - lessFactor;
        //    WeightOverTrigger.totalBallsFallen90pct = (int)((headScript.totalBallsFallen - lessFactor) * 0.9f);
        //}
        //else
        //{
            WeightOverTrigger.totalBallsFallen = headScript.totalBallsFallen;
            WeightOverTrigger.totalBallsFallen90pct = (int)((headScript.totalBallsFallen) * 0.9f);
        //}
       

        finalBottom.gameObject.SetActive(true);//

        //#if UNITY_EDITOR

        //        WeightOverTrigger.totalBallsFallen90pct = 300;
        //#endif
#if UNITY_EDITOR
        //if (increaseWeightArea)//
        //{
        //    Vector3 scale;

        //    scale = wotArray[0].transform.parent.localScale;
        //    scale.y = 18;
        //    wotArray[0].transform.parent.localScale = scale;

        //    scale = wotArray[1].transform.parent.localScale;

        //    scale.y = 25;
        //    wotArray[1].transform.parent.localScale = scale;

        //    scale = wotArray[2].transform.parent.localScale;
        //    scale.y = 30;
        //    wotArray[2].transform.parent.localScale = scale;
        //    print("scale "+ scale);//
        //}
#endif
        //WeightOverTrigger.totalBallsFallen = 93;
        //initializedStarCanvas = true;
        EnableNextBend();
    }
    private void OnFirstBallMet()
    {
        //print("OnFirstBallMet ");
        //UIManager.Instance.InitialiseStarCanvas(levelGenerator.totalCount, collectedCount);
        //EnableNextBend();
    }

    private void OnBasketSet()
    {
        Initialize();
        //CreateMultipliers();
        AddRubberLayers();
        Vector3 scale;
        for (int i = 0; i < wotArray.Length; i++)
        {
#if UNITY_EDITOR
            if (increaseWeightArea)//
            {
                scale = wotArray[i].transform.parent.localScale;
                if(i==0)
                scale.y = 18;
                else if(i==1)
                scale.y = 25;
                else if (i==2)
                scale.y = 35;

                wotArray[i].transform.parent.localScale = scale;
            }
#endif
            wotArray[i].CalculateBounds();
            int totalCount = DecideMaxValue();
            //if(gameManager.currentLevel<4)
            //    totalCount = 300;
            //else
            //    totalCount = levelGenerator.totalCount;
            if (i!=2)
            wotArray[i].totalCount = totalCount;
            else
                wotArray[i].totalCount = totalCount - lessFactor;

        }
    }
    private void Initialize()
    {
        rightZ = leftZ + zWidth;
        bottomY = topY - height;
        //backX = frontX - xWidth;

        //left.localPosition = new Vector3(frontX, topY, leftZ);
        //left.localScale = new Vector3(xWidth, 0.5f, height);
        //right.localPosition = new Vector3(frontX, topY, rightZ);
        //right.localScale = new Vector3(xWidth, 0.5f, height);
        //back.localPosition = new Vector3(backX, topY, (leftZ + rightZ) / 2f);
        //back.localScale = new Vector3(zWidth, 0.5f, height+4);
        //front.localPosition = new Vector3(frontX, topY, (leftZ + rightZ) / 2f);
        //front.localScale = new Vector3(zWidth, 0.5f, height+4);
        bottom.localPosition = new Vector3(frontX, bottomY, (leftZ + rightZ) / 2f);

        //bottom.localScale = new Vector3(zWidth + 1, xWidth, 0.5f);

        lowestPoint.position = bottom.position + Vector3.up * 15;

        //float dy1 = leftSlant.localPosition.y - left.localPosition.y;
        //float dz1 = left.localPosition.z - leftSlant.localPosition.z;

        //float dy2 = rightSlant.localPosition.y - right.localPosition.y;
        //float dz2 = rightSlant.localPosition.z - right.localPosition.z;


        //float dy3 = Mathf.Abs(middleSlant.localPosition.y - back.localPosition.y);
        //float dx3 = Mathf.Abs(back.localPosition.x - middleSlant.localPosition.x);

        //float leftSlantLength = Mathf.Sqrt(dy1 * dy1 + dz1 * dz1);
        //float rightSlantLength = Mathf.Sqrt(dy2 * dy2 + dz2 * dz2);
        //float middleSlantLength = Mathf.Sqrt(dy3 * dy3 + dx3 * dx3);

        //float angle1 = Mathf.Atan2(dz1, dy1);
        //float angle2 = Mathf.Atan2(dz2, dy2);
        //float angle3 = (90 - Mathf.Rad2Deg * Mathf.Atan2(dy3, dx3));

        //leftSlant.transform.localEulerAngles = new Vector3(-Mathf.Rad2Deg * angle1, 0, 0);
        //Vector3 scale = leftSlant.localScale;
        //leftSlant.localScale = new Vector3(scale.x, leftSlantLength, scale.z);

        //rightSlant.transform.localEulerAngles = new Vector3(Mathf.Rad2Deg * angle2, 0, 0);
        //scale = rightSlant.localScale;
        //rightSlant.localScale = new Vector3(scale.x, rightSlantLength, scale.z);

        //middleSlant.localEulerAngles = new Vector3(-angle3, 90, 0);
        //scale = middleSlant.localScale;
        //middleSlant.localScale = new Vector3(scale.x, middleSlantLength, scale.z);

        headScript.lastZ = bottom.transform.position.z;
    }

    private void AddRubberLayers()
    {
        //GameObject obj;
        float unitValue = height / 3f;
        //float a = unitValue;
        Vector3 bridgePos = new Vector3(left.position.x, left.position.y - unitValue, back.position.z);
        //Vector3 bridgeTriggerPos = new Vector3(left.position.x - xWidth / 2, left.position.y-1, back.position.z);
        int maxValue = DecideMaxValue();
        //if (gameManager.currentLevel < 3)
        //    maxValue = 300;
        //else
        //maxValue = levelGenerator.totalCount;
        //print("maxValue " + maxValue);
        for (int i = 0; i < 3; i++)
        {//
            bottomPlates[i].transform.position = bridgePos;
            bridgePos.y -= unitValue;
            WeightOverTrigger weightOverTrigger = bottomPlateTriggers[i].GetComponentInChildren<WeightOverTrigger>();
            wotArray[i] = weightOverTrigger;
            int a;
            if (i != 2)
            {
                //a = maxValue / (3 - i);
                a = (int)(maxValue*(i+1) /3f);
                weightOverTrigger.Initialize(a);
            }
            else
            {
                a = (maxValue / (3 - i)) - lessFactor;
                weightOverTrigger.Initialize(a, maxValue);
            }
        }

    }
    int index = -1;
    public void EnableNextBend()
    {
        index++;
        //if (index != 0)
        //    wotArray[index - 1].checkBalls = false;
        if (index < 3)
        {
            wotArray[index].StartCoroutine("CountBallsOverBridge");
        }
    }
    public void PlayGoldRainConfetti()
    {
        goldRainConfetti.Play();
    }
    //public void EnableNextBend()
    //{
    //    print("next bend enabled");
    //    index++;
    //    //if (index != 0)
    //    //    wotArray[index - 1].checkBalls = false;
    //    if (index < 3)
    //    {
    //        //if(index>0)
    //        //bottomPlateTriggers[index-1].SetActive(false);
    //        //bottomPlateTriggers[index].SetActive(true);
    //        //wotArray[index].checkBalls = true;
    //        wotArray[index].StartCoroutine("CheckWeight");
    //    }
    //    else
    //    {
    //        print("all over");
    //    }
    //}
    private int DecideMaxValue()
    {
        //if (gameManager.currentLevel == 1)
        //    return 600;
        //else if (gameManager.currentLevel == 2)
        //    return 600;
        //else
            return levelGenerator.totalCount;
    }
}