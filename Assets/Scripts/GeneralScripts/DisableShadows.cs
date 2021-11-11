using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableShadows : MonoBehaviour
{
    List<MeshRenderer> mrs = new List<MeshRenderer>();
    // Start is called before the first frame update
    void Start()
    {
        mrs.AddRange(FindObjectsOfType<MeshRenderer>());
        foreach (MeshRenderer mr in mrs)
        {
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mr.receiveShadows = false;
        }
    }
}
