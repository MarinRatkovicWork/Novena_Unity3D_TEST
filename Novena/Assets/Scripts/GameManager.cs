using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class GameManager : MonoBehaviour
{
    [Header("Screen 1 - Refrences")]
    [SerializeField]
    GameObject Page1;
    [SerializeField]
    GameObject LanguageButtonHolder;
    [Space]

    [Header("Screen 2 - Refrences")]
    [SerializeField]
    GameObject Page2;
    [SerializeField]
    GameObject TopicContainer;
    [SerializeField]
    Button ReturnPage2;
    [Space]

    [Header("Screen 3 - Refrences")]
    [SerializeField]
    GameObject Page3;
    [SerializeField]
    Text TopicNumber;
    [SerializeField]
    Text TopicName;
    [SerializeField]
    Button ReturnPage3;
    [SerializeField]
    GameObject AudioBar;
    [SerializeField]
    RawImage ImageHolder;
    [SerializeField]
    Button AudioPlayPauseButton;
    [SerializeField]
    GameObject Play;
    [SerializeField]
    GameObject Pause;

    [Space]

    [Header("Screen 4 - Refrences")]
    [SerializeField]
    GameObject Page4;
    [Space]

    [Header("Prefabs spownObjects")]
    [SerializeField]
    GameObject TopicButton;
    [SerializeField]
    GameObject LanguageButtonPrefab;

    AudioSource AudioSource;
    bool PlayingAudio= false;
    JSONData Jdata = new JSONData();
    public float PictureSwopInterva = 5;
    private bool isGallaryStop;
    public List<GameObject> gallary = new List<GameObject>();

    public void Start()
    {
        isGallaryStop = false;
        EnablePage(1);
        Debug.Log(Application.persistentDataPath);
    }
    public void Update()
    {
        if (Page3.active != false)
        {
            AudioBar.GetComponentInChildren<Text>().text = FormatTime(AudioSource.time) + " / " + FormatTime(AudioSource.clip.length);
            AudioBar.GetComponent<Slider>().value = AudioSource.time;
            if (isGallaryStop == false)
            {
                StartCoroutine(SetUpGallary(gallary));
            }
        } 
        
        
    }
    public void FixedUpdate()
    {

        if (this.gameObject.GetComponent<JSONReader>().jsonData != Jdata)
        {
            Jdata = this.gameObject.GetComponent<JSONReader>().jsonData;
            InstantiateAllLanguages(Jdata);

        }
    }

    private void InstantiateNewLanguage(TranslatedContents _data)
    {
        GameObject button = Instantiate(LanguageButtonPrefab);
        button.transform.SetParent(LanguageButtonHolder.transform);
        button.GetComponentInChildren<Text>().text = _data.LanguageName;
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        button.GetComponent<Button>().onClick.AddListener(() => InstantiateTopics(_data));
    }

    private void InstantiateTopics(TranslatedContents _data)
    {
        int counter = 1;
        EnablePage(2);
        foreach (Topics topic in _data.Topics)
        {
            GameObject button = Instantiate(TopicButton);
            button.transform.SetParent(TopicContainer.transform);
            button.GetComponent<TopicButton>().TopicText.text = topic.Name;
            button.GetComponent<TopicButton>().TopicNumberText.text = counter.ToString();
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                SetUpPage3(topic, counter);
                EnablePage(3);
            });
            ReturnPage2.GetComponent<Button>().onClick.AddListener(() =>
            {
                EnablePage(1);
                DestroyAllChidren(TopicContainer);
            });
            counter++;
        }
    }

    private void DestroyAllChidren(GameObject _object)
    {
        for (int i = 0; _object.transform.childCount > i; i++)
        {
            Destroy(_object.transform.GetChild(i).gameObject);
        }
    }


    private void InstantiateAllLanguages(JSONData _data)
    {
        foreach (TranslatedContents tc in _data.TranslatedContents)
        {
            Debug.Log("Izvrseno");
            InstantiateNewLanguage(tc);
        }
    }

    private void EnablePage(int pageNumber)
    {
        List<GameObject> list = new List<GameObject>();
        list.Add(Page1);
        list.Add(Page2);
        list.Add(Page3);
        list.Add(Page4);
        for (int i = 0; list.Count > i; i++)
        {
            if (pageNumber - 1 == i)
            {
                list[i].gameObject.SetActive(true);
            }
            else
            {
                list[i].gameObject.SetActive(false);
            }
        }
    }

    private void SetUpPage3(Topics _data, int _topicNumber)
    {
        ReturnPage3.GetComponent<Button>().onClick.AddListener(() => EnablePage(2));
        TopicNumber.text = _topicNumber.ToString();
        TopicName.text = _data.Name;
        gallary.Clear();
        foreach(Media MD in _data.Media)
        {
            
              LoadGallaryRawImagesFromDisk(MD);
            if(MD.FilePath != null)
            {
               // LoadAudioClipFromDisk(MD.Name);
            }
        }
        
        SetAudioFile(_data);       
    }

    private IEnumerator SetUpGallary(List<GameObject> _gallery)
    {
      for(int i = 0; i < _gallery.Count; i++)
        {            
            ImageHolder.texture = _gallery[i].GetComponent<RawImage>().texture;
            yield return new WaitForSecondsRealtime(PictureSwopInterva);
            if(i== 0)
            {
                isGallaryStop = true;
            }
            else if(i== _gallery.Count - 1)
            {
                isGallaryStop = false;

            }
        }
     
    }
    private void SetAudioFile(Topics _data)
    {
        AudioSource = AudioBar.GetComponent<AudioSource>();
        AudioPlayPauseButton.GetComponent<Button>().onClick.RemoveAllListeners();
        AudioBar.GetComponent<Slider>().maxValue = AudioSource.clip.length;
        AudioBar.GetComponent<Slider>().onValueChanged.AddListener(delegate{AudioSource.time = AudioBar.GetComponent<Slider>().value;});
        AudioBar.GetComponent<Slider>().value = AudioSource.time;
        AudioPlayPauseButton.GetComponent<Button>().onClick.AddListener(()=>PressPlay(AudioSource));        
    }

    private void GetAudioFile(Media _data)
    {
        JSONReader jR = this.gameObject.GetComponent<JSONReader>();
        jR.LoadAudio(_data.FilePath,_data.Name);
    }
    
    private void PressPlay(AudioSource _audioSource)
    {   if(PlayingAudio == false)
        {
            PlayingAudio = true;
        }
        else
        {
            PlayingAudio = false;
        }
        if(PlayingAudio == true)
        {
            
            PlayAudio(_audioSource, true);
            PlayButtonGrafic(false);
        }
        else
        {
            
            PlayAudio(_audioSource, false);
            PlayButtonGrafic(true);
        }
    }


    private void PlayAudio (AudioSource _audioSource,bool _isPlaying)
    {
        if(_isPlaying == false)
        {
            _audioSource.Pause();
        }
        else
        {
            _audioSource.Play();
        }
    }


    private void PlayButtonGrafic(bool _isPlayingSprite)
    {
        if(_isPlayingSprite == true)
        {
            Play.SetActive(true);
            Pause.SetActive(false);
        }
        else
        {
            Play.SetActive(false);
            Pause.SetActive(true);
        }
    } 
    public string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;     
        return string.Format("{0:00}:{1:00}", minutes, seconds);     
    }
    //Fach data from Aplication.PersistentDataPath
    public void LoadGallaryRawImagesFromDisk(Media _data)
    {
        if (_data.Photos != null)
        {
            foreach (Photos PH in _data.Photos)
            {
                if (PH != null)
                {
                    if (File.Exists(Application.persistentDataPath + "/" + PH.Name))
                    {
                        byte[] UploadByte = File.ReadAllBytes(Application.persistentDataPath + "/" + PH.Name);
                        Texture2D texture = new Texture2D(100, 100);
                        texture.LoadImage(UploadByte);
                        GameObject obj = new GameObject();
                        obj.name = PH.Name;
                        obj.AddComponent<RawImage>();
                        obj.GetComponent<RawImage>().texture = texture;
                        gallary.Add(obj);
                    }
                    else
                    {  
                        Debug.Log("Error on LoadImage");
                    }
                }
                else
                {

                    Debug.Log("Error on LoppData load");

                }
            }
        }
    }
    public void LoadAudioClipFromDisk(string filename)
    {
        if (File.Exists(Application.persistentDataPath + "/" + filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + filename, FileMode.Open);
            AudioClip clip = (AudioClip)bf.Deserialize(file);
            file.Close();
            AudioBar.GetComponent<AudioSource>().clip = clip;
        }
        else
        {
            Debug.Log("File Not Found!");
        }

    }

}
