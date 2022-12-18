using Microsoft.Unity.VisualStudio.Editor;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using Image = UnityEngine.UI.Image;

public class ContentManager : MonoBehaviour
{
    private string JsonUrl;
    public string  JsonName;
    public JsonData JsonData;

    private ImageContent imageContent;
    private AudioContent audioContent;
    private JsonContent jsonContent;
    public void Start()
    {
        jsonContent = new JsonContent();
        JsonUrl = "https://raw.githubusercontent.com/MarinRatkovicWork/Novena_Unity3D_TEST/main/DataFile.json";
        StartCoroutine(DownloadJsonFile(JsonUrl,JsonName));
        
    }
    private IEnumerator DownloadJsonFile(string _Url, string _Name)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(_Url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Error while loding image from web: " + www.error);
            }
            else
            {
                string jsonString = www.downloadHandler.text;
                jsonContent.SaveJson(_Name, jsonString);
                JsonData = jsonContent.LoadJson(JsonName);
            }
        }
    }
    public IEnumerator GetJsonDataFromFile(string _Url,string _Name)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(_Url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Error while loding image from web: " + www.error);
            }
            else
            {
                byte[] bytes = www.downloadHandler.data;
                imageContent.SaveImage(_Name, bytes);
            }
        }
    }

    private IEnumerator DownloadImageFromWeb(string _Url, string _Name)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(_Url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Error while loding image from web: " + www.error);
            }
            else
            {
                byte[] bytes = www.downloadHandler.data;
                imageContent.SaveImage(_Name, bytes);                
            }
        }
    } 
    
    private IEnumerator DownloadAudioFromWeb(string _Url, string _Name)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(_Url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Error while loding audio from web: " + www.error);
            }
            else
            {
                byte[] bytes = www.downloadHandler.data;
                AudioContent.Instance.SaveAudio(_Name,bytes);
            }
        }

    } 
    
    private IEnumerator PullAudioFromLocalFile(string _Name, Action<AudioClip> callback)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + Application.persistentDataPath + "/Audio/"+_Name, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Error while loding audio from web: " + www.error);
            }
            else
            {
                Debug.Log("Audio start faching from file: ");
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);                
                yield return myClip;
                callback(myClip);
                Debug.Log("Audio end faching from file: ");
            }
        }
       

    }
    public void GetAudioFromFile(string _AudioName,AudioSource _AudioSwopTarget)
    {
        StartCoroutine(PullAudioFromLocalFile(_AudioName,(getCallback) =>
        {
            _AudioSwopTarget.clip = getCallback;
        }));
    }

    public void GetImageFromeFile(string _ImageName,Image _ImageSwopTarget)
    {
       
       _ImageSwopTarget.sprite = imageContent.LoadImageSprite(_ImageName);

    }

   
}



