using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class JsonController <T>
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="jsonObj"></param>
    /// <param name="path"> Path of folder in Assets like "/languages/" </param>
    /// <param name="jsonName"> File name like "RU.json" </param>
    public static void SaveIntoJson(T jsonObj, string path, string jsonName)
    {
#if UNITY_EDITOR
        string _path = Application.dataPath + path;
#elif UNITY_ANDROID
        string _path = Application.persistentDataPath + path;
#endif
        string jsonText = JsonConvert.SerializeObject(jsonObj);
        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
        }
        File.WriteAllText(_path + jsonName, jsonText);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"> Path of folder in Assets like "/languages/" </param>
    /// <param name="jsonName"> File name like "RU.json" </param>
    public static T LoadFromJson(string path, string jsonName)
    {
#if UNITY_EDITOR
        string _path = Application.dataPath + path ;
#elif UNITY_ANDROID
        string _path = Application.persistentDataPath + path;
#endif
        if (File.Exists(_path + jsonName))
        {
            T jsonObj = JsonConvert.DeserializeObject<T>(File.ReadAllText(_path + jsonName));
            return jsonObj;
        }
        Debug.LogError("File: \"" + _path + "\" - not found");
        return default(T);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endOfPath"> Path of json like "Jsons/ShipsPurchaseInfo.json" </param>
    /// <returns></returns>
    public static T LoadFromResources(string endOfPath)
    {
        string newpath = endOfPath.Replace(".json", "");
        TextAsset ta = Resources.Load<TextAsset>(newpath);
        if (ta != null)
        {
            T jsonObj = JsonConvert.DeserializeObject<T>(ta.text);
            return jsonObj;
        }
        return default(T);
    }  
}
