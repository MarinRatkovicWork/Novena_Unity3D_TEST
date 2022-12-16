using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
            Debug.Log("Sucessful jasonData downlad!");
        }
        else
        {
            Debug.Log("Error while downloding jasonData!!!" + _www.error.ToString());
        }
    }
    private void processJsonData(string _url)
    {
        JSONData data = JsonUtility.FromJson<JSONData>(_url);
        jsonData = data;
        Debug.Log(jsonData.TranslatedContents);

        foreach(TranslatedContents TC in jsonData.TranslatedContents)
        {
            foreach(Topics TP in TC.Topics)
            {
                foreach(Media ME in TP.Media)
                {
                    if(ME.FilePath != null)
                    {
                        //StartCoroutine(LoadAudio(ME.FilePath, ME.Name));
                    }
                    else
                    {
                        foreach(Photos PH in ME.Photos)
                        {
                            StartCoroutine(LoadImages(PH.Path,PH.Name));    
                        }
                    }
                }
            }
        }
    }

    public IEnumerator LoadImages(string _url,string _name)
    {
        
        WWW _www = new WWW(_url);
        yield return _www;
        if (!File.Exists(Application.persistentDataPath + "/" + _name))
        {
            if (_www.error == null)
            {
                byte[] dataByte = _www.texture.EncodeToPNG();
                yield return dataByte;              
                File.WriteAllBytes(Application.persistentDataPath +"/"+ _name, dataByte);
                
                Debug.Log("Sucessful image load!");
            }
            else
            {
                Debug.Log("Error while downloding image!!!" + _www.error.ToString());
            }

        }
        else
        {
            Debug.Log("Image exist in file!");

        }
        
    }
    public IEnumerator LoadAudio(string _url,string _name)
    {
        
        WWW _www = new WWW(_url);
        yield return _www;
        if (!File.Exists(Application.persistentDataPath + "/" + _name))
        {
            if (_www.error == null)
            {
                
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath +"/"+ _name, FileMode.Create);
                AudioClip audio;
                audio= _www.GetAudioClip(false, false, AudioType.WAV);
                bf.Serialize(file, audio);                                            
                Debug.Log("Sucessful audio downloding!>>" + file.Name);
                file.Close(); 
            }
            else
            {
                Debug.Log("Error while downloding audio!!!" + _www.error.ToString());
            }

        }
        else
        {
            Debug.Log("Audio exist in file!");

        }
        
    }

  
}
