using System;
using System.Collections.Generic;


[System.Serializable]
public class JSONData
{
    public TranslatedContents[] TranslatedContents;        
}
[System.Serializable]
public class TranslatedContents
{
    public int LanguageId;
    public string LanguageName;
    public Topics[] Topics;
}
[System.Serializable]
public class Topics
{
    public string Name;
    public Media[] Media;
}
[System.Serializable]
public class Media
{
    public string Name;
    public string FilePath;
    public Photos[] Photos;
}
[System.Serializable]
public class Photos
{
    public string Path;
    public string Name;
}