
using System.IO;
using UnityEngine;

public class AudioContent : MonoBehaviour
{
    public static AudioContent Instance;
    string SavePath;
    public void Start()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this);
            return;
        }
        Instance = this;
        SavePath =Application.persistentDataPath + "/Audio/";
    }
    bool AudioExists(string _name)
    {
        return File.Exists(SavePath + _name);
    }
    public void SaveAudio(string _name, byte[] _bytes)
    {
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

}
