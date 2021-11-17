using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulMove : MonoBehaviour
{
    private int direction;
    public float speed;
    [HideInInspector] private bool move = true;
    [HideInInspector] public bool freeze = false;
    [HideInInspector] public Vector2 limits;
    //public GameObject bottomObject;
    //public GameObject topObject;
    public GameObject line;
    private GameManager gameManager;
    void Awake()
    {
        direction = (Random.Range(0, 2) == 0) ? -1 : 1;
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        //123 statiionary
        if (gameManager.currentLevel < 5)
            speed = 0;
        else if (gameManager.currentLevel < 10)
            speed = 6;
        else if (gameManager.currentLevel < 15)
            speed = 8;
        else if (gameManager.currentLevel < 20)
            speed = 10;
    }

    void Update()
    {
        if(!freeze)
        if (move)
        {
            transform.position += Vector3.up * direction * speed * Time.deltaTime;
            if (transform.position.y > limits.y)
            {
                transform.position = new Vector3(transform.position.x, limits.y, transform.position.z);
                direction *= -1;
                //move = false;
                //Invoke("StartAgain", 0.5f);
                //JU.Pause();
                //GameDataClass.Instance.debug = transform;
                //JU.Beep();
            }
            else if(transform.position.y < limits.x)
            {
                transform.position = new Vector3(transform.position.x, limits.x,transform.position.z);
                direction *= -1;
                //move = false;
                //Invoke("StartAgain", 0.5f);
                //JU.Pause();
            }
        }
    }
    private void StartAgain()
    {
        move = true;
    }
    public void SetLimits(float bottom, float top)
    {
        limits.x = bottom;
        float height = GetComponentInChildren<Collider>().transform.localScale.y;
        limits.y = top-height;
        //print("bottom "+ bottom);
        //print("top -2 " + top);
        //BottomLimit
        Vector3 pos = line.transform.position;
        pos.y = bottom - 0.1f;
        //bottomObject.transform.position = pos;
        line.transform.position = pos;
        Vector3 scale =  line.transform.localScale;
        scale.y = (top - bottom);
        line.transform.localScale = scale;


        //TopLimit
        //pos.y = top + 2.1f;
        //topObject.transform.position = pos;

        //bottomObject.transform.SetParent(null);
        //topObject.transform.SetParent(null);
        line.transform.SetParent(null);
        line.hideFlags = HideFlags.HideInHierarchy;
        //StaticBatchingUtility.Combine(bottomObject);
        //StaticBatchingUtility.Combine(topObject);
        StaticBatchingUtility.Combine(line);
    }
    public void SetStationary()
    {
        freeze = true;
        Destroy(line);
    }

}
