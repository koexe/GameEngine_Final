using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMNG : MonoBehaviour
{
    private static AudioMNG _instance;
    private AudioSource Player_BGM;
    private AudioSource Player_SE;

    public static AudioMNG Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(AudioMNG)) as AudioMNG;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        
    }


    public void ChangeBGM(AudioClip clip)
    {
        Player_BGM.clip = clip;
        if(Player_BGM.isPlaying == false)
        {
            Player_BGM.Play();
        }
    }
    public void ChangeSE(AudioClip clip) 
    {  
        Player_SE.clip = clip;
        Player_SE.Play();
    }
}
