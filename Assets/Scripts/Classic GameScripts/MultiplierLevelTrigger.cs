using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MultiplierLevelTrigger : MonoBehaviour
{
    /*Vector3 left;
    int mask;
    PlayerBallScript playerBallScript;
    BigPitScript bigPitScript;
    private void Awake()
    {
        playerBallScript = FindObjectOfType<PlayerBallScript>();
    }
    private void Start()
    {
        mask = LayerMask.GetMask("SmallBall", "SmallBallNoSelf");
    }
    private void OnEnable()
    {
        playerBallScript.PathEndReached += OnPathEndReached;
    }
    private void OnDisable()
    {
        playerBallScript.PathEndReached -= OnPathEndReached;
    }
    private Transform t;
    private Transform pt;
    RaycastHit hit;
    private void OnPathEndReached()
    {
        //StartCoroutine(Jaya());

    }
    public IEnumerator CheckLevel()
    {
        left = transform.position + Vector3.back * 10;
        while (true)
        {
            if(Physics.Raycast(left, Vector3.forward,out hit, 15, mask))
            {
                t = hit.transform;
                if(t == pt)
                {
                    //EditorApplication.isPaused = true;
                    //AudioManager.Instance.PlayClingSound();
                    bigPitScript.MoveNextMultiplier();
                    yield break;
                }
                pt = t;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
    public void SetBigPitScript(BigPitScript bps, int multiplierValue)
    {
        this.bigPitScript = bps;
    }*/
}
