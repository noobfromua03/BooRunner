using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;

/// <summary>
/// Base class to automatic saving and loading game data.
/// Uses Reflection and Xml to save all non-static private and public fields.
/// </summary>
/// <typeparam name="T">Type of class to save</typeparam>
[Serializable]
public class ProgressBase<T> : MonoBehaviour where T : ProgressBase<T>
{
    #region Singleton instance

    private static T p_instance;
    protected static T instance
    {
        get
        {
            if (p_instance == null)
            {
                GameObject go = new GameObject("_progress");
                p_instance = go.AddComponent<T>();
                DontDestroyOnLoad(go);
                LoadAllFields(p_instance);
            }
            return p_instance;
        }
    }
#if UNITY_EDITOR
    protected static bool HasInstance => p_instance != null;
#endif
    public static T GetInstance()
    {
        return instance;
    }

    #endregion

    #region Private Auto Methods
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) return;

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Save();
    }

    void OnApplicationQuit()
    {
        Save();
        PlayerPrefs.Save();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        Save();
    }
    #endregion

    #region Private Methods (Load/Save/Clear)

    static private void LoadAllFields(T _instance)
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        FieldInfo[] fields = _instance.GetType().GetFields(flags);
        foreach (FieldInfo fInfo in fields)
        {
            if (fInfo.FieldType.IsArray)
                LoadArrayField(fInfo);
            else
                LoadField(fInfo);
        }
    }

    static private void LoadField(FieldInfo fieldInfo)
    {
        string key = fieldInfo.FieldType.ToString() + "+" + fieldInfo.Name;

        if (PlayerPrefs.HasKey(key))
        {
            var obj = Deserialize(PlayerPrefs.GetString(key), fieldInfo.FieldType);
            fieldInfo.SetValue(instance, obj);
        }
        else
        {
            CreateDefaultField(fieldInfo);
        }
    }

    static private void LoadArrayField(FieldInfo fieldInfo)
    {
        string key = fieldInfo.FieldType.ToString() + "+" + fieldInfo.Name;

        if (PlayerPrefs.HasKey(key))
        {
            Array obj = Deserialize(PlayerPrefs.GetString(key), fieldInfo.FieldType) as Array;
            Array array = fieldInfo.GetValue(instance) as Array;
            CreateDefaultField(fieldInfo, Mathf.Max(obj.Length, array.Length));
            for (int i = obj.GetLowerBound(0); i <= Mathf.Min(obj.GetUpperBound(0), array.GetUpperBound(0)); i++)
            {
                array.SetValue(obj.GetValue(i), i);
            }
            fieldInfo.SetValue(instance, array);
        }
        else
            CreateDefaultField(fieldInfo);
    }

    static private void CreateDefaultField(FieldInfo fieldInfo, int arrayLength = -1)
    {
        if (fieldInfo.FieldType.IsArray)
        {
            if (arrayLength < 0)
                arrayLength = (fieldInfo.GetValue(instance) as Array).Length;
            Array array = Array.CreateInstance(fieldInfo.FieldType.GetElementType(), arrayLength);
            for (int i = array.GetLowerBound(0); i <= array.GetUpperBound(0); i++)
                array.SetValue(Activator.CreateInstance(fieldInfo.FieldType.GetElementType()), i);
            fieldInfo.SetValue(instance, array);
        }
        else
            fieldInfo.SetValue(instance, Activator.CreateInstance(fieldInfo.FieldType));
    }

    /// <summary>
    /// Force save field info 
    /// </summary>
    /// <param name="fieldInfo">Field to save</param>
    protected void SaveField(FieldInfo fieldInfo)
    {
        string key = fieldInfo.FieldType.ToString() + "+" + fieldInfo.Name;

        string value = Serialize(fieldInfo.GetValue(instance), fieldInfo.FieldType);
        PlayerPrefs.SetString(key, value);
    }

    /// <summary>
    /// Force save field info 
    /// </summary>
    /// <param name="fieldName">name of field</param>
    protected void SaveField(string fieldName)
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        FieldInfo[] fields = instance.GetType().GetFields(flags);
        foreach (FieldInfo fInfo in fields)
        {
            if (fInfo.Name == fieldName)
            {
                SaveField(fInfo);
                return;
            }
        }
    }

    /// <summary>
    /// Clears all fields and set default values
    /// </summary>
    protected void ClearAllFields()
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        FieldInfo[] fields = instance.GetType().GetFields(flags);
        foreach (FieldInfo fInfo in fields)
            CreateDefaultField(fInfo, fInfo.FieldType.IsArray ? (fInfo.GetValue(instance) as Array).Length : -1);
    }

    /// <summary>
    /// Change progress and load all fields
    /// </summary>
    public virtual void ChangeProgress(int newProgressIndex)
    {
        Debug.LogError(newProgressIndex);
        Save();
        LoadAllFields(instance);
    }

    #endregion

    #region Public Methods Save

    /// <summary>
    /// Save all fields by type
    /// </summary>
    /// <typeparam name="_type">Type of field to save</typeparam>
    static public void Save<_type>()
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        FieldInfo[] fields = instance.GetType().GetFields(flags);
        foreach (FieldInfo fInfo in fields)
        {
            if (fInfo.FieldType == typeof(_type))
                instance.SaveField(fInfo);
        }
    }

    /// <summary>
    /// Force save all fields
    /// </summary>
    static public void Save()
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        FieldInfo[] fields = instance.GetType().GetFields(flags);
        foreach (FieldInfo fInfo in fields)
            instance.SaveField(fInfo);
    }
    #endregion

    #region XML Helpers
    private static string Serialize(object details)
    {
        return Serialize(details, details.GetType());
    }

    private static string Serialize(object details, System.Type type)
    {
        XmlSerializer serializer = new XmlSerializer(type);
        MemoryStream stream = new MemoryStream();
        serializer.Serialize(stream, details);
        StreamReader reader = new StreamReader(stream);
        stream.Position = 0;
        string retSrt = reader.ReadToEnd();
        stream.Flush();
        stream = null;
        reader = null;
        return retSrt;
    }

    private static object Deserialize(string details, System.Type type)
    {
        XmlSerializer serializer = new XmlSerializer(type);
        XmlReader reader = XmlReader.Create(new StringReader(details));
        return serializer.Deserialize(reader);
    }
    #endregion
}

