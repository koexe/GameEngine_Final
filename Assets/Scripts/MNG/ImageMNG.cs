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
    }

    public class Image_Player : _Image 
    {
        

    }
    public class Image_Father : _Image
    {

    }
    public class Image_Bot : _Image 
    { 
    }
    public class Image_Monster: _Image
    {

    }


    private void ImageInit()
    {
        if (GameMNG.Instance.g_ImageDIc == null)
        {
            GameMNG.Instance.g_ImageDIc = new Dictionary<string, _Image> ();
        }
        Image_Player Player = LoadImage<Image_Player>("������");
        Image_Father Father = LoadImage<Image_Father>("������");
        Image_Bot Bot = LoadImage<Image_Bot>("��Ƽ");
        Image_Monster Monster = LoadImage<Image_Monster>("�𸮾� ����");
        GameMNG.Instance.g_ImageDIc.Add("������", Player);
        GameMNG.Instance.g_ImageDIc.Add("������", Father);
        GameMNG.Instance.g_ImageDIc.Add("��Ƽ", Bot);
        GameMNG.Instance.g_ImageDIc.Add("�𸮾� ����", Monster);
    }

    private T LoadImage<T>(string CharacterName)where T : _Image ,new()
    {
        T instance = new T();

        instance.Default = Resources.Load<Sprite>("Images/" + CharacterName + "/Default");

        return instance;
    }
}
