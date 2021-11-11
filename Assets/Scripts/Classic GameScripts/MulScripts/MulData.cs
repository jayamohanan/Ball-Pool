using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MulData : MonoBehaviour
{
    private Color white = Color.white;
    private Color black = Color.black;
    public Transform cube;
    public MeshRenderer mr;//
    //public Transform canvasTransform;
    //public BreakableWindow window;

    public TextMeshPro text;
    [HideInInspector] public int mulValue;
    [HideInInspector] public int mulVerticalIndex;
    [HideInInspector] public int mulColorIndex;
    public void SetMulValue(int mulVerticalIndex, int value, Material mat, int mulColorIndex, int textColor = 0)
    {
        this.mulValue = value;
        this.mulVerticalIndex = mulVerticalIndex;
        if (value>0)
        text.text = value.ToString();
        else
        text.text = "X"+Mathf.Abs(value).ToString();
        this.mulColorIndex = mulColorIndex;
        //if no color only transparent comment below
        mr.sharedMaterial = mat;
        if(textColor == 0)
        {
            text.color = Color.white;//
        }
        else
        {
            text.color = Color.black;
        }
    }
    //public void DestroyMulCanvas()
    //{
    //    Destroy(canvasTransform.gameObject);
    //}
}
