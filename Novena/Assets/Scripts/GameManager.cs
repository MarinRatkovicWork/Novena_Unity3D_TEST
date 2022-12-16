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
    GameObject TopicNumber;
    [SerializeField]
    GameObject TopicName;
    [SerializeField]
    Button ReturnPage3;
    [SerializeField]
    GameObject AudioBar;
    [SerializeField]
    GameObject ImageHolder;
    [SerializeField]
    Button AudioPlayPauseButton;
    [SerializeField]
    GameObject Play;
    [SerializeField]
    GameObject Pouse;
    
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

    JSONData Jdata = new JSONData();
    public void Start()
    {
        
       EnablePage(1);
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
            button.GetComponent<Button>().onClick.AddListener(() => SetUpPage3(_data));
            ReturnPage2.GetComponent<Button>().onClick.AddListener(() => EnablePage(1));
            ReturnPage2.GetComponent<Button>().onClick.AddListener(() => DestroyAllChidren(TopicContainer));
            counter++;
        }
    }

    private void DestroyAllChidren(GameObject _object)
    {
        for( int i = 0; _object.transform.childCount > i; i++)
        {
            Destroy(_object.transform.GetChild(i).gameObject);
        }
    }
 

    private void InstantiateAllLanguages(JSONData _data)
    {
        foreach(TranslatedContents tc in _data.TranslatedContents)
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
        for(int i = 0; list.Count > i; i++)
        {
            if(pageNumber-1 == i )
            {
                list[i].gameObject.SetActive(true);
            }
            else
            {
                list[i].gameObject.SetActive(false);
            }
        }
    } 
    
    private void SetUpPage3(TranslatedContents data)
    {

    }

}
