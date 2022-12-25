using System;
using System.IO;
using UnityEngine;

public class ImageContent
{
    string SavePath;  
    public bool ImageExists(string _name)
    {
        SavePath = Application.persistentDataPath + "/Images/";
        return File.Exists(SavePath + _name);
    }

    public void SaveImage(string _name, byte[] _bytes, Action<bool> _isSaveFinish)
    {
        SavePath = Application.persistentDataPath + "/Images/";
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
            _isSaveFinish(true);
        }
        if (!ImageExists(_name))
        {
            File.WriteAllBytes(SavePath + _name, _bytes);
            _isSaveFinish(true);
        }
        else
        {
            _isSaveFinish(true);
        }
    }

    byte[] LoadImageBytes(string _name)
    {
        byte[] bytes = new byte[0];
        if (ImageExists(_name))
        {
            bytes = File.ReadAllBytes(SavePath + _name);
        }
        return bytes;
    }

    public Sprite LoadImageSprite(string _name)
    {
        SavePath = Application.persistentDataPath + "/Images/";
        byte[] bytes = LoadImageBytes(_name);
       Texture2D texture = new Texture2D(1,1);
       texture.LoadImage(bytes);
       Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2(0.5f,0.5f));
        
       return sprite;
    }
}
