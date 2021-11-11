using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
public class WeightOverTrigger : MonoBehaviour
{
    public static int lessFactor;
    public SkinnedMeshRenderer smr;
    public MeshCollider mc;
    public TextMeshPro counterText;
    private int bridgeCapacity;//lessFactor included
    [HideInInspector] public bool checkBalls;
    private BigPitScript bigPitScript;
    int mask;
    Bounds b;
    public float waitTime = 1;
    [HideInInspector] public bool updateStar;
    [HideInInspector] public int index;
    [HideInInspector] public static int totalBallsFallen = int.MaxValue;
    [HideInInspector] public static int totalBallsFallen90pct = int.MaxValue;
    private CameraFollow cameraFollow;
    int multiplier;
    WaitForSeconds waitTimeWfs;
    private bool gameCompleted;
    [HideInInspector] public int totalCount;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(index == 2)
            {
                int ballsOnBridge = Physics.OverlapBoxNonAlloc(transform.position, b.extents, overlapSpheres, transform.rotation, mask);
                print("ballsOnBridge  "+gameObject.name+" " + ballsOnBridge);
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            print("********* "+index);
            print("bridgeCapacity "+bridgeCapacity);
            print("totalBallsCollected "+ totalBallsFallen);
            print("totalCount " + totalCount);
        }
    }

    private void Awake()
    {
        waitTimeWfs = new WaitForSeconds(waitTime);
        Refs refs = FindObjectOfType<Refs>();
        cameraFollow = refs.cameraFollow;
        //b = GetComponent<MeshRenderer>().bounds;
        //    print(" b "+b.size);
        mask = LayerMask.GetMask("SmallBall", "SmallBallNoSelf");
        bigPitScript = refs.bigPitScript;
        mesh = new Mesh();

        smr.SetBlendShapeWeight(0, 0);
        mesh = new Mesh();
        smr.BakeMesh(mesh);
        mc.sharedMesh = mesh;
    }

    public void CalculateBounds()//calculate bounds after scale is changed from bigpitscript
    {
        b = GetComponent<MeshRenderer>().bounds;
    }
    private void OnEnable()
    {
        cameraFollow.CameraRotated += OnCameraRotated;
    }
    private void OnDisable()
    {
        cameraFollow.CameraRotated -= OnCameraRotated;
    }
    bool cameraRotated;
    private void OnCameraRotated()
    {
        cameraRotated = true;
    }
    public void Initialize(int wl, int actualValue = -1)
    {
        bridgeCapacity = wl;
        if(actualValue == -1)
        counterText.text = wl.ToString();
        else
            counterText.text = actualValue.ToString();
    }
    Mesh mesh;
    int strength;//strength of plate
    private IEnumerator SetBlendWeight(float weight, int count)
    {

        yield return waitTimeWfs;
        strength = bridgeCapacity - count;
        counterText.text = strength.ToString();
        if (index == 2)
        {
            counterText.text = (strength).ToString();
        }
        if (updateStar)
        {
            UIManager.Instance.stars[index].fillAmount = weight / 100f;
        }
        if (smr != null)
        {
            if (mc != null)
            {
                smr.SetBlendShapeWeight(0, weight);
                mesh = new Mesh();
                smr.BakeMesh(mesh);
                mc.sharedMesh = mesh;
            }
        }
        //if finished before capcity
        if(weight!=100 &&  count == totalBallsFallen)//if all balls dropped are colected in sheets, sometimes one or two misses in that case wait for sometimes and if repaet count is 20 then finish game
        {
            if (!gameCompleted)
            {
                gameCompleted = true;
                multiplier = index + 1;
                if (cameraFollow.printSuccessType)
                {
                    print("No breaking but count finished");
                }
                Invoke("GameWon", waitTime + 2);
                yield break;
            }

        }

        else if (weight == 100)
        {
            //EditorApplication.isPaused = true;
            updateStar = false;
            UIManager.Instance.PlayStarConfetti(index);
            //AudioManager.Instance.PlayClingSound();//
            StopAllCoroutines();

            bigPitScript.EnableNextBend();
            transform.parent.parent.gameObject.SetActive(false);
            ParticleSystem clouds = bigPitScript.clouds;
            clouds.transform.localScale = Vector3.one * 5;
            clouds.transform.localPosition = transform.parent.position + Vector3.up * 5;
            clouds.Play();
            AudioManager.Instance.PlayBottomBreakSound();
            //ScoreManager.Instance.AddScore(15);
            checkBalls = false;
            Transform parent = transform.parent.parent;
            Vector3 explosionPos = parent.position - Vector3.up * 4 - Vector3.right * 1;
            Collider[] cols = new Collider[100];
            Physics.OverlapSphereNonAlloc(explosionPos, 4, cols, mask);
            Rigidbody rb;
            if(index!=2)
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i] != null)
                {
                    rb = cols[i].GetComponent<Rigidbody>();
                    rb.AddExplosionForce(800, explosionPos, 50);//
                }
            }
            if(index == 2)
            {
                if (!gameCompleted)
                {
                    gameCompleted = true;
                    cameraFollow.FinalLookAt();
                    multiplier = index + 2;
                    if (cameraFollow.printSuccessType)
                    {
                        print("Final Win");
                    }
                    Invoke("GameWon", (waitTime + 4));
                    yield break;
                }
            }
        }
    }
    [HideInInspector] public int bendCount = 15;
    int target = 0;
    int value1 = 0;
    int ballsOnBridge;
    private bool countedAllBallsFallen;
    Collider[] overlapSpheres;
    int ballsOnBridgePrevious = -1;
    int targetPrevious = -1;
    int repeatCount = 0;
    public int maxTr;
    public IEnumerator CountBallsOverBridge()
    {
        int unitValue = bridgeCapacity / bendCount;
        value1 = unitValue;
        float weightValue;
        float previousValue = float.MinValue;
        overlapSpheres = new Collider[totalCount];
        if(index == 1)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else if(index == 2)
        {
            yield return new WaitForSeconds(0.4f);
        }
        while (true)
        {
            if (!countedAllBallsFallen)//
            {
                ballsOnBridge = Physics.OverlapBoxNonAlloc(transform.position, b.extents, overlapSpheres, transform.rotation, mask);
                target = Mathf.Min(ballsOnBridge, bridgeCapacity);
                
                if (target == totalBallsFallen)
                {
                        countedAllBallsFallen = true;
                        CameraCullScript cs = FindObjectOfType<CameraCullScript>();
                        if (cs != null)
                            if (cs.test)
                                if (target == 0)
                                    countedAllBallsFallen = false;
                    
                }
                if (value1 >= totalBallsFallen90pct)//start waiting after 90 pct
                {
                    if (cameraRotated)//camera turned
                    {
                        if (target < bridgeCapacity && target  == targetPrevious)
                        {
                            repeatCount++;
                            if (repeatCount == 20)
                            {
                                if (!gameCompleted)
                                {
                                    if (cameraFollow.printSuccessType)
                                    {
                                        print("Waited for 20 iteration");
                                    }
                                    //if (index == 2)
                                    //{
                                    if (totalBallsFallen == bridgeCapacity)
                                    {
                                        StartCoroutine(SetBlendWeight(100, totalCount));
                                        yield break;
                                    }
                                    //}
                                    else
                                    {
                                        gameCompleted = true;
                                        multiplier = index + 1;
                                        Invoke("GameWon", waitTime + 2);
                                        yield break;
                                    }

                                }
                            }
                        }
                        else
                        {
                            repeatCount = 0;
                        }
                    }
                }
                   
                ballsOnBridgePrevious = ballsOnBridge;
                targetPrevious = target;
            }

            value1 += unitValue;
            value1 = Mathf.Min(value1, target);
            weightValue = value1 * 100f / bridgeCapacity;
            //print("setting weightValue "+ weightValue+ " bridgeCapacity "+ bridgeCapacity+ " value1 "+ value1);
            if (weightValue > previousValue)
            {
                StartCoroutine(SetBlendWeight(weightValue, value1));
                previousValue = weightValue;
            }
            yield return wfs005;
            if(countedAllBallsFallen && target == value1)
            {
                yield break;
            }
        }
    }
    WaitForSeconds wfs005 = new WaitForSeconds(0.05f);
    private void GameWon()
    {//
        Time.timeScale = 1;
        FindObjectOfType<GameManager>().GameWon(multiplier);
    }
    private void PlayGoldRainConfetti()
    {
        bigPitScript.PlayGoldRainConfetti();

    }
}
