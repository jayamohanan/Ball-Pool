using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameDataClass : MonoBehaviour
{
    public static GameDataClass Instance;
    [Tooltip("Material for collecting balls")]
    public Material[] mats;
    [Tooltip("Check for Single Colored balls, also select color index below")]
    public bool useSingleColor;
    //public BallColor ballColor;
    public Color ballColor;
    //[Range(0,4)]
    //[Tooltip("0:Blue, 1:Brown, 2:Green,3:Violet, 4:Yellow")]
    //public int colorIndex;
    public int lessFactor = 5;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            CalculateLocalPositions();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [HideInInspector] public List<Vector3> localPositions = new List<Vector3>();

    private void CalculateLocalPositions()
    {
        Vector3[] positionArrayTemp;

        float xStartValue = 5.8f;
        int totalCount1 = 1000;

        positionArrayTemp = new Vector3[totalCount1];
        Vector3 pos = new Vector3(xStartValue, 0, 0);
        //int xDirection = -1;
        float gap = 1.05f;
        int mulCount = 10;
        int countBackwards = totalCount1 / (mulCount * 10);
        int baseCount = countBackwards * 10;
        for (int k = 0; k < mulCount; k++)
        {
            for (int i = 0; i < countBackwards; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    positionArrayTemp[j + i * 10 + k * baseCount] = (pos + Vector3.left * j * gap);
                }
                pos.z -= gap;
                pos.x = xStartValue;
            }
            pos.x = xStartValue;
            pos.z = 0;
            pos.y += gap;
        }
        Vector3 centre = (positionArrayTemp[positionArrayTemp.Length - 1] + positionArrayTemp[0]) / 2f;
        localPositions = positionArrayTemp.OrderBy(x => (Vector3.Distance(centre, x))).ToList();
        for (int i = 0; i < localPositions.Count; i++)
        {
            localPositions[i] -= centre;
        }
        //for (int i = 0; i < 50; i++)
        //{
        //    JU.DebugSphere(localPositions[i]+Vector3.up*50, Color.red, 1);
        //}
    }
}
public enum BallColor 
{
    Blue,
    Brown,
    Green,
    Violet,
    Yellow
}

