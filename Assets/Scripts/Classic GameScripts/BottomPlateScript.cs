using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomPlateScript : MonoBehaviour
{
    private string smallBall3String = "SmallBall3";
    int collectedCount = 0;
    int soundInterval = 30;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(smallBall3String))
        {
            collectedCount++;
            if(collectedCount% soundInterval == 0)
            {
                AudioManager.Instance.PlayFallSound();
            }
        }
    }
}
