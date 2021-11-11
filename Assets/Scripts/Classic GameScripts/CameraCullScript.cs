//Normal
//-14.53751, -10.95476,  26.6016
//2.717,  48.358, -6.961
//16,  15, 3

//Crative
//-21, -10.5, 20.8
//2.717,  48.358, -6.961
//16,  15, 3


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraCullScript : MonoBehaviour
{
    LevelGenerator levelGenerator;
    private BottomTriggerScript bottomTriggerScript;
    [SerializeField] private CameraFollow cameraFollow;
    private HeadScript headScript;
    [HideInInspector] public List<GameObject> culledObjs = new List<GameObject>();
    bool cull;
    private void Awake()
    {
        Refs refs = FindObjectOfType<Refs>(); 
        levelGenerator= refs.levelGenerator;
        headScript= refs.headScript;
        bottomTriggerScript = refs.bottomTriggerScript;
#if !UNITY_EDITOR
test = false;
#endif
    }

    void Start()
    {
        cull = true;
        if (test)
        {
            StartCoroutine("kkk");
        }
    }
    private IEnumerator kkk()
    {
        yield return null;
        yield return null;
        yield return null;
        FindObjectOfType<HeadManager>().transform.position = FindObjectOfType<BigPitScript>().transform.position + Vector3.up * 10-Vector3.forward*6;
        //EditorApplication.isPaused = true;

    }
    private void OnEnable()
    {
        levelGenerator.SetBasket += OnBasketSet;
        cameraFollow.CameraRotated += OnCameraRotated;
    }
    private void OnDisable()
    {
        levelGenerator.SetBasket -= OnBasketSet;
        cameraFollow.CameraRotated -= OnCameraRotated;
    }
    public bool test;

    public int testCount;
    public int missingCount;
    public GameObject ballPrefab;
    public Transform bullParent;
    public int lessFactor = 5;

    public int totalBalls = 0;
    public int totalBallsActual = 0;
    private void OnCameraRotated()
    {
        if (!test)
        {
            StartCoroutine(ReposAll());
        }
        else
        {

            bullParent = new GameObject("BullParent").transform;
            Queue<GameObject> smallBalls = new Queue<GameObject>();
            for (int i = 0; i < testCount; i++)
            {
                GameObject obj = Instantiate(ballPrefab, lastPositions[i], Quaternion.identity, bullParent);
                obj.layer = 12;
                obj.SetActive(true);
                smallBalls.Enqueue(obj);
                totalBallsActual++;

            }
            for (int i = 0; i < missingCount; i++)
            {

                GameObject obj = smallBalls.Dequeue();
                Destroy(obj);
                totalBallsActual++;
            }
            totalBalls = Mathf.Min(testCount - lessFactor, totalBallsActual);
            StartCoroutine("jayay");

        }
    }
    private IEnumerator jayay()
    {
        yield return null;
        yield return null;
        WeightOverTrigger.totalBallsFallen = testCount;
        WeightOverTrigger.totalBallsFallen90pct = (int)((testCount) * 0.9f);
    }
    Vector3[] lastPositions;
    //int lastPosIndex = 0;
    //private int lastPosArrayLength;
    private void OnBasketSet()
    {
        lastPositions = levelGenerator.lastPositions;
        //lastPosArrayLength = lastPositions.Length-1;
    }
    //private Vector3 GetPosition()
    //{
    //    Vector3 pos = lastPositions[lastPosIndex];
    //    return pos;
    //}
    private GameObject obj;
    private void OnTriggerEnter(Collider other)
    {
        if (cull)
        {
            obj = other.gameObject;
            if (obj.activeSelf)
            {
                //if (lastPosIndex < lastPosArrayLength)//lastPosArrayLength is actual value less one
                //{
                    //obj.transform.localPosition = lastPositions[lastPosIndex];
                    //obj.SetActive(false);
                    //culledObjs.Add(obj);
                    //lastPosIndex++;
                
                obj.transform.localPosition = lastPositions[culledObjs.Count];
                    obj.SetActive(false);
                    culledObjs.Add(obj);
                    //lastPosIndex++;
                //}
            }
        }
    }
    private IEnumerator ReposAll()
    {
        //int minusPoints = headScript.minusPoints;
        int turnOnCount = culledObjs.Count;
        //if (minusPoints < culledObjs.Count)
        //{
        //    turnOnCount = culledObjs.Count - minusPoints;
        //    //print("not turning on minusPoints "+minusPoints+" only turning on "+turnOnCount);
        //}
        //else
        //{
        //    turnOnCount = 0;
        //    //print("Not truning anything at all");
        //}
        Rigidbody rb;
        cull = false;
        //int length = culledObjs.Count;
        if (turnOnCount > 0)
        {
            int minCheckPoint = turnOnCount / 10;
            int checkPoint = minCheckPoint;
            Vector3 platformDirection = levelGenerator.platformDirection;
            for (int i = 0; i < turnOnCount; i++)
            {
                if (culledObjs[i] != null)
                {
                    culledObjs[i].SetActive(true);
                    rb = culledObjs[i].GetComponent<Rigidbody>();
                    if (rb != null)
                        rb.AddForce(platformDirection * 15, ForceMode.Impulse);

                    if (i == checkPoint)
                    {
                        checkPoint *= 2;
                        yield return null;
                    }
                }
            }
        }
        bottomTriggerScript.ResetSoundOpt();
    }
    public List<GameObject> realFar = new List<GameObject>();
    public void DestroyCulledObects(int count)
    {
        int num = 0;
        if (count < culledObjs.Count)
        {
            num = count;
        }
        else
        {
            num = culledObjs.Count;
        }
        GameObject obj;

        for (int i = num-1; i>-1; i--)
        {
            obj = culledObjs[culledObjs.Count - 1];
            culledObjs.RemoveAt(culledObjs.Count - 1);
            Destroy(obj);
        }
    }
}
