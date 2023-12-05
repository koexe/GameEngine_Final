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
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
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
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // �Ʒ��� �Լ��� ����Ͽ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
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
