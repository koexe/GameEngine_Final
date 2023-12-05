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
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
        Player_BGM = GameObject.Find("BGM_Player").GetComponent<AudioSource>();
        Player_SE = GameObject.Find("SE_Player").GetComponent<AudioSource>();
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

    public void ChangeBGMVolume(float value)
    {
        Player_BGM.volume = value;
    }
    public void ChangeSEVolume(float value)
    {
        Player_SE.volume = value;
    }
}
