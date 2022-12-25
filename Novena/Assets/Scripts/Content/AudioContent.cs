
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AudioContent
{
    string SavePath;
    public bool AudioExists(string _name)
    {
        SavePath = Application.persistentDataPath + "/Audio/";
        return File.Exists(SavePath + _name);
    }
    public void SaveAudio(string _name, byte[] _bytes, Action<bool> _isSaveFinish)
    {
        
        SavePath = Application.persistentDataPath + "/Audio/";
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
            _isSaveFinish(true);
        }
        if (!AudioExists(_name))
        {
            File.WriteAllBytes(SavePath + _name, _bytes);
            _isSaveFinish(true);
            Debug.Log("Audio writen to file: " + SavePath + _name);
        }
        else
        {
            _isSaveFinish(true);
        }
    }
    public IEnumerator LoadAudio(string _Name, Action<AudioClip> _callback)
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
                _callback(myClip);
                Debug.Log("Audio end faching from file: ");
            }
        }


    }
}
