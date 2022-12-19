using System.IO;
using UnityEngine;

public class ImageContent
{
    string SavePath;  
    bool ImageExists(string _name)
    {
        return File.Exists(SavePath + _name);
    }

    public void SaveImage(string _name, byte[] _bytes)
    {
        SavePath = Application.persistentDataPath + "/Images/";
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
        if (!ImageExists(_name))
        {
            File.WriteAllBytes(SavePath + _name, _bytes);
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
