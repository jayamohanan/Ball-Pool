//Completely independant, uses player position to calculate confetti positions that can be adjusted
//Script execution order after level setter
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;
using Facebook.Unity;

public class GameManager : MonoBehaviour//
{

    public bool deleteAll;
    public bool setLevel;
    public int level;
    [HideInInspector]public int score;
    //private Transform collectables;
    //private int totalCollectables;
    //[HideInInspector] public Transform currentLevelTransform;
    [HideInInspector] public bool gameOver;
    [HideInInspector] public bool gameWon;
    [HideInInspector] public bool gameLost;
    [HideInInspector] public bool started;
    public event System.Action LevelStartedEvent;
    public event System.Action LevelSetEvent;
    public event System.Action<bool, int> LevelOverEvent;

    [HideInInspector] public int currentLevel;

    //[HideInInspector] public bool editor;
    //[HideInInspector] public bool mobile;
    //private float halfX;
    //private float halfY;

    //private string systemID;
    public bool quickNavigation;
    public bool quickNavigationMob;
    public bool enableLogs;
    Touch t;
    private void Awake()
    {
        GameAnalytics.Initialize();

#if UNITY_EDITOR
        if (deleteAll)
            PlayerPrefs.DeleteAll();
        //if (setLevel)
        //    currentLevel = level;
#endif
#if !UNITY_EDITOR
        if (enableLogs)
            Debug.unityLogger.logEnabled = true;
        else
            Debug.unityLogger.logEnabled = false;
#endif
        //halfX = Screen.width / 2f;
        //halfY = Screen.height / 2f;
        //List<int> list = new List<int>() { 1,2,3,4,5,6,7,8,9};
        //if (SystemInfo.deviceUniqueIdentifier == "2b0b36e2ff8adf7b0f4f4dcb128caef5")
        //{
        //    jayaMobile = true;
        //}
//#if UNITY_EDITOR
//        editor = true;
//#elif UNITY_ANDROID || UNTIY_IOS
//if(!editor)
//   mobile = true;
//#endif
    }
    

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        if (currentLevel == 0)
        {
            currentLevel = 1;
        }
#if UNITY_EDITOR
        if (setLevel)
            currentLevel = level;
#endif
        //currentLevel = 35;
        StartCoroutine(OnLevelStarted());
    }
//    private void Update()
//    {
//        if (quickNavigation)
//        {
//            if (true /*|| jayaMobile*/)
//            {
//#if (UNITY_EDITOR)
//                if (Input.GetKeyDown(KeyCode.A))
//                {
//                    JumpPreviousLevel();
//                }
//                else if (Input.GetKeyDown(KeyCode.S))
//                {
//                    JumpNextLevel();
//                }
//#elif (UNITY_ANDROID || UNITY_IOS)
//            if(quickNavigationMob)
//            if (Input.touchCount > 0)
//            {
//                    t = Input.GetTouch(0);
//                    if(t.phase == TouchPhase.Began)
//                    {
//                        if (t.position.y > halfY)
//                        {
//                            if (Input.GetTouch(0).position.x < halfX)
//                            {
//                                JumpPreviousLevel();
//                            }
//                            else
//                            {
//                                JumpNextLevel();
//                            }
//                        }
//                    }
//                }
//#endif
//            }
//        }
//    }
    private IEnumerator OnLevelStarted()
    {
        //StopAllCoroutines();
        //if (confParent != null)
        //    Destroy(confParent.gameObject);
        //scoreText.gameObject.SetActive(true);
        yield return null;//this is called in start, level setter uses it's start to subscribe to events, so wait one frame b4 calling LevelSetEvent
        gameOver = false;
        gameWon = false;
        gameLost = false;
        //Due to script execution order this is below LevelSetter, so level setter will subscribe to event before next line invoke
        LevelSetEvent?.Invoke();
        yield return null;
        LevelStartedEvent?.Invoke();
        //if(UIManager.Instance!=null)
        //UIManager.Instance.SetLevelStartUI(currentLevel);
        Invoke("StartGameDelay",0.5f);
#if !UNITY_EDITOR
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevel.ToString());
#endif
    }
    private void StartGameDelay()
    {
        started = true;
    }
    public void GameWon(int multiplier = 1)
    {
        //print("Won");
        LevelOverEvent?.Invoke(true, multiplier);
        started = false;
        gameOver = true;
        gameWon = true;

        //UIManager.Instance.SetWinScreen(true, multiplier);
#if !UNITY_EDITOR
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevel.ToString(), multiplier);
#endif
        currentLevel++;
    }
    public void GameFailed(string remark = "")
    {
        LevelOverEvent?.Invoke(false, -1);
        started = false;
        gameOver = true;
        gameLost = true;
        //UIManager.Instance.SetLoseScreen(true);
#if !UNITY_EDITOR
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, currentLevel.ToString());
#endif
    }
    public void Restart()
    {
        print("Res");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Retry()
    {
        PlayerPrefs.SetInt("CurrentLevel", (currentLevel));
        ReloadScene();
    }
    public void NextLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", (currentLevel));
        ReloadScene();
    }
    private void JumpPreviousLevel()
    {
        if (currentLevel > 1)
            currentLevel--;
        ReloadScene();
    }
    private void JumpNextLevel()
    {
        currentLevel++;
        ReloadScene();

    }
    private void OnApplicationPause(bool pause)
    {
#if (!UNITY_EDITOR)
        if(pause)
        {
            //if gamewon currentlevel will be incremented before this
            PlayerPrefs.SetInt("CurrentLevel", (currentLevel));
        }
        else
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
#endif
        // Check the pauseStatus to see if we are in the foreground
        // or background
        if (!pause)
        {
            //app resume
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                //Handle FB.Init
                FB.Init(() =>
                {
                    FB.ActivateApp();
                });
            }
        }
    }
    private void OnApplicationQuit()
    {
        print("OnApplicationQuit ");
#if (UNITY_EDITOR)
        //if gamewon currentlevel will be incremented before this
        PlayerPrefs.SetInt("CurrentLevel", (currentLevel));
#endif
    }
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}