using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
public class PlayerBallScript : MonoBehaviour
{
    //public PlatformScript platformScript;//to set sound optimization on collision
    private GameManager gameManager;
    [HideInInspector] public int lessFactor;
    //private CameraFollow cameraFollow;
    //private BottomTriggerScript bottomTriggerScript;
    private LevelGenerator levelGenerator;
    public PlayerBallControlScript playerBallControlScript;

    public CharacterController controller;//
    public GameObject smallBall;
    private Rigidbody rb;
    private int smallBallLayer;
    private Material[] setMulMats;
    private int dropDivisions = 5;
    public event System.Action PathEndReached;
    private bool isRigidbody;
    [HideInInspector] public int totalBallsFallen = 0;
    [HideInInspector] public int totalBallsFallenActual = 0;
    //public ParticleSystem smokeRing;
    public ParticleSystem cloud;
    private float levelEndZ = float.MaxValue;
    [HideInInspector] public float lastZ = float.MaxValue;
    private bool levelEndReached;
    public GameObject trail;
    List<Vector3> localPositions = new List<Vector3>();
    private bool checkLastZ;

    private string obstacleString = "Obstacle";
    private string mulString = "Mul";
    public event System.Action<int> BallsDropped;
    //[HideInInspector] public int minusPoints = 0;
    private int minusAmount = 10;
    private string smallBall1String = "SmallBall1";
    private string smallBall2String = "SmallBall2";// after hitting platform
    private CameraCullScript cameraCullScript;
    public GameObject smokePrefab;
    private Transform ballParent;

    void Awake()
    {
        ballParent = new GameObject("BllParent").transform;
        Refs refs = FindObjectOfType<Refs>();
        gameManager = refs.gameManager;
        //bottomTriggerScript =refs.bottomTriggerScript;
        //cameraFollow =refs.cameraFollow;
        levelGenerator = refs.levelGenerator;
        cameraCullScript = refs.cameraCullScript;
        rb = GetComponent<Rigidbody>();
        smallBallLayer = LayerMask.NameToLayer("SmallBall");
    }
    private void OnEnable()
    {
        levelGenerator.SetBasket += OnBasketSet;
    }
    private void OnDisable()
    {
        levelGenerator.SetBasket -= OnBasketSet;
    }
    private void Start()
    {
        lessFactor = GameDataClass.Instance.lessFactor;
        setMulMats = GameDataClass.Instance.mats;
        localPositions = GameDataClass.Instance.localPositions; 
    }
    private void OnBasketSet()
    {
        levelEndZ = levelGenerator.lastPos.z;
    }

    private void Update()
    {
        //isGrounded = controller.isGrounded;

        if (!gameManager.gameLost)
        {
            if (!levelEndReached)
            {
                if (transform.localPosition.z > levelEndZ)
                {
                    levelEndReached = true;
                    PathEndReached?.Invoke();
                    checkLastZ = true;
                }
            }
            if (checkLastZ)
            {
                if (transform.localPosition.z > lastZ)
                {
                    checkLastZ = false;
                    MakeRb();
                    playerBallControlScript.stopMotion = true;
                    trail.SetActive(false);
                }
            }
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag(obstacleString))
        {
            cloud.transform.localPosition = transform.localPosition;
            cloud.GetComponent<Renderer>().sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;
            cloud.transform.localScale *= 2;
            cloud.Play();
            gameObject.SetActive(false);
            //MakeRb();
            AudioManager.Instance.PlayFailSound();
            gameManager.GameFailed();
        }
    }
    public void MakeRb()//called when hit obstacle, Also called when basket is reached
    {
        if (!isRigidbody)
        {
            controller.enabled = false;
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * 15, ForceMode.Impulse);
            gameObject.AddComponent<SphereCollider>();
            isRigidbody = true;
        }
    }
    int previousCollectedMulIndex = -1;
    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.gameOver)
            return;
        if (other.CompareTag(mulString))
        {
            MulData mulData = other.GetComponentInParent<MulData>();
            //if(mulData.mulVerticalIndex == 9)
            //xa = -2;
            if (previousCollectedMulIndex!= mulData.mulVerticalIndex)
            {
                //if (mulData.mulValue > 0)
                //{
                if (mulData.mulValue > 0)
                {
                    StartCoroutine(DropBalls(mulData.mulValue, transform.localPosition, mulData.mulVerticalIndex, mulData.mulColorIndex));
                    totalBallsFallenActual += mulData.mulValue;
                }
                else
                {
                    int c = (Mathf.Abs(mulData.mulValue)-1) * totalBallsFallenActual;
                    StartCoroutine(DropBalls(c, transform.localPosition, mulData.mulVerticalIndex, mulData.mulColorIndex));
                    totalBallsFallenActual += c;
                    
                }
                other.transform.parent.gameObject.SetActive(false);
                    AudioManager.Instance.PlayGlassBreakSound();
                #if UNITY_EDITOR
                PlaySmoke(transform.localPosition + Vector3.forward * 2f);
                #endif
                    previousCollectedMulIndex = mulData.mulVerticalIndex;
                UIManager.Instance.SetCollectedCountText(/*mulData.mulValue*/totalBallsFallenActual, mulData.mulValue);
            }
            else
            {
                other.transform.parent.gameObject.SetActive(false);//just disabling no ballls spawning, just to make user not feel bad
            }
        }
        else if (other.CompareTag(obstacleString))
        {
            AudioManager.Instance.PlayBoingSound();
            other.transform.parent.gameObject.SetActive(false);
            totalBallsFallenActual -= minusAmount;
            if (totalBallsFallenActual < 0)
            {
                totalBallsFallenActual = 0;
            }

            GameObject[] balls2 = GameObject.FindGameObjectsWithTag(smallBall2String);
            int remaining = DestroyArray(balls2, minusAmount);
            if (remaining != -1)
            {
                GameObject[] balls1 = GameObject.FindGameObjectsWithTag(smallBall1String);
                remaining = DestroyArray(balls1, remaining);
                if (remaining != -1)
                {
                    //minusPoints += remaining;
                    //print("remaining +" + remaining + "added to minus " + minusPoints + " culledObjs.Count " + cameraCullScript.culledObjs.Count);
                    //minusPoints = Mathf.Min(cameraCullScript.culledObjs.Count, minusPoints);
                    //print("minusPoints after adjust " + minusPoints);
                    cameraCullScript.DestroyCulledObects(remaining);
                }
            }
            UIManager.Instance.SetCollectedCountText(/*mulData.mulValue*/totalBallsFallenActual, -minusAmount);
        }
        totalBallsFallen = Mathf.Min(totalCount - lessFactor, totalBallsFallenActual);
    }
    private int DestroyArray(GameObject[] array, int count)
    {
        int remaining = -1;
        if (array.Length >= count)
        {
            for (int i = count - 1; i > -1; i--)
            {
                Destroy(array[i]);
            }
        }
        else
        {
            remaining = count - array.Length;
            for (int i = array.Length - 1; i > -1; i--)
            {
                Destroy(array[i]);//
            }
        }
        return remaining;

    }
    static float minValue = 0.3f;
    static float maxValue = 1f;
    private IEnumerator DropBalls(int count, Vector3 position, int mulVerticalIndex, int mulColorIndex)
    {
        SmallBallData sbd;
        GameObject obj;
        //GameObject[] toChangeLayerArray = new GameObject[count];
        int perFrameCount = count / dropDivisions;
        //Material ballMat = setMulMats[mulColorIndex];
        //bool changeColor = (count != maxValueArray[mulVerticalIndex]);
        for (int i = 0; i < count; i++)//
        {
            //sbd = sbdArray[mulVerticalIndex][i];
            sbd = smallBallDataQueue.Dequeue();
            obj = sbd.obj;
            //obj.transform.localPosition = position + sbd.positionDelta;
            obj.transform.localPosition = position + localPositions[i];

            obj.SetActive(true);
            sbd.rb.AddForce(sbd.force, ForceMode.Impulse);
            //if (changeColor)
            //{
            //    EditorApplication.Beep();
            //    sbd.mr.sharedMaterial = ballMat;
            //    print("changeColor");
            //}
            //toChangeLayerArray[i] = obj;
            if (i == perFrameCount - 1)//
            {
                perFrameCount *= 2;
                position -= Vector3.up * 0.6f;
                yield return null;
            }
        }
        //if (xa < 0)
        //    JU.Pause();
        //sbdArray[mulVerticalIndex] = null;
        BallsDropped?.Invoke(count);
        //StartCoroutine(ChangeLayer(toChangeLayerArray));
    }
    //int xa;
    WaitForSeconds changeLayerWfs = new WaitForSeconds(1.5f);
    private IEnumerator ChangeLayer(GameObject[] list)//
    {
        yield return changeLayerWfs;
        for (int i = 0; i < list.Length; i++)
        {
            if(list[i]!=null)
            list[i].layer = smallBallLayer;//
        }
    }
    private SmallBallData[][] sbdArray;
    private Queue<SmallBallData> smallBallDataQueue;
    int[] maxValueArray;
    int totalCount;
    public void SetPool(int[] maxValueArray, int mulCount, int[] maxValueColorIndexArray)//To be called from SetMul
    {
        SmallBallData sb = new SmallBallData();
        GameObject ballInstantiated;

        //sbdArray = new SmallBallData[mulCount][];
        this.maxValueArray = maxValueArray;
        MeshRenderer mr;
        totalCount = levelGenerator.totalCount;
        //smallBallDataArray = new SmallBallData[totalCount];
        smallBallDataQueue = new Queue<SmallBallData>();
        bool useSingleColor = GameDataClass.Instance.useSingleColor;

        //BallColor bc = GameDataClass.Instance.ballColor;
        //int colorIndex = 0;
        //if (bc == BallColor.Blue)
        //    colorIndex = 0;
        //else if (bc == BallColor.Brown)
        //    colorIndex = 1;
        //else if (bc == BallColor.Green)
        //    colorIndex = 2;
        //else if (bc == BallColor.Violet)
        //    colorIndex = 3;
        //else if (bc == BallColor.Yellow)
        //    colorIndex = 4;

        //int colorIndex = GameDataClass.Instance.colorIndex;
        if (useSingleColor)
            setMulMats[0].color = GameDataClass.Instance.ballColor;
        for (int i = 0; i < totalCount; i++)
        {
            ballInstantiated = Instantiate(smallBall);
            mr = ballInstantiated.GetComponent<MeshRenderer>();
            if(useSingleColor)
            mr.sharedMaterial = setMulMats[0];
            else
            mr.sharedMaterial = setMulMats[Random.Range(0, setMulMats.Length)];
            //sb.obj = ballInstantiated;
            //sb.rb = ballInstantiated.GetComponent<Rigidbody>();
            //sb.mr = mr;
            sb = new SmallBallData(ballInstantiated, ballInstantiated.GetComponent<Rigidbody>(), mr);
            smallBallDataQueue.Enqueue(sb);
            ballInstantiated.transform.SetParent(ballParent);
            //sbdArray[i][j] = sb;
            //#if UNITY_EDITOR
            //            //ballInstantiated.transform.SetParent(poolParent);
            //            ballInstantiated.hideFlags = HideFlags.HideInHierarchy;
            //#endif
        }
//        for (int i = 0; i < maxValueArray.Length; i++)
//        {
//            //print("Length of sbdArray is " + sbdArray.Length+ " maxValueArray is  "+ maxValueArray.Length);
//            //print(" i = "+i);
//            sbdArray[i] = new SmallBallData[Mathf.Abs(maxValueArray[i])];
//            for (int j = 0; j < maxValueArray[i]; j++)
//            {
//                ballInstantiated = Instantiate(smallBall);
//                //ballInstantiated.SetActive(false);
//                mr = ballInstantiated.GetComponent<MeshRenderer>();
//                mr.sharedMaterial = setMulMats[maxValueColorIndexArray[i]];
//                //sb.obj = ballInstantiated;
//                //sb.rb = ballInstantiated.GetComponent<Rigidbody>();
//                //sb.mr = mr;
//                sb = new SmallBallData(ballInstantiated, ballInstantiated.GetComponent<Rigidbody>(), mr);
//                sbdArray[i][j] = sb;
//#if UNITY_EDITOR
//                //ballInstantiated.transform.SetParent(poolParent);
//                ballInstantiated.hideFlags = HideFlags.HideInHierarchy;
//#endif
//            }
        //}
    }

    private struct SmallBallData
    {
        public GameObject obj;
        public Rigidbody rb;
        public MeshRenderer mr;

        public Vector3 positionDelta; 
        public Vector3 force;
        public SmallBallData(GameObject obj, Rigidbody rb, MeshRenderer mr)
        {
            this.obj = obj;
            this.rb = rb;
            this.mr = mr;
            positionDelta = new Vector3(Random.Range(minValue, 5), Random.Range(minValue, maxValue), Random.Range(minValue, maxValue));
            //force = new Vector3(0, -Random.Range(4f, 8f), Random.Range(2f, 5f)) * 5;
            force = new Vector3(0, -5, 3) * 5;
        }
    }
    private void PlaySmoke(Vector3 position)
    {
        cloud.transform.localPosition = position;
        cloud.Play();
    }
   
}
