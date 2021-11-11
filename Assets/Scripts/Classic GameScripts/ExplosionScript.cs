using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public GameObject piecesParent;
    public Rigidbody[] pieces;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Explode(Vector3 position)
    {
        piecesParent.transform.SetParent(null);
        piecesParent.SetActive(true);
        for (int i = 0; i < pieces.Length; i++)
        {
            //pieces[i].AddExplosionForce(5,position, 5,1, ForceMode.Impulse);
            Vector3 direction = new Vector3(0, Random.Range(-1,0.5f), Random.Range(0.5f,1));
            pieces[i].AddForce(direction*25, ForceMode.Impulse);
        }
        //print("explodd");
    }
}
