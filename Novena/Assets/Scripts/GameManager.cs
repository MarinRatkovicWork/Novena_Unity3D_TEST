using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Image ImageHolder;


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
    [SerializeField]
    AudioPlayer audioPlayer;

    public float pictureSwopInterval = 5;
    private bool isGallaryStart;
    public List<Sprite> gallary = new List<Sprite>();
    
    ContentManager contentManager;
    JsonData jsonData = new JsonData();

    public void Start()
    {
        contentManager = this.gameObject.GetComponent<ContentManager>();
        isGallaryStart = false;
        EnablePage(1);
        Debug.Log(Application.persistentDataPath);
    }
    public void Update()
    {
        if (Page3.active != false)
        {
            if (isGallaryStart == false)
            {
                StartCoroutine(SetUpGallary(gallary));
            }
        } 
        
        
    }
    public void FixedUpdate()
    {

        if (this.gameObject.GetComponent<ContentManager>().JsonData != jsonData)
        {
            jsonData = this.gameObject.GetComponent<ContentManager>().JsonData;
            InstantiateAllLanguages(jsonData);

        }
    }

    //Page methods
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
            int curentNum = counter;
            GameObject button = Instantiate(TopicButton);
            button.transform.SetParent(TopicContainer.transform);
            button.GetComponent<TopicButton>().TopicText.text = topic.Name;
            button.GetComponent<TopicButton>().TopicNumberText.text = counter.ToString();
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                
                SetUpPage3(topic, curentNum);
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
    private void InstantiateAllLanguages(JsonData _data)
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
        foreach (Media MD in _data.Media)
        {
            if (MD.Photos != null)
            {

                foreach (Photos PH in MD.Photos)
                {
                        gallary.Add(contentManager.GetImageFromeFile(PH.Name));   
                    
                }
            }
            if(MD.FilePath != null)
            {
                contentManager.GetAudioFromFile(MD.Name,audioPlayer.audio);
            }
        }               
    }

    //Gallary methods 
    private IEnumerator SetUpGallary(List<Sprite> _gallery)
    {
        if (_gallery.Count != 0)
        {
            for (int i = 0; i < _gallery.Count; i++)
            {
                ImageHolder.sprite = _gallery[i];
                yield return new WaitForSecondsRealtime(pictureSwopInterval);
                if (i == 0)
                {
                    isGallaryStart = true;
                }
                else if (i == _gallery.Count - 1)
                {
                    isGallaryStart = false;

                }
            }
        }
     
    }

}
