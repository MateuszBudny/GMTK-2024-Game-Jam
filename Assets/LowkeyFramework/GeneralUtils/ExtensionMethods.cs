//using LowkeyFramework.AttributeSaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2 Rotate(this Vector2 vector, float eulerAngle)
    {
        float radians = eulerAngle * Mathf.Deg2Rad;
        return new Vector2(
            vector.x * Mathf.Cos(radians) - vector.y * Mathf.Sin(radians),
            vector.x * Mathf.Sin(radians) + vector.y * Mathf.Cos(radians)
        );
    }

    public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    public static Vector2 RotateAroundPivot(Vector2 point, Vector2 pivot, float angle)
    {
        return (point - pivot).Rotate(angle) + pivot;
    }

    public static T GetRandomElement<T>(this List<T> list)
    {
        int chosenElementIndex = UnityEngine.Random.Range(0, list.Count);
        return list[chosenElementIndex];
    }

    public static List<T> Shuffle<T>(this List<T> list)
    {
        List<T> listCopy = new List<T>(list);
        List<T> finalShuffledList = new List<T>();
        for(int i = 0; i < listCopy.Count; i++)
        {
            T elementDrawn = listCopy.GetRandomElement();
            finalShuffledList.Add(elementDrawn);
            listCopy.Remove(elementDrawn);
        }

        return finalShuffledList;
    }

    public static void ForEach<T>(this List<T> list, Action<T, int> forEachDo)
    {
        for(int i = 0; i < list.Count; i++)
        {
            forEachDo(list[i], i);
        }
    }

    public static float RandomRangeFloat(this Vector2 minMax)
    {
        return UnityEngine.Random.Range(minMax.x, minMax.y);
    }

    public static Vector3 RandomRangeVector(this Vector3 minInclusive, Vector3 maxInclusive)
    {
        return new Vector3(UnityEngine.Random.Range(minInclusive.x, maxInclusive.x), UnityEngine.Random.Range(minInclusive.y, maxInclusive.y), UnityEngine.Random.Range(minInclusive.z, maxInclusive.z));
    }

    //public static void SafeDestroy(this GameObject gameObject)
    //{
    //    gameObject.transform.parent = null;
    //    gameObject.name = "$disposed";
    //    SaveableBehaviour saveable = gameObject.GetComponent<SaveableBehaviour>();
    //    if(saveable)
    //    {
    //        saveable.TurnOffSavingAndLoadingForThisBehaviour = true;
    //    }
    //    UnityEngine.Object.Destroy(gameObject);
    //    gameObject.SetActive(false);
    //}
}
