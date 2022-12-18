
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AudioContent
{
    string SavePath;
    bool AudioExists(string _name)
    {
        return File.Exists(SavePath + _name);
    }
    public void SaveAudio(string _name, byte[] _bytes)
    {
        SavePath = Application.persistentDataPath + "/Audio/";
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
        if (!AudioExists(_name))
        {
            File.WriteAllBytes(SavePath + _name, _bytes);
            Debug.Log("Image writen to file: " + SavePath + _name);
        }
    }
    public IEnumerator LoadAudio(string _Name, Action<AudioClip> callback)
    {
        SavePath = Application.persistentDataPath + "/Audio/";
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + SavePath + _Name, AudioType.MPEG))
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
}
