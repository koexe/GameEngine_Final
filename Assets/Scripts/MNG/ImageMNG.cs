using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class ImageMNG : MonoBehaviour
{
    private static ImageMNG _instance;
    

    public static ImageMNG Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(ImageMNG)) as ImageMNG;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    void Awake()
    {
        ImageInit();
        
    }
    public abstract class _Image
    {
        public Sprite Default;

        public Delegate getDefault;
        
        
    }

    public class Image_Player : _Image 
    {
        

    }
    public class Image_Bot : _Image 
    { 
    }


    private void ImageInit()
    {
        if (GameMNG.Instance.g_ImageDIc == null)
        {
            GameMNG.Instance.g_ImageDIc = new Dictionary<string, _Image> ();
        }
        Image_Player Player = LoadImage<Image_Player>("Player");
        Image_Bot Bot = LoadImage<Image_Bot>("Bot");
        GameMNG.Instance.g_ImageDIc.Add("Player", Player);
        GameMNG.Instance.g_ImageDIc.Add("Bot", Bot);

    }

    private T LoadImage<T>(string CharacterName)where T : _Image ,new()
    {
        T instance = new T();

        instance.Default = Resources.Load<Sprite>("Images/" + CharacterName + "/Default");

        return instance;
    }



}