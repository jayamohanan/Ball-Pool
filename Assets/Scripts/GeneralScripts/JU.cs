using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class JU
{
    public static List<Vector3> SortList(List<Vector3> list)
    {
        List<Vector3> newList = new List<Vector3>();
        List<Vector3> outputList = new List<Vector3>();
        for (int i = 0; i < list.Count; i++)
        {
            newList.Add(list[i]);
        }
        int rand;
        while (newList.Count>0)
        {
            rand = Random.Range(0, newList.Count);
            outputList.Add(newList[rand]);
            newList.RemoveAt(rand);
        }
        return outputList;
    }
    public static void PrintList<T>(List<T> list)
    {
        string s = "";
        for (int i = 0; i < list.Count; i++)
        {
            s += list[i].ToString() + " "; 
        }
        Debug.Log(s);
    }
    public static void PrintArray<T>(T[] array)
    {
        string s = "";
        for (int i = 0; i < array.Length; i++)
        {
            s += array[i].ToString() + " "; 
        }
        Debug.Log(s);
    }
    public static GameObject DebugCube(Vector3 pos,  Color color, float scale = 0.2f)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.position = pos;
        obj.transform.localScale *= scale;
        obj.GetComponentInChildren<MeshRenderer>().material.color = color;
        return obj;
    }
    public static GameObject DebugSphere(Vector3 pos,  Color color, float scale = 0.2f)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.transform.position = pos;
        obj.transform.localScale *= scale;
        obj.GetComponentInChildren<MeshRenderer>().material.color = color;
        return obj;
    }
    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
    public static bool IsNull(Object obj)
    {
        if (obj == null)
        {
            Debug.Log("Obj null");
            return true;
        }
        else
        {
            Debug.Log("Obj not null");
            return false;
        }
    }
    public static void Pause(bool pause = true)
    {//
        #if UNITY_EDITOR
        EditorApplication.isPaused = pause;
#endif
    }
    public static void Beep()
    {//
        #if UNITY_EDITOR
        EditorApplication.Beep();
#endif
    }
}
