using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallary : MonoBehaviour
{
    [SerializeField]
    Image image;

    public List<Sprite> spriteList = new List<Sprite>();

    
    public bool isGallaryAutomaticShowEnabled;
    public float timeGallaryAutomaticInterval;
    void FixedUpdate()
    {
        if (isGallaryAutomaticShowEnabled)
        {          
          StartCoroutine(GetNewImageFromListOverTime(timeGallaryAutomaticInterval));           
        }
   
    }

    private IEnumerator GetNewImageFromListOverTime(float _time)
    {
        
        if (spriteList.Count != 0)
        {
            for (int i = 0; i < spriteList.Count; i++)
            {
                image.sprite = spriteList[i];
                yield return new WaitForSecondsRealtime(_time);
                if (i == 0)
                {
                    isGallaryAutomaticShowEnabled = false;
                }
                else if (i == spriteList.Count - 1)
                {
                    isGallaryAutomaticShowEnabled = true;

                }              
            }
        }
    }

}
