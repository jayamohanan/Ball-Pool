/*
Add the following to the recieving script
 
    void Start()
        {    Tweak.Instance.valueChange += OnEditValueChanged;
            SetTweakInitialValues();
        }

    private void SetTweakInitialValues()
    {
        Tweak.Instance.SetIF(1, value1, "Value1Identifier");
        //Tweak.Instance.SetToggle(1, bool1,"Bool1Identifier");
    }

    private void OnEditValueChanged()
    {
        float1 = Tweak.Instance.ifValueList[0];
        //bool1 = Tweak.Instance.tList[0].isOn;
    }

Call SetIF function from other script
when valueChange callback is called, take the value from corresponding ifValueList  
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tweak : MonoBehaviour
{
    public event System.Action valueChange;
    public GameObject tweakPanel;
    public Text frameRateText;
    //InputFields
    [Header("Input Fields")] public InputField[] ifList = new InputField[4];
    [Header("Texts")] public Text[] ifTextList = new Text[4];
    [Header("Identifier Text")] public Text[] ifIdentifierList = new Text[4];
    [HideInInspector] public float[] ifValueList = new float[4];
    //Toggles
    [Header("Toggles")] public Toggle[] tList = new Toggle[4];
    [Header("Identifier Text")] public Text[] tIdentifierList = new Text[4];
    [HideInInspector] public bool[] tValueList = new bool[4];

    private float parsedValue;
    public static Tweak Instance;

    void Awake()
    {
        if (Instance == null)
        {
            tweakPanel.SetActive(false);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        tIdentifierList[3].text = "FrameRate";
    }
    public void ToggleVisibility()
    {
        tweakPanel.SetActive(!tweakPanel.activeSelf);
        if (tweakPanel.activeSelf)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
    public void OnValueChanged()
    {
        for (int i = 0; i < 4; i++)
        {
            if (float.TryParse(ifList[i].text, out parsedValue))
            {
                if (parsedValue != ifValueList[i])
                {
                    ifTextList[i].text = parsedValue.ToString();
                    ifValueList[i] = parsedValue;
                }
            }
        }
        for (int i = 0; i < 4; i++)
        {
            tValueList[i] = tList[i].isOn;
        }
        valueChange?.Invoke();
        if(tValueList[3] == true)
        {
            frameRateText.gameObject.SetActive(true);
        }
        else
        {
            frameRateText.gameObject.SetActive(false);
        }
    }

    //public void SetIF(int index, float value, string identifierName)
    //{
    //    ifList[index - 1].text = value.ToString();
    //    ifValueList[index - 1] = value;
    //    ifTextList[index - 1].text = value.ToString();
    //    ifIdentifierList[index - 1].text = identifierName;
    //}
    //public void SetToggle(int index, bool value, string identifierName)
    //{
    //    tList[index-1].isOn = value;
    //    tIdentifierList[index - 1].text = identifierName;
    //}
    public void SetIF(int index, float value, string identifierName)
    {
        ifList[index - 1].text = value.ToString();
        ifValueList[index - 1] = value;
        ifTextList[index - 1].text = value.ToString();
        ifIdentifierList[index - 1].text = identifierName;
    }
    public void SetToggle(int index, bool value, string identifierName)
    {
        tList[index - 1].isOn = value;
        tIdentifierList[index - 1].text = identifierName;
    }
    int frameRate;
    private void Update()
    {
        frameRate = (int)(1 / Time.deltaTime);
        frameRateText.text = frameRate.ToString();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
