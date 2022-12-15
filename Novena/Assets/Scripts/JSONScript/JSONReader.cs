using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class JSONReader : MonoBehaviour
{
    [TextArea(20,15)]
   public string jsonURL;
   public JSONData jsonData;
    void Start()
    {
        StartCoroutine(LoadJsonFile());
    }

    public IEnumerator LoadJsonFile()
    {
        WWW _www = new WWW(jsonURL);
        yield return _www;
        if (_www.error == null)
        {
            processJsonData(_www.text);
            Debug.Log("Sucessful jasonData downlad");

        }
        else
        {

            Debug.Log("Error while downloding jasonData!!!");
        }
    }
    private void processJsonData(string _url)
    {
        JSONData data = JsonUtility.FromJson<JSONData>(_url);
        jsonData = data;
        Debug.Log(jsonData.TranslatedContents);
    }
}
