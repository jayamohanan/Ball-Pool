using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;//
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Start Screen")]
    public GameObject startScreen;

    [Header("In Game Screen")]
    public GameObject inGameScreen;
    public TextMeshProUGUI levelNoText;
    public TextMeshProUGUI collectedCountText;
    public TextMeshProUGUI changeCountText;
    public TextMeshProUGUI outOfText;
    public GameObject slider;
    public Image greenImage;
    public Image bgImage;
    public RectTransform greenArrowRect;
    public Text arrowText;
    private float greenImgHalfRange;
    public Text lostGainCountText;
    private RectTransform lostGainRect;

    [Header("Win Screen")]
    public GameObject winScreen;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI multiplierText;
    public GameObject rainConfetti;
    private bool fullPercent = false;
    public GameObject[] NAConfettiArray;
    public GameObject scoreGainedPanel;
    public TextMeshProUGUI scoreGainedText;
    public GameObject scorePanel;
    public TextMeshProUGUI scoreText;
    public GameObject nextButtonObj;
    public GameObject coinAnimPanel;

    [Header("Lose Screen")]
    public GameObject loseScreen;

    [Header("Star")]
    public GameObject starCanvas;
    public Image[] stars;
    public Color fillColor;
    public Color goldColor;

    private Camera mainCam;
    public Canvas cmamSpaceCanvas;
    private GameManager gameManager;

    private int collectedCountTextTweenId; 
    void Awake()
    {
        if (Instance == null)
        {
            mainCam = Camera.main;
            Instance = this;
            DontDestroyOnLoad(gameObject);
            greenImgHalfRange = greenImage.rectTransform.sizeDelta.x / 2;
            gameManager = FindObjectOfType<GameManager>();
            //lostGainRect = lostGainCountText.GetComponent<RectTransform>();
        }
        else
            Destroy(gameObject);
    }
   
    
    private void OnEnable()
    {
        if(Instance == this)
        {
            gameManager = FindObjectOfType<GameManager>();
            gameManager.LevelSetEvent += OnLevelSet;
            gameManager.LevelOverEvent += OnLevelOver;
        }
    }
    private void OnDisable()//
    {
        if(Instance == this)
        {
            gameManager.LevelSetEvent -= OnLevelSet;
            gameManager.LevelOverEvent -= OnLevelOver;
        }
    }
    
    private void OnLevelSet()
    {
        SetLevelStartUI(gameManager.currentLevel);
    }
    private void OnLevelOver(bool win, int multiplier)
    {
        //print("OnLevelover");
        if(win)
            SetWinScreen(true, multiplier);
        else
            SetLoseScreen(true);
    }
    private void OnLevelWasLoaded(int level)
    {
        //print("f: OnLevelWasLoaded");

        if (Instance == this)
        {
            gameManager = FindObjectOfType<GameManager>();
            gameManager.LevelSetEvent += OnLevelSet;
            gameManager.LevelOverEvent += OnLevelOver;

            mainCam = Camera.main;
            cmamSpaceCanvas.worldCamera = mainCam;
            starCanvas.SetActive(false);
        }
    }

    public void SetStartScreen(bool active)
    {
        scorePanel.gameObject.SetActive(false);

        StopAllCoroutines();//To stop game win confetti if playing
        if(confParent!=null)
        Destroy(confParent.gameObject);
        startScreen.SetActive(active);
        fullPercent = false;
        //SetSlider(0);
    }
    public void SetInGameScreen(bool active, int levelNumber)
    {
        if (active)
        {
            levelNoText.gameObject.SetActive(true);
            levelNoText.text = "LEVEL " + levelNumber.ToString();

        }
        inGameScreen.SetActive(active);
    }
    public void SetWinScreen(bool active, int multiplier = 1, string status = "Completed!")
    {
        if (active)
        {
            statusText.text = status;
            //if(multiplier!=-1)
            //multiplierText.text = multiplier.ToString() + "%";
            StartCoroutine(PlayConfetti(multiplier));
            scoreGainedPanel.SetActive(true);
            Invoke("ShowNextButton",2);
            if(multiplier == 1)
            {
                coinCount = 3;
                scoreGainedText.text = "+3";
            }
            else if(multiplier == 2)
            {
                coinCount = 5;
                scoreGainedText.text = "+5";
            }
            else if(multiplier == 3)
            {
                coinCount = 10;
                scoreGainedText.text = "+10";
            }
            else if(multiplier == 4)
            {
                coinCount = 25;
                scoreGainedText.text = "+25";
                Invoke("ShowCoinAnim", 0.5f);//
            }
        }
        winScreen.SetActive(active);
    }
    int coinCount;
    private void ShowCoinAnim()
    {
        //int childCount = coinAnimPanel.transform.childCount;
        //for (int i = 0; i < childCount; i++)
        //{
        //    if(i<coinCount)
        //    coinAnimPanel.transform.GetChild(i).gameObject.SetActive(true);
        //    else
        //    coinAnimPanel.transform.GetChild(i).gameObject.SetActive(false);
        //}
        coinAnimPanel.SetActive(true);
        ScoreManager.Instance.AddScore(coinCount);

    }
    private void ShowNextButton()
    {
        nextButtonObj.SetActive(true);
    }
    public void SetLoseScreen(bool active)
    {
        if (active)
        {
            Invoke("LoseScreenTimer", 1);
            //StartCoroutine("LoseScreenTimer");
        }
        else
            loseScreen.SetActive(false);

    }
    private /*IEnumerator*/void LoseScreenTimer() 
    { 
        //yield return new WaitForSeconds(1);
        loseScreen.SetActive(true);
    }

    public void SetLevelStartUI(int levelNumber)
    {
        SetStartScreen(true);
        SetInGameScreen(true, levelNumber);
        SetWinScreen(false);
        SetLoseScreen(false);
        //collectedCountText.gameObject.SetActive(false);
        collectedCountText.text = "0";
        outOfText.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
        currentSliderValue = 0;
    }
    public void Retry()
    {
        SetLoseScreen(false);
        FindObjectOfType<GameManager>().Retry();
    }
    public void NextLevel()
    {
        if (confParent != null)
            Destroy(confParent.gameObject);
        SetWinScreen(false);
        FindObjectOfType<GameManager>().NextLevel();//Should come first
    }
    private Transform confParent;
    private GameObject[] confettiGenerated = new GameObject[3];
    private IEnumerator PlayConfetti(int count)
    {
        yield return new WaitForSeconds(0.1f);
        confParent = new GameObject("ConfParent").transform;
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(.5f, .65f, 8));
        AudioManager.Instance.PlayWowSound();
        GameObject obj;

        WaitForSeconds wfs = new WaitForSeconds(0.3f);
        for (int i = 0; i < count; i++)
        {
            obj = Instantiate(NAConfettiArray[0], pos, Quaternion.Euler(Vector3.zero), confParent);
            yield return wfs;
            Destroy(obj, 8);
        }
    }
    public void SetCountText(int outOfCount)
    {
        //collectedCountText.gameObject.SetActive(true);
        outOfText.gameObject.SetActive(true);
        //slider.gameObject.SetActive(true);
        outOfText.text = " / "+outOfCount;
    }
    //List<Vector3> confettiPositions = new List<Vector3>();
    //SLIDER
    float currentSliderValue = -1;
    //public void SetSlider(float percent)
    //{
    //    if (fullPercent)
    //        return;
    //    if (percent > currentSliderValue)
    //    {
    //        if (percent > 1)
    //            percent = 1;
    //        currentSliderValue = percent;
    //        greenImage.fillAmount = percent;
    //        greenArrowRect.anchoredPosition = new Vector2(Mathf.Lerp(-greenImgHalfRange, greenImgHalfRange, percent), greenArrowRect.anchoredPosition.y);
    //        arrowText.text = ((int)(percent * 100)).ToString();
    //    }
    //    if (percent == 1)//
    //    {
    //        fullPercent = true;
    //        Vector3 fullPercentPos = Camera.main.ScreenToWorldPoint(new Vector3(greenArrowRect.transform.position.x, greenArrowRect.transform.position.y, 25));
    //        Instantiate(NAConfettiArray[0], fullPercentPos, Quaternion.Euler(new Vector3(0,0,0)), confParent);
    //        //AudioManager.Instance.PlayClingSound();
    //    }

    //}
    //STAR
    //public void InitialiseStarCanvas(int totalValue, int collectedCount)
    //{
    //    print("InitialiseStarCanvas");
    //    bottomValueIndex = -1;
    //    allStarCollected = false;

    //    starCanvas.SetActive(true);
    //    SetStarLimits(totalValue);
    //    MoveNextStar();//from -1 to 0
    //    ModifyStar(collectedCount);

    //    Image img;
    //    for (int i = 0; i < stars.Length; i++)
    //    {
    //        img = stars[i];
    //        img.color = fillColor;
    //        img.fillAmount = 0;
    //    }
    //    SetCountText(totalValue);
    //}
    public void InitialiseStarCanvas1()
    {
        //bottomValueIndex = -1;
        //allStarCollected = false;
        //value = 0;
        starCanvas.SetActive(true);
        levelNoText.gameObject.SetActive(false);
        //SetStarLimits(totalValue);
        //MoveNextStar();//from -1 to 0
        //ModifyStar(collectedCount);

        Image img;
        for (int i = 0; i < stars.Length; i++)
        {
            img = stars[i];
            img.color = fillColor;
            img.fillAmount = 0;
        }
        //SetCountText(totalValue);
    }
    int[] limits = new int[4];
    private int bottomValueIndex = -1;
    public void SetStarLimits(int maxValue)
    {
        int unitCount = maxValue / 3;
        limits[0] = 0;
        limits[1] = unitCount;
        limits[2] = unitCount*2;
        limits[3] = maxValue;
    }
    int bottomValue = 0;
    int topValue = 0;
    int diffValue = 0;
    bool allStarCollected;
    public void ModifyStar(int currentFillCount)
    {
        if (allStarCollected) 
            return;
        stars[bottomValueIndex].fillAmount = ((currentFillCount- bottomValue )/ (float)diffValue);
        if (currentFillCount >= topValue)
        {
            stars[bottomValueIndex].color = goldColor;
            GameObject obj = Instantiate(NAConfettiArray[0], stars[bottomValueIndex].transform.position+Vector3.right*0.5f, Quaternion.identity);
            obj.transform.localScale *= 0.5f;
            Destroy(obj, 3);
            if (bottomValueIndex < 2)
            {
                MoveNextStar();
                ModifyStar(currentFillCount);
            }
            else
            {
                allStarCollected = true;
            }
        }
    }
    public void PlayStarConfetti(int index)
    {
        stars[index].color = goldColor;
        GameObject obj = Instantiate(NAConfettiArray[0], stars[index].transform.position + Vector3.right * 0.5f, Quaternion.identity);
        obj.transform.localScale *= 0.5f;
        Destroy(obj, 3);
    }
    private void MoveNextStar()
    {
        if(bottomValueIndex < 2)//
        {
            bottomValueIndex++;//012
            bottomValue = limits[bottomValueIndex];
            topValue = limits[bottomValueIndex + 1];
            diffValue = topValue - bottomValue;
            stars[bottomValueIndex].gameObject.SetActive(true);
        }
    }
    //private int value = 0;
    public void SetScoreText(int s)
    {
        scorePanel.gameObject.SetActive(true);
        scoreText.text = s.ToString();
    }
    private WaitForSeconds collectedCountWaitTime = new WaitForSeconds(1);
    Coroutine ccCoroutine;
    Coroutine changeCountCoroutine;
    public void SetCollectedCountText(int collectedCount, int changedValue)
    {
        collectedCountText.text = collectedCount.ToString();
        if(changedValue>0)
        changeCountText.text = "+"+changedValue.ToString();
        else
            changeCountText.text =changedValue.ToString();

        //if (changeCountCoroutine != null)
        //    StopCoroutine(changeCountCoroutine);
        //changeCountCoroutine = StartCoroutine(ChangeCountCoroutine());
    }
    private IEnumerator ChangeCountCoroutine()
    {
        changeCountText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        changeCountText.gameObject.SetActive(false);
    }

    private IEnumerator ShowCollectedCount(int collectedCount)
    {
        collectedCountText.text = collectedCount.ToString();
        collectedCountText.gameObject.SetActive(true);
        yield return collectedCountWaitTime;
        collectedCountText.gameObject.SetActive(false);
    }
    public void SetLostGainCountText(Vector3 worldPos)
    {
        Vector3 screenPos = mainCam.WorldToScreenPoint(worldPos);
        lostGainRect.anchoredPosition = screenPos;

    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
