using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool zerofound;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            zerofound = false;
            for (int i = 0; i < 100; i++)
            {
                int[] a = GenerateRandomNumbers(3, 15);
                for (int j = 0; j < 3; j++)
                {
                    if(a[j]<2)
                    {
                        zerofound = true;

                        print("Zero found at "+i);
                        JU.PrintArray(a);
                        break;
                    }
                }
                if (zerofound)
                    break;
            }
            if (!zerofound)
                print("successful");
            //JU.PrintArray(a);
        }   
    }
    private int[] GenerateRandomNumbers(int divisions, int totalValue)
    {
        totalValue -= 2 * divisions;
        List<float> rnds = new List<float>();
        int weightedSum = 0;
        int addedSoFar = 0;
        int minRange = Mathf.Max((int)(totalValue / divisions * 0.5f), 1);
        int maxRange = (int)(totalValue / divisions * 2.5f);
        for (int i = 0; i < divisions; i++)
        {
            int a = Random.Range(minRange, maxRange);
            rnds.Add(a);
            weightedSum += a;
        }
        for (int i = 0; i < divisions; i++)
        {
            rnds[i] = rnds[i] / weightedSum;
        }
        int[] finalValues = new int[divisions];
        for (int i = 0; i < divisions; i++)
        {
            if (i == divisions - 1)
            {
                finalValues[i] = totalValue - addedSoFar;
                addedSoFar += finalValues[i];
            }
            else
            {
                finalValues[i] = Mathf.CeilToInt(rnds[i] * totalValue);
                addedSoFar += finalValues[i];
            }
        }
        //JU.PrintArray(finalValues);
        for (int i = 0; i < finalValues.Length; i++)
        {
            finalValues[i] += 2;
        }
        bool repeat = false;
        for (int i = 0; i < finalValues.Length; i++)
        {
            if (finalValues[i] < 2)
            {
                repeat = true;
                break;
            }
        }
        if (repeat)
            finalValues = GenerateRandomNumbers(divisions, totalValue);
        return finalValues;
    }
}
