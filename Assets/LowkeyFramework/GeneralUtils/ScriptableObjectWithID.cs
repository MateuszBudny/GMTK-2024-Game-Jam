using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
#endif

// borrowed from: https://stackoverflow.com/a/77062168

public class ScriptableObjectIDAttribute : PropertyAttribute
{
}

public class ScriptableObjectWithID : ScriptableObject, IEquatable<ScriptableObjectWithID>
{
    [field: ScriptableObjectID][field: SerializeField] public string ID { get; private set; }

    public bool Equals(ScriptableObjectWithID other)
    {
        if(ReferenceEquals(null, other))
            return false;
        if(ReferenceEquals(this, other))
            return true;
        return ID == other.ID;
    }

    public override bool Equals(object obj)
    {
        if(obj is null)
            return false;
        if(ReferenceEquals(this, obj))
            return true;
        if(obj.GetType() != GetType())
            return false;
        return Equals((ScriptableObjectWithID)obj);
    }

    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return HashCode.Combine(ID);
    }

    public static bool operator ==(ScriptableObjectWithID left, ScriptableObjectWithID right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ScriptableObjectWithID left, ScriptableObjectWithID right)
    {
        return !Equals(left, right);
    }
}

#if UNITY_EDITOR
public class SOWithIDChecker : BuildPlayerProcessor
{
    public override int callbackOrder => -1;

    public override void PrepareForBuild(BuildPlayerContext buildPlayerContext)
    {
        HashSet<string> ids = new HashSet<string>();

        string[] guids = AssetDatabase.FindAssets("t:" + typeof(ScriptableObjectWithID));
        foreach(string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObjectWithID so = AssetDatabase.LoadAssetAtPath<ScriptableObjectWithID>(path);
            if(string.IsNullOrEmpty(so.ID))
            {
                Debug.LogError("SO doesn't have ID", so);
                throw new Exception();
            }

            if(!ids.Add(so.ID))
            {
                throw new Exception($"SO ({so.name}) has the same ID as some other SO!");
            }
        }
    }
}

[CustomPropertyDrawer(typeof(ScriptableObjectIDAttribute))]
public class ScriptableObjectIdDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if(string.IsNullOrEmpty(property.stringValue))
        {
            property.stringValue = Guid.NewGuid().ToString();
        }

        Rect propertyRect = new Rect(position);
        propertyRect.xMax -= 100;
        Rect buttonRect = new Rect(position)
        {
            xMin = position.xMax - 100
        };

        GUI.enabled = false;
        EditorGUI.PropertyField(propertyRect, property, label, true);
        GUI.enabled = true;

        if(GUI.Button(buttonRect, "Regenerate ID"))
        {
            property.stringValue = Guid.NewGuid().ToString();
        }
    }
}
#endif