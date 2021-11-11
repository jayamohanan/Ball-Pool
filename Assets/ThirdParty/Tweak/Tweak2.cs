

/*
Add the following to the recieving script

1)  
    In Start() method
    "Tweak2.Instance.valueChange += OnEditValueChanged2" at Start function
    SetTweak2InitialValues();

2)   
    bool set2;
    private void SetTweak2InitialValues()
    {
        Tweak2.Instance.SetIF(1, value1, "Value1Identifier");
        Tweak2.Instance.SetIF(2, value2,"Value1Identifier");
        Tweak2.Instance.SetIF(3, value3,"Value1Identifier");
        Tweak2.Instance.SetIF(4, value4,"Value1Identifier");
        Tweak2.Instance.SetToggle(1, bool1,"Bool1Identifier");
        Tweak2.Instance.SetToggle(2, bool1,"Bool1Identifier");
        Tweak2.Instance.SetToggle(3, bool1,"Bool1Identifier");
        Tweak2.Instance.SetToggle(4, bool1,"Bool1Identifier");
        set2 = true;
    }
3)    private void OnEditValueChanged2()
    {
        if(!set2)
        return;
        float1 = Tweak2.Instance.ifValueList[0];
        float2 = Tweak2.Instance.ifValueList[1];
        float3 = Tweak2.Instance.ifValueList[2];
        float4 = Tweak2.Instance.ifValueList[3];

        bool1 = Tweak2.Instance.tList[0].isOn;
        bool2 = Tweak2.Instance.tList[1].isOn;
        bool3 = Tweak2.Instance.tList[2].isOn;
        bool4 = Tweak2.Instance.tList[3].isOn;
    }

Call SetIF function from other script
when valueChange callback is called, take the value from corresponding ifValueList  
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tweak2 : MonoBehaviour
{
    public event System.Action valueChange;
    public GameObject tweakPanel;
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
    public static Tweak2 Instance;

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
    }
    public void ToggleVisibility()
    {
        Tweak tweak = FindObjectOfType<Tweak>();
        if (tweak != null)
        {
            GameObject tPanel = tweak.transform.Find("Tweek Panel").gameObject;
            if(tPanel.activeSelf)
                tPanel.SetActive(false);
        }
        tweakPanel.SetActive(!tweakPanel.activeSelf);
    }
    public void OnValueChanged()
    {
        for (int i = 0; i < 4; i++)
        {
            if (float.TryParse(ifList[i].text, out parsedValue))
            {
                if (parsedValue != i)
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
        valueChange();
    }
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
}
