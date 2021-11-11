using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlatformScript : MonoBehaviour
{
    GameManager gameManager;
    HeadScript headScript;

    public int platformFellCount = 0;
    bool optimizeSound;
    int soundPlayCycle;
    public int totalSoundCount = 10;//
    void Awake()
    {
        Refs refs = FindObjectOfType<Refs>();
        gameManager =refs.gameManager;
        headScript = refs.headScript;
    }
    private void OnEnable()
    {
        headScript.BallsDropped += ResetSoundOpt;
    }
    private void OnDisable()
    {
        headScript.BallsDropped -= ResetSoundOpt;
    }
    int count1 = 0;
    private string smallBall1String = "SmallBall1";
    private string smallBall2String = "SmallBall2";
    private void OnCollisionEnter(Collision collision)
    {
        if(!gameManager.gameLost)
        if (collision.collider.CompareTag(smallBall1String))
        {
            collision.collider.tag = smallBall2String;
            platformFellCount++;
                if (optimizeSound)
                {
                    if( count1 % soundPlayCycle == 0)
                    {
                        AudioManager.Instance.PlayFallSound(0.1f);
                    }
                    count1++;
                }
        }
    }
    public void ResetSoundOpt(int count)
    {
        if (count > totalSoundCount)
        {
            optimizeSound = true;
        }
        else
            optimizeSound = false;
        soundPlayCycle = count / totalSoundCount;
    }
}
