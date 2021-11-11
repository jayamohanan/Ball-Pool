using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BottomTriggerScript : MonoBehaviour
{
    public int collectedCount;
    private GameManager gameManager;
    private LevelGenerator levelGenerator;
    private bool firstBall;
    public event System.Action firstBallEvent;
    private HeadScript headScript;
    void Awake()
    {
        Refs refs = FindObjectOfType<Refs>();
        gameManager = refs.gameManager;
        headScript = refs.headScript;//
        levelGenerator = refs.levelGenerator;//

    }
    int count1 = 0;
    private string smallBall1String = "SmallBall1";
    private string smallBall2String = "SmallBall2";
    private string smallBall3String = "SmallBall3";
    private void OnTriggerEnter(Collider other)
    {
        if (!gameManager.gameLost)
        {
            if (other.CompareTag(smallBall2String) || other.CompareTag(smallBall1String))
            {
                other.tag = smallBall3String;
                collectedCount++;
                if (!firstBall)
                {
                    firstBall = true;
                    firstBallEvent?.Invoke();
                }
                if(collectedCount< levelGenerator.totalCount - 50)
                {
                    if (count1 % soundPlayCycle == 0)
                    {
                        AudioManager.Instance.PlayFallSound(0.1f);
                    }
                }
                else
                {
                    AudioManager.Instance.PlayFallSound(0.1f);
                }
                count1++;
            }
            else if(!firstBall && other.gameObject.layer == 11)
            {
                firstBall = true;
                firstBallEvent?.Invoke();
            }
        }
    }
    private int totalSoundCount = 100;
    private int soundPlayCycle = 5;
    public void ResetSoundOpt()
    {
        int count = headScript.totalBallsFallen - collectedCount;
        soundPlayCycle = count / totalSoundCount;
        if (soundPlayCycle == 0)
            soundPlayCycle = 1;
    }
}
