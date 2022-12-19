using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    Slider audioBar;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    Button audioPlayPauseButton;
    [SerializeField]
    GameObject play;
    [SerializeField]
    GameObject pause;
    [SerializeField]
    Text timeText;

   public AudioSource audio = new AudioSource();
   public bool isPlaying = false;
    public void Start()
    {        
        audio =audioSource;
        audioPlayPauseButton.GetComponent<Button>().onClick.AddListener(() => PressPlay());
        SetAudioFile();
    }
    public void Update()
    {
         audioSource.clip = audio.clip;
         SetAudioFile();
         audioBar.value = audioSource.time;
         timeText.text = FormatTime(audioSource.time) + " / " + FormatTime(audioSource.clip.length);
         PlayButtonGrafic();
    }
    private void SetAudioFile()
    {
        audioBar.maxValue = audioSource.clip.length;
        audioBar.onValueChanged.AddListener(delegate { audioSource.time = audioBar.value; });
        audioBar.value = audioSource.time;
        
    }
    private void PressPlay()
    {
        if (isPlaying == false)
        {
            isPlaying = true;
        }
        else
        {
            isPlaying = false;
        }      
        PlayAudio();    
    }
    private void PlayAudio()
    {
        if (isPlaying == false)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
    }
    private void PlayButtonGrafic()
    {
        if (isPlaying == false)
        {
            play.SetActive(true);
            pause.SetActive(false);
        }
        else
        {
            play.SetActive(false);
            pause.SetActive(true);
        }
    }
    private string FormatTime(float _time)
    {
        int minutes = (int)_time / 60;
        int seconds = (int)_time - 60 * minutes;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
