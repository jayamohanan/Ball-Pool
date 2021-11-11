using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbBallScript : MonoBehaviour
{
    [HideInInspector] public bool absorb;
    GameObject obj;
    //private void OnTriggerStay(Collider other)
    //{
    //    if (absorb)
    //    {
    //        obj = other.gameObject;
    //        if (obj.layer == 12)//small ball with self collision
    //        {
    //            Destroy(obj);
    //        }
    //    }
    //}

    private void OnCollisionStay(Collision collision)
    {
        if (absorb)
        {
            obj = collision.collider.gameObject;
            if (obj.layer == 12)//small ball with self collision
            {
                Destroy(obj);
            }
        }
    }
}
