using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class JsonContent : MonoBehaviour
{
    string SavePath;

    bool JsonExists(string _name)
    {
        return File.Exists(SavePath + _name);
    }

    public void SaveJson(string _name, string _jsonString)
    {
        SavePath = Application.persistentDataPath + "/Json/";
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
        if (!JsonExists(_name))
        {
            File.WriteAllText(SavePath + _name, _jsonString);
            Debug.Log("Image writen to file: " + SavePath + _name);
        }
    }

    public JsonData LoadJson(string _name)
    {
        SavePath = Application.persistentDataPath + "/Json/";

        string jsonString = File.ReadAllText(SavePath + _name);
        JsonData data = JsonUtility.FromJson<JsonData>(jsonString);
        return data;
    }

}
