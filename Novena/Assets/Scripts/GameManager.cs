using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("PageDownloading/Loding")]
    [SerializeField]
    GameObject PageDownloadingLoding;
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
    AudioPlayer audioPlayer;
    [SerializeField]
    Gallary gallary;


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
       
    ContentManager contentManager;
    JsonData jsonData = new JsonData();

    public void Start()
    {
        contentManager = this.gameObject.GetComponent<ContentManager>();
        EnablePage(1);
        Debug.Log(Application.persistentDataPath);
    }

    public void FixedUpdate()
    {

        if (this.gameObject.GetComponent<ContentManager>().JsonData != jsonData)
        {
            jsonData = this.gameObject.GetComponent<ContentManager>().JsonData;
            SetUpPage1(jsonData);
 

        }
    }
    //Page methods
    private void SetUpPage1(JsonData _data)
    {
        foreach (TranslatedContents tc in _data.TranslatedContents)
        {
            GameObject button = Instantiate(LanguageButtonPrefab);
            button.transform.SetParent(LanguageButtonHolder.transform);
            button.GetComponentInChildren<Text>().text = tc.LanguageName;
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.GetComponent<Button>().onClick.AddListener(() => SetUpPage2(tc));
        }
    }
    private void SetUpPage2(TranslatedContents _data)
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
                
               StartCoroutine(SetUpPage3(topic, curentNum));
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
    private IEnumerator SetUpPage3(Topics _data, int _topicNumber)
    {
        ActivatePageDownloadingLoding("Loading...");
        ReturnPage3.GetComponent<Button>().onClick.AddListener(() => 
        {
            EnablePage(2);
            audioPlayer.isPlaying = false;
        }) ;
        TopicNumber.text = _topicNumber.ToString();
        TopicName.text = _data.Name;
        gallary.spriteList.Clear();
        foreach (Media MD in _data.Media)
        {
            if (MD.Photos != null)
            {

                foreach (Photos PH in MD.Photos)
                {
                    gallary.spriteList.Add(contentManager.GetImageFromeFile(PH.Name));                       
                }
            }
            if(MD.FilePath != null)
            {
                contentManager.GetAudioFromFile(MD.Name,audioPlayer.audio);
                
            }
        }
        gallary.isGallaryAutomaticShowEnabled = true;
        //I know it is not idial solution but works fine becuse loding time fore audio dosn't lag more then 2 seconds
        yield return new WaitForSeconds(2);
        DeactivatePageDownloadingLoding();
    }
    //Downloding and loding pupup
    public void ActivatePageDownloadingLoding(string _textMesagge)
    {
        PageDownloadingLoding.SetActive(true);
        PageDownloadingLoding.GetComponentInChildren<Text>().text = _textMesagge;
    }
    public void DeactivatePageDownloadingLoding()
    {
        PageDownloadingLoding.SetActive(false);
    }
    //Hellper methods
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
    private void DestroyAllChidren(GameObject _object)
    {
        for (int i = 0; _object.transform.childCount > i; i++)
        {
            Destroy(_object.transform.GetChild(i).gameObject);
        }
    }
}
