using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
        imageContent = new ImageContent();
        audioContent = new AudioContent();
        JsonUrl = "https://raw.githubusercontent.com/MarinRatkovicWork/Novena_Unity3D_TEST/AfterDeadlineBranch/DataFile.json";

        DownloadAllContent();
    }

    public void DownloadAllContent()
    {
        StartCoroutine(DownloadJsonFile(JsonUrl, JsonName, (isDone) =>
        {
            if (isDone)
            {
                foreach (TranslatedContents TC in JsonData.TranslatedContents)
                {
                    foreach (Topics TP in TC.Topics)
                    {
                        foreach (Media ME in TP.Media)
                        {
                            if (ME.FilePath != null)
                            {
                                StartCoroutine(DownloadAudioFromWeb(ME.FilePath, ME.Name));
                                
                            }
                            else
                            {
                                foreach (Photos PH in ME.Photos)
                                {
                                    StartCoroutine(DownloadImageFromWeb(PH.Path, PH.Name));
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                Debug.Log("Somting went wrong!!");
            }

        }));
  
    }
    private IEnumerator DownloadJsonFile(string _Url, string _Name, Action<bool> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(_Url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Error while loding image from web: " + www.error);
                callback(false);
            }
            else
            {
                string jsonString = www.downloadHandler.text;
                jsonContent.SaveJson(_Name, jsonString);
                JsonData = jsonContent.LoadJson(JsonName);
                callback(true);
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
                audioContent.SaveAudio(_Name,bytes);
            }
        }

    }    
    public void GetAudioFromFile(string _AudioName,AudioSource _AudioSwopTarget)
    {
        StartCoroutine(audioContent.LoadAudio(_AudioName,(callClip) =>
        {
            _AudioSwopTarget.clip = callClip;
        }));
    }
    public Sprite GetImageFromeFile(string _ImageName)
    {
       
       return imageContent.LoadImageSprite(_ImageName);

    }

   
}



