//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//public class LevelGenerator : MonoBehaviour
//{
//    private GameManager gameManager;
//    private HeadScript headScript;

//    public bool greenRed;
//    public bool straightOnly;
//    public bool slantedOnly;
//    public float baseGap = 10;
//    public float forwardGap = 20;
//    private int mulCount = 1;
//    public GameObject basicTwoMulPrefab;
//    public GameObject bigOneMulPrefab;
//    public GameObject poleVaultPrefab;
//    public GameObject horizontalTwoMulPrefab;
//    public GameObject butterflyPrefab;
//    public GameObject upwayPrefab;
//    public GameObject pathEndTriggerPrefab;
//    public GameObject sphereMulPrefab;
//    public GameObject sphereMulButterflyPrefab;
//    [HideInInspector] public float platformZLength;
//    private Transform staticRoot;
//    private CameraFollow cameraFollow;

//    public Transform platform;
//    float platformSlope;
//    //Vector3 platformOrigin;
//    float platformOriginZ = -50;
//    float tanAngle;
//    private Transform mulParent;
//    public Transform basket;
//    private Vector3 reSpawnPos;
//    [HideInInspector] public int totalCount = 50;

//    [HideInInspector] public Vector3 platformDirection;
//    int[] maxValueArray;
//    public event System.Action SetBasket;
//    private Material[] mats;
//    private int[] maxValueColorIndexArray;
//    Vector3 position = new Vector3(0, 0, 15);
//    public Material defaultMat;
//    public Material greenMat;
//    public Material redMat;
//    public bool sphereMul = true;
//    //public int div;
//    //public int fullValue;
//    private void Awake()
//    {
//#if UNITY_EDITOR

//        QualitySettings.shadows = ShadowQuality.HardOnly;
//#endif
//        Refs refs = FindObjectOfType<Refs>();
//        gameManager = refs.gameManager;
//        headScript = refs.headScript;
//        staticRoot = new GameObject("StaticRoot").transform;
//        cameraFollow = FindObjectOfType<CameraFollow>();
//#if !UNITY_EDITOR
//        straightOnly = false;
//        slantedOnly = false;
//#endif
//    }
//    private void OnEnable()
//    {
//        gameManager.LevelStartedEvent += OnLevelStarted;
//    }
//    private void OnDisable()
//    {
//        gameManager.LevelStartedEvent -= OnLevelStarted;
//    }
//    private void Start()
//    {
//        mats = GameDataClass.Instance.mats;
//        //int[] a = GenerateRandomNumbers(div,fullValue);
//        //JU.PrintArray(a);
//    }
//    private void Update()
//    {
//        //if (Input.GetKeyDown(KeyCode.Q))
//        //{
//        //    int[] a = GenerateRandomNumbers(div, fullValue);
//        //    JU.PrintArray(a);
//        //}
//    }
//    private void OnLevelStarted()
//    {
//        //platformOrigin = platform.position;
//        platformSlope = platform.transform.eulerAngles.x;
//        tanAngle = Mathf.Tan(Mathf.Deg2Rad * platformSlope);

//        platformDirection = new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * -platformSlope), Mathf.Cos(Mathf.Deg2Rad * -platformSlope));

//        SetMulCount();
//        CreateMuls();
//        CalculatePositions();
//        headScript.SetPool(maxValueArray, mulCount, maxValueColorIndexArray);
//        SetBasket?.Invoke();
//    }
//    private int[] GenerateMaxValuesAndMultipliers(int count)
//    {
//        int[] a = new int[count];
//        if (count == 10)
//        {
//            int[] b = GenerateRandomNumbers(3, 15);
//            for (int i = 0; i < b.Length; i++)
//            {
//                a[i] = b[i];
//            }
//            a[3] = -4;
//            b = GenerateRandomNumbers(2, 20);
//            for (int i = 0; i < 2; i++)
//            {
//                a[4 + i] = b[i];
//            }
//            a[6] = -3;
//            b = GenerateRandomNumbers(2, 60);
//            for (int i = 0; i < 2; i++)
//            {
//                a[7 + i] = b[i];
//            }
//            a[9] = -2;
//        }
//        else if (count == 3)
//        {//totalCount = 120
//            a = new int[3] { 90, -2, -3 };
//        }
//        else if (count == 6)
//        {
//            if (gameManager.currentLevel == 2)
//            {
//                a = new int[6] { 85, -2, 50, 30, -2, 55 };

//            }
//            else if (gameManager.currentLevel == 3)
//            {
//                a = new int[6] { 50, 50, -2, 40, 35, -2 };
//            }
//            else
//            {
//                //totalCount = 510
//                int[] y = GenerateRandomNumbers(2, 20);
//                a = new int[6] { Random.Range(12, 15), -2, y[0], -3, y[1], -3 };

//            }

//        }
//        int totalCountTemp = 0;
//        for (int i = 0; i < a.Length; i++)
//        {
//            if (a[i] < 0)
//            {
//                totalCountTemp *= Mathf.Abs(a[i]);
//            }
//            else
//            {
//                totalCountTemp += a[i];
//            }
//        }
//        totalCount = totalCountTemp;
//        //JU.PrintArray(a);
//        return a;
//    }
//    private int[] GenerateRandomNumbers(int divisions, int totalValue)
//    {
//        totalValue -= 2 * divisions;
//        List<float> rnds = new List<float>();
//        int weightedSum = 0;
//        int addedSoFar = 0;
//        int minRange = Mathf.Max((int)(totalValue / divisions * 0.5f), 1);
//        int maxRange = (int)(totalValue / divisions * 2.5f);
//        for (int i = 0; i < divisions; i++)
//        {
//            int a = Random.Range(minRange, maxRange);
//            rnds.Add(a);
//            weightedSum += a;
//        }
//        for (int i = 0; i < divisions; i++)
//        {
//            rnds[i] = rnds[i] / weightedSum;
//        }
//        int[] finalValues = new int[divisions];
//        for (int i = 0; i < divisions; i++)
//        {
//            if (i == divisions - 1)
//            {
//                finalValues[i] = totalValue - addedSoFar;
//                addedSoFar += finalValues[i];
//            }
//            else
//            {
//                finalValues[i] = Mathf.CeilToInt(rnds[i] * totalValue);
//                addedSoFar += finalValues[i];
//            }
//        }
//        //JU.PrintArray(finalValues);
//        for (int i = 0; i < finalValues.Length; i++)
//        {
//            finalValues[i] += 2;
//        }
//        bool repeat = false;
//        for (int i = 0; i < finalValues.Length; i++)
//        {
//            if (finalValues[i] < 2)
//            {
//                repeat = true;
//                break;
//            }
//        }
//        if (repeat)
//            finalValues = GenerateRandomNumbers(divisions, totalValue);
//        return finalValues;
//    }
//    [HideInInspector] public Vector3 lastPos;
//    MulType prevMulType = MulType.none;
//    Vector3 forwardSlope = new Vector3(0, -90, -30);
//    Vector3 backSlope = new Vector3(0, -90, 30);
//    public void CreateMuls()
//    {
        
//        //if (cameraFollow.frontView)
//        //{
//        //    forwardSlope.y = -90;
//        //    backSlope.y = -90;
//        //}

//        mulParent = new GameObject("MulParent").transform;

//        //maxValueArray = GenerateRandomNumbers(mulCount, totalCount);
//        maxValueArray = GenerateMaxValuesAndMultipliers(mulCount);

//        int upperLimit = 7;
//        if (gameManager.currentLevel < 3)     //straight only
//            upperLimit = 1;
//        else if (gameManager.currentLevel < 5)// straight, tough only
//            upperLimit = 2;
//        else if (gameManager.currentLevel < 7)// straight, tough, flagLike only
//            upperLimit = 3;
//        else if (gameManager.currentLevel < 9)// straight, tough, flagLike, horizontal only
//            upperLimit = 4;
//        else if (gameManager.currentLevel < 11)// straight, tough, flagLike, horizontal, forward only
//            upperLimit = 5;
//        else if (gameManager.currentLevel < 13)// straight, tough, flagLike, horizontal, forward, backward only
//            upperLimit = 6;
//        else if (gameManager.currentLevel < 15)// straight, tough, flagLike, horizontal, forward, backward , upwayonly
//            upperLimit = 7;
//        if (gameManager.currentLevel > 20 && gameManager.currentLevel % 5 == 0)
//        {
//            int rnd = ((gameManager.currentLevel / 5) % 7) - 1;
//            if (rnd == -1)
//                rnd = 6;
//            if (rnd < 1)
//            {
//                for (int i = 0; i < mulCount - 1; i++)
//                {
//                    SpawnMulType(MulType.tough, position, i);
//                }
//            }
//            else if (rnd < 2)
//            {
//                for (int i = 0; i < mulCount - 1; i++)
//                {
//                    SpawnMulType(MulType.forward, position, i);
//                }
//            }
//            else if (rnd < 3)
//            {
//                for (int i = 0; i < mulCount - 1; i++)
//                {
//                    SpawnMulType(MulType.flagLike, position, i);
//                }
//            }
//            else if (rnd < 4)
//            {
//                for (int i = 0; i < mulCount - 1; i++)
//                {
//                    SpawnMulType(MulType.backward, position, i);
//                }
//            }
//            else if (rnd < 5)
//            {
//                for (int i = 0; i < mulCount - 1; i++)
//                {
//                    SpawnMulType(MulType.upway, position, i);
//                }
//            }
//            else if (rnd < 6)
//            {
//                for (int i = 0; i < mulCount - 1; i++)
//                {
//                    SpawnMulType(MulType.horizontal, position, i);
//                }
//            }
//            else if (rnd < 7)
//            {
//                for (int i = 0; i < mulCount - 1; i++)
//                {
//                    SpawnMulType(MulType.horizontal, position, i);
//                }
//            }
//        }
//        else
//        {
//            for (int i = 0; i < mulCount - 1; i++)
//            {
//                int rand = 0;
//                #region SelectedItem
//                //rand = Random.Range(0, 2);
//                if (straightOnly)
//                {
//                    rand = 0;
//                }
//                else if (slantedOnly)
//                {
//                    rand = Random.Range(0, 1);
//                    if (rand == 0)
//                    {
//                        rand = 4;
//                    }
//                    else if (rand == 1)
//                    {
//                        rand = 5;
//                    }
//                    //else if (rand == 2)
//                    //{
//                    //    rand = 0;
//                    //}
//                }
//                else
//                {
//                    rand = Random.Range(0, upperLimit);
//                }
//                #region ToCheckBetweenTwo
//                //if (i == 0)
//                //{
//                //    //position += Vector3.forward * forwardGap;
//                //    //SpawnTwoMulWithObstacles(MulType.normal, position, i, Vector3.zero);

//                //    //position += Vector3.forward * forwardGap;
//                //    //SpawnTwoMulWithObstacles(MulType.tough, position, i, Vector3.zero);

//                //    //position += Vector3.forward * forwardGap;
//                //    //SpawnTwoMulWithObstacles(MulType.upway, position, i, Vector3.zero);

//                //    //position += Vector3.forward * forwardGap;
//                //    //SpawnSingleMul(MulType.flagLike, position, i);


//                //    //if (prevMulType == MulType.forward)
//                //    //{
//                //    //    position += Vector3.forward * 18;
//                //    //}
//                //    //else
//                //    //{
//                //    //    position += Vector3.forward * 10;
//                //    //}
//                //    //SpawnTwoMulWithObstacles(MulType.forward, position, i, forwardSlope);


//                //    if (prevMulType == MulType.forward)
//                //    {
//                //        position += Vector3.forward * 32;
//                //    }
//                //    else
//                //    {
//                //        position += Vector3.forward * 25;
//                //    }
//                //    SpawnTwoMulWithObstacles(MulType.backward, position, i, backSlope);

//                //    ////position += Vector3.forward * 10;
//                //    //SpawnTwoMulWithObstacles(MulType.horizontal, position, i, Vector3.zero);
//                //    continue;
//                //}
//                //else if (i == 1)
//                //{
//                //    if(prevMulType == MulType.forward)
//                //    {
//                //        position += Vector3.forward * 25;
//                //    }
//                //    else
//                //    {
//                //        position += Vector3.forward * forwardGap;
//                //    }
//                //    SpawnTwoMulWithObstacles(MulType.normal, position, i, Vector3.zero);

//                //    //position += Vector3.forward * forwardGap;
//                //    //SpawnTwoMulWithObstacles(MulType.tough, position, i, Vector3.zero);

//                //    //position += Vector3.forward * forwardGap;
//                //    //SpawnTwoMulWithObstacles(MulType.upway, position, i, Vector3.zero);

//                //    //position += Vector3.forward * forwardGap;
//                //    //SpawnSingleMul(MulType.flagLike, position, i);


//                //    //if (prevMulType == MulType.forward)
//                //    //{
//                //    //    position += Vector3.forward * 18;
//                //    //}
//                //    //else
//                //    //{
//                //    //    position += Vector3.forward * 10;
//                //    //}
//                //    //SpawnTwoMulWithObstacles(MulType.forward, position, i, forwardSlope);


//                //    //if (prevMulType == MulType.forward)
//                //    //{
//                //    //    position += Vector3.forward * 32;
//                //    //}
//                //    //else
//                //    //{
//                //    //    position += Vector3.forward * 25;
//                //    //}
//                //    //SpawnTwoMulWithObstacles(MulType.backward, position, i, backSlope);

//                //    ////position += Vector3.forward * 10;
//                //    //SpawnTwoMulWithObstacles(MulType.horizontal, position, i, Vector3.zero);
//                //    continue;
//                //}
//                //else if (i == 2)
//                //{
//                //    position += Vector3.forward * forwardGap;
//                //    SpawnTwoMulWithObstacles(MulType.normal, position, i, Vector3.zero);

//                //    //SpawnTwoMulWithObstacles(MulType.tough, position, i, Vector3.zero);
//                //    //position += Vector3.forward * forwardGap;

//                //    //SpawnTwoMulWithObstacles(MulType.upway, position, i, Vector3.zero);
//                //    //position += Vector3.forward * forwardGap;

//                //    //SpawnSingleMul(MulType.flagLike, position, i);
//                //    //position += Vector3.forward * forwardGap;

//                //    //position += Vector3.forward * 5;
//                //    //SpawnTwoMulWithObstacles(MulType.forward, position, i, forwardSlope);

//                //    //position += Vector3.forward * 6;
//                //    //SpawnTwoMulWithObstacles(MulType.backward, position, i, backSlope);
//                //    //position += Vector3.forward * (forwardGap + 3);

//                //    ////position += Vector3.forward * 10;
//                //    //SpawnTwoMulWithObstacles(MulType.horizontal, position, i, Vector3.zero);
//                //    //position += Vector3.forward * (forwardGap + 3);
//                //    continue;
//                //}
//                #endregion
//                //continue;
//                #endregion
//                //if (i < 3)
//                //{
//                //    position = SetDistance(MulType.normal, position);
//                //    SpawnTwoMulWithObstacles(MulType.normal, position, i, Vector3.zero);
//                //    continue;
//                if ((gameManager.currentLevel < 3) || i == 0 || rand == 0)
//                {
//                    if (gameManager.currentLevel == 1)
//                    {
                        
//                        SpawnMulType(MulType.bigOne, position, i);
//                    }
//                    else
//                    {
//                        SpawnMulType(MulType.normal, position, i);
//                    }

//                }
//                else if (rand ==1)
//                {
//                    SpawnMulType(MulType.tough, position, i);
//                }
//                else if (rand ==2)//forward
//                {
//                    SpawnMulType(MulType.flagLike, position, i);
//                }
//                else if (rand ==3)//backward
//                {
//                    SpawnMulType(MulType.horizontal, position, i);
//                }
//                else if (rand ==4)
//                {
//                    SpawnMulType(MulType.forward, position, i);
//                }
//                else if (rand ==5)//horizontal
//                {
//                    SpawnMulType(MulType.backward, position, i);
//                }
//                else if (rand ==6)
//                {
//                    SpawnMulType(MulType.upway, position, i);
//                }
//                #region All Items
//                //}
//                //else if (i == 3)
//                //{
//                //    position = SetDistance(MulType.normal, position);
//                //    SpawnTwoMulWithObstacles(MulType.normal, position, i, Vector3.zero);
//                //}
//                //else if (i == 4)
//                //{
//                //    position = SetDistance(MulType.tough, position);
//                //    SpawnTwoMulWithObstacles(MulType.tough, position, i, Vector3.zero);
//                //}
//                //else if (i == 5)
//                //{
//                //    position = SetDistance(MulType.upway, position);
//                //    SpawnTwoMulWithObstacles(MulType.upway, position, i, Vector3.zero);
//                //}
//                //else if (i == 6)
//                //{
//                //    position = SetDistance(MulType.flagLike, position);
//                //    SpawnSingleMul(MulType.flagLike, position, i);
//                //}
//                //else if (i == 7)
//                //{
//                //    position = SetDistance(MulType.forward, position);
//                //    SpawnTwoMulWithObstacles(MulType.forward, position, i, forwardSlope);
//                //}
//                //else if (i == 8)
//                //{
//                //    position = SetDistance(MulType.backward, position);
//                //    SpawnTwoMulWithObstacles(MulType.backward, position, i, backSlope);
//                //}
//                //else if (i == 9)
//                //{
//                //    position = SetDistance(MulType.horizontal, position);
//                //    SpawnTwoMulWithObstacles(MulType.horizontal, position, i, Vector3.zero);
//                //}
//                #endregion
//            }
//        }

//        if (gameManager.currentLevel < 5)
//        {
//            position = SetDistance(MulType.normal, position);
//            SpawnBigOne(MulType.bigOne, position, mulCount - 1, (gameManager.currentLevel < 4) ? 1 : 0.3f);
//        }
//        else
//        {
//            SpawnMulType(MulType.butterfly, position, mulCount - 1);
//        }

//        position.z += 20;
//        lastPos = new Vector3(position.x, -(position.z - platformOriginZ) * tanAngle, position.z);
//        platformZLength = (position.z - platform.transform.localPosition.z) * (1 / Mathf.Cos(Mathf.Deg2Rad * platformSlope));
//        platform.localScale = new Vector3(platform.localScale.x, platform.localScale.y, platformZLength);
//        basket.transform.position = lastPos;

//        //reSpawnPos = new Vector3(position.x, -(position.z-5 - platformOriginZ) * tanAngle+0.5f, position.z-5)+Vector3.up*3;
//        reSpawnPos = new Vector3(position.x, -(position.z - 5 - platformOriginZ) * tanAngle + 0.5f, position.z - 5) + Vector3.up * 4;
//        //smallBall.position = reSpawnPos;
//        //spawnBox.gameObject.SetActive(true);
//        //pathEndTrigger = Instantiate(pathEndTriggerPrefab);
//        //pathEndTrigger.transform.SetParent(mulParent);
//        //pathEndTrigger.transform.position = lastPos;

//        if (gameManager.currentLevel < 6)
//            DestroyAllObstacles();
//        platform.SetParent(staticRoot);
//        //mulParent.SetParent(staticRoot);
//        //basket.SetParent(staticRoot);
//        StaticBatchingUtility.Combine(staticRoot.gameObject);
//    }
//    private Vector3 SpawnTwoMulWithObstacles(MulType mulType, Vector3 position, int index)
//    {
//        Vector3 eulerAngles = GetMulSlope(mulType);
//        int bigValue = maxValueArray[index];
//        float bottomY = -(position.z - platformOriginZ) * tanAngle + baseGap;
//        position.y = bottomY;
//        int actualValueIndex = Random.Range(0, 2);


//        int smallValue = Random.Range(1, bigValue / 2);//
//        int bottomValue;
//        int topValue;
//        if (actualValueIndex == 0)
//        {
//            bottomValue = bigValue;
//            topValue = smallValue;
//        }
//        else
//        {
//            bottomValue = smallValue;
//            topValue = bigValue;
//        }
//        GameObject prefab = basicTwoMulPrefab;
//        if (mulType == MulType.flagLike)
//        {
//            prefab = poleVaultPrefab;
//        }
//        else if (mulType == MulType.horizontal)
//        {
//            prefab = horizontalTwoMulPrefab;
//        }
//        else if (mulType == MulType.upway)
//        {
//            prefab = upwayPrefab;
//        }
//        GameObject twoMulObj = Instantiate(prefab, position, Quaternion.identity);
//        TwoMulData twoMulData = twoMulObj.GetComponent<TwoMulData>();


//        int topColorIndex = Random.Range(0, 5);
//        int bottomColorIndex;
//        do
//        {
//            bottomColorIndex = Random.Range(0, 5);
//        } while (bottomColorIndex == topColorIndex);
//        if (actualValueIndex == 0)
//        {
//            maxValueColorIndexArray[index] = bottomColorIndex;
//        }
//        else if (actualValueIndex == 1)
//        {
//            maxValueColorIndexArray[index] = topColorIndex;

//        }
//        //if random colors
//        //twoMulData.SetMulValues(index, bottomValue, mats[bottomColorIndex], bottomColorIndex, topValue, mats[topColorIndex], topColorIndex);
//        //if green-red colors

//        if (topValue > bottomValue)
//        {
//            if (greenRed)
//                twoMulData.SetMulValues(index, -bottomValue, redMat, bottomColorIndex, topValue, greenMat, topColorIndex);
//            else
//                twoMulData.SetMulValues(index, bottomValue, defaultMat, bottomColorIndex, topValue, defaultMat, topColorIndex, 1);//0 = white text, 1 = black text


//        }
//        else
//        {
//            if (greenRed)
//                twoMulData.SetMulValues(index, bottomValue, greenMat, bottomColorIndex, -topValue, redMat, topColorIndex);
//            else
//                twoMulData.SetMulValues(index, bottomValue, defaultMat, bottomColorIndex, topValue, defaultMat, topColorIndex, 1);//0 = white text, 1 = black text


//        }

//        if (mulType == MulType.horizontal)
//        {
//            twoMulData.ReposHorizontalMuls();
//        }
//        else if (mulType == MulType.upway)
//        {
//            twoMulData.ReposHorizontalMuls();
//        }
//        else if (mulType == MulType.tough)
//        {
//            twoMulData.SetTough();
//        }
//        twoMulObj.transform.eulerAngles = eulerAngles;
//        twoMulObj.transform.SetParent(mulParent);
//        if (cameraFollow.frontView)
//        {
//            if (mulType == MulType.normal/* || mulType == MulType.upway*/|| mulType == MulType.tough)
//            {
//                twoMulObj.transform.eulerAngles = Vector3.up * -90;//
//            }
//        }
//        prevMulType = mulType;
//        return position;
//    }
//    private GameObject SpawnSingleMul(MulType mulType, Vector3 position, /*float height, */int mulVerticalIndex)//Biggest mul, useMaxValue is true
//    {
//        //this.position = SetDistance(mulType, this.position);
//        int mulValue = maxValueArray[mulVerticalIndex];
//        float bottomY = -(position.z - platformOriginZ) * tanAngle + baseGap;
//        position.y = bottomY;
//        //print(0);
//        GameObject mul;
//        if (mulType == MulType.butterfly)
//        {
//            mul = Instantiate(butterflyPrefab);
//            position.y += 1;
//            mul.transform.position = position + Vector3.up * 10/*(Random.Range(0, 20))*/;
//            MulMove mulMove = mul.GetComponent<MulMove>();
            
//            mulMove.SetLimits(position.y + 1, position.y + 22);
//            if (gameManager.currentLevel < 4)
//                mulMove.SetStationary();
//        }
//        else
//        {
//            GameObject prefab;
//            if (mulType == MulType.flagLike)
//                prefab = poleVaultPrefab;
//            else if (mulType == MulType.sphere)
//                prefab = sphereMulPrefab;
//            else if (mulType == MulType.upway)
//                prefab = upwayPrefab;
//            else if (mulType == MulType.bigOne)
//                prefab = bigOneMulPrefab;
//            //else if (mulType == MulType.downway)
//            //    prefab = downwayPrefab;
//            else
//            {
//                prefab = null;
//            }

//            mul = Instantiate(prefab);
//            mul.transform.position = position;
//        }
//        MulData mulData = mul.GetComponentInChildren<MulData>();
//        //int mulColorIndex= GetColorIndex(mulValue);
//        int mulColorIndex = Random.Range(0, 5);
//        maxValueColorIndexArray[mulVerticalIndex] = mulColorIndex;

//        //mulData.SetMulValue(mulVerticalIndex, mulValue, mats[mulColorIndex], mulColorIndex);
//        if (greenRed)
//            mulData.SetMulValue(mulVerticalIndex, mulValue, greenMat, mulColorIndex);
//        else
//        {
//            mulData.SetMulValue(mulVerticalIndex, mulValue, defaultMat, mulColorIndex, 1);
//        }
//        mul.transform.SetParent(mulParent);
//        prevMulType = mulType;
//        if (cameraFollow.frontView)
//        {
//            if (mulType == MulType.butterfly || mulType == MulType.bigOne)
//            {
//                mul.transform.eulerAngles = Vector3.up * -90;//
//            }
//        }
//        return mul;
//    }
//    private GameObject SpawnSphere(Vector3 position, int index, bool butterfly = false)
//    {
//        float bottomY = -(position.z - platformOriginZ) * tanAngle + baseGap;
//        position.y = bottomY;
        

//        GameObject prefab = (butterfly) ? sphereMulButterflyPrefab : sphereMulPrefab;
//        GameObject mul = Instantiate(prefab);
//        position.y += 1;
//        mul.transform.position = position + Vector3.up * 10;
//        mul.transform.SetParent(mulParent);
//        MulData mulData = mul.GetComponent<MulData>();
//        mulData.SetMulValue(index, maxValueArray[index], defaultMat, 0, 1);
//        if (butterfly)
//        {
//            MulMove mulMove = mul.GetComponent<MulMove>();
//            mulMove.SetLimits(position.y + 1, position.y + 22);
//            if (gameManager.currentLevel < 4)
//                mulMove.SetStationary();
//        }
//        prevMulType = MulType.sphere;
//        this.position = position;
//        return mul;
//    }
//    private GameObject SpawnBigOne(MulType mulType, Vector3 position, int mulVerticalIndex, float heightScaleFactor = 1)
//    {
//        int mulValue = maxValueArray[mulVerticalIndex];
//        float bottomY = -(position.z - platformOriginZ) * tanAngle + baseGap;
//        position.y = bottomY;
//        //print(0);
//        GameObject mul = Instantiate(bigOneMulPrefab);
//        Transform cubeTransform;
//        BoxCollider bc = mul.GetComponentInChildren<BoxCollider>();
//        if (bc != null)
//        {
//            cubeTransform = bc.transform;
//            Vector3 scale = cubeTransform.localScale;
//            scale.y *= heightScaleFactor;
//            cubeTransform.transform.localScale = scale;
//        }

//        mul.transform.position = position;
//        MulData mulData = mul.GetComponentInChildren<MulData>();
//        //int mulColorIndex= GetColorIndex(mulValue);
//        int mulColorIndex = Random.Range(0, 5);
//        maxValueColorIndexArray[mulVerticalIndex] = mulColorIndex;

//        //mulData.SetMulValue(mulVerticalIndex, mulValue, mats[mulColorIndex], mulColorIndex);
//        if (greenRed)
//            mulData.SetMulValue(mulVerticalIndex, mulValue, greenMat, mulColorIndex);
//        else
//        {
//            mulData.SetMulValue(mulVerticalIndex, mulValue, defaultMat, mulColorIndex, 1);

//        }
//        mul.transform.SetParent(mulParent);
//        prevMulType = mulType;
//        return mul;
//    }
//    private Vector3 SetDistance(MulType mulType, Vector3 position)
//    {
//        if (prevMulType == MulType.upway || prevMulType == MulType.horizontal)
//        {
//            position += Vector3.forward * 5;
//        }
//        if (mulType == MulType.normal || mulType == MulType.sphere || mulType == MulType.sphereButterfly || mulType == MulType.bigOne || mulType == MulType.tough || mulType == MulType.flagLike || mulType == MulType.upway || mulType == MulType.horizontal || mulType == MulType.butterfly)
//        {
//            if (prevMulType == MulType.forward)
//            {
//                position += Vector3.forward * 25;
//                //print("a");
//            }
//            else if (prevMulType != MulType.none)
//            {
//                position += Vector3.forward * forwardGap;
//                //print("a");

//            }
//            if (mulType == MulType.butterfly)
//            {
//                position.z += 7;
//                //print("a");

//            }
//        }//
//        else if (mulType == MulType.forward)
//        {
//            if (prevMulType == MulType.forward)
//            {
//                position += Vector3.forward * 18;
//            }
//            else if (prevMulType != MulType.none)
//            {
//                position += Vector3.forward * 15;
//            }
//        }
//        else if (mulType == MulType.backward)
//        {
//            if (prevMulType == MulType.forward)
//            {
//                position += Vector3.forward * 32;
//            }

//            else if (prevMulType != MulType.none)
//            {
//                position += Vector3.forward * 25;
//            }
//        }

//        this.position = position;
//        return position;
//    }
//    private void SetMulCount()
//    {
//        int avgCount = 60;

//        if (gameManager.currentLevel != 1 && gameManager.currentLevel % 5 == 0)
//        {
//            mulCount = 6;
//            avgCount = 100;//600
//        }
//        else
//        {
//            if (gameManager.currentLevel == 1)
//            {
//                mulCount = 3;
//                avgCount = 190;//570

//            }
//            else if (gameManager.currentLevel < 4)
//            {
//                mulCount = 6;
//                avgCount = 120;//600

//            }
//            //else if (gameManager.currentLevel < 10)
//            //{
//            //    mulCount = 8;
//            //    avgCount = 70;//560

//            //}
//            else
//            {
//                mulCount = 10;
//                avgCount = 60;//600

//            }
//        }
//        maxValueColorIndexArray = new int[mulCount];//
//#if UNITY_EDITOR
//        avgCount = 100;
//#endif
//        //totalCount = (mulCount) * avgCount;
//    }
//    [HideInInspector] public Vector3[] lastPositions;
//    private void CalculatePositions()
//    {
//        float xStartValue = 4.9f;
//        lastPositions = new Vector3[totalCount];
//        Vector3 pos = new Vector3(xStartValue, reSpawnPos.y, reSpawnPos.z);
//        //int xDirection = -1;
//        float gap = 1.1f;
//        int countBackwards = totalCount / (mulCount * 10);
//        int baseCount = countBackwards * 10;
//        for (int k = 0; k < mulCount; k++)
//        {
//            for (int i = 0; i < countBackwards; i++)
//            {
//                for (int j = 0; j < 10; j++)
//                {
//                    lastPositions[j + i * 10 + k * baseCount] = (pos + Vector3.left * j * gap);
//                    //if (j == 9)
//                    //    print("9 "+(pos + Vector3.left * j * gap).x);
//                }
//                pos.z -= gap;
//                pos.x = xStartValue;
//            }
//            pos.x = xStartValue;
//            pos.z = reSpawnPos.z;
//            pos.y += gap;//
//        }
//    }
//    public enum MulType
//    {
//        normal, tough, forward, backward, horizontal, upway, downway, flagLike, butterfly, bigOne, sphere, sphereButterfly, none
//    }
//    private void DestroyAllObstacles()
//    {
//        GameObject[] objs = GameObject.FindGameObjectsWithTag("Obstacle");
//        for (int i = 0; i < objs.Length; i++)
//        {
//            Destroy(objs[i]);
//        }
//    }
//    private void SpawnMulType(MulType mulType, Vector3 position, int mulIndex)
//    {

//        int mulValue = maxValueArray[mulIndex];
//        if (sphereMul)
//        {
//            if (mulValue < 0)
//            {

//                position = SetDistance(MulType.sphereButterfly, position);
//                GameObject mul = SpawnSphere(position, mulIndex, true);
                
//            }
//            else
//            {
//                position = SetDistance(MulType.sphere, position);
//                //SpawnSingleMul(MulType.sphere, position, mulIndex);
//                SpawnSphere(position, mulIndex, false);//
//            }
//        }
//        else
//        {
//            if (mulValue < 0)
//            {
//                position = SetDistance(MulType.butterfly, position);
//                SpawnSingleMul(MulType.butterfly, position, mulIndex);
//            }
//            else
//            {
//                position = SetDistance(mulType, position);
//                bool isSingleType = IsSingleType(mulType);
//                if(isSingleType)
//                SpawnSingleMul(mulType, position, mulIndex);
//                else
//                {
//                    SpawnTwoMulWithObstacles(mulType, position, mulIndex);
//                }
//            }
//        }
//        this.position = position;
//    }
//    private bool IsSingleType(MulType mulType)
//    {
//        return (mulType == MulType.flagLike || mulType == MulType.butterfly || mulType == MulType.bigOne || mulType == MulType.sphere || mulType == MulType.sphereButterfly);
//    }
//    private Vector3 GetMulSlope(MulType mulType)
//    {
//        if (mulType == MulType.forward)
//            return forwardSlope;
//        else if (mulType == MulType.backward)
//            return backSlope;
//        else
//            return Vector3.zero;
//    }
//}
