using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarWorldCanvas : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image starImage;
    public Color highlightColor;
    void Start()
    {
        
    }

    public void SetText(int multiplierVaue)
    {
        text.text = multiplierVaue.ToString() + "X";
    }
    public void SetStar()
    {
        //starImage.color = highlightColor;
        text.color = highlightColor;
    }
}
