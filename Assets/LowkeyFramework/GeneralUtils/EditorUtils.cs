#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorUtils
{
    public static IEnumerable<T> FindAssetsByType<T>() where T : Object
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        foreach(string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if(asset != null)
            {
                yield return asset;
            }
        }
    }
}
#endif