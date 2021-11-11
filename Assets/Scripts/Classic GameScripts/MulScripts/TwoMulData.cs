using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TwoMulData : MonoBehaviour
{
    public MulData bottomData;
    public MulData topData;
    [Header("Tough")]
    public GameObject bottomMul;
    public GameObject bottomMulCube;
    public GameObject topMul;
    public GameObject topMulCube;

    public GameObject bottomObstacle;
    public GameObject topObstacle;
    public float mulHeight = 3;

    public GameObject middleObstacle;

    [Header("Top Bottom Repos")]
    public GameObject topMulGroup;
    public GameObject bottomMulGroup;

    public void SetTough()
    {
        Destroy(bottomObstacle);
        Destroy(topObstacle);

        Vector3 scale = topMulCube.transform.localScale;
        scale.y = mulHeight;
        topMulCube.transform.localScale = scale;

        scale = bottomMulCube.transform.localScale;
        scale.y = mulHeight;
        bottomMulCube.transform.localScale = scale;

        Vector3 position = topMul.transform.localPosition;
        //position.y = Random.Range(0, 4f);
        position.y =0;
        topMul.transform.localPosition = position;

        position = bottomMul.transform.localPosition;
        //position.y = Random.Range(10f, 15);
        position.y = 13;
        bottomMul.transform.localPosition = position;
                                                   
        //Vector3 sc = middleObstacle.transform.localScale;
        //sc.y = 1;
        //middleObstacle.transform.localScale = sc;
        Destroy(middleObstacle);
    }

    public void SetMulValues(int mulIndex,int bottomValue, Material bottomMaterial/* = null*/, int bottomMaterialIndex, int topValue, Material topMaterial/* = null*/, int topMaterialIndex, int textColor = 0)//text color 0= white, 1 = black
    {
        topData.SetMulValue(mulIndex, topValue, topMaterial, topMaterialIndex, textColor);
        bottomData.SetMulValue(mulIndex, bottomValue, bottomMaterial, bottomMaterialIndex,textColor);
    }
    public void ReposHorizontalMuls()
    {
        Vector3 position;
        //if(bottomMulGroup == null)
        //{
        //    print("name "+transform.name);
        //    if (bottomMulGroup.transform.parent != null)
        //    {
        //        print("parent name " + bottomMulGroup.transform.parent.name);
        //    }
        //}
        position = bottomMulGroup.transform.position;
        position.y += Random.Range(0, 5);
        bottomMulGroup.transform.position = position;
        
        position = topMulGroup.transform.position;
        position.y += Random.Range(0, 5);
        topMulGroup.transform.position = position;


    }
}
