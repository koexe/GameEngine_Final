using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImageMNG : MonoBehaviour
{
    private static ImageMNG _instance;
    

    public static ImageMNG Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
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
        public Sprite Smile;
        public Sprite Sad;
        public Sprite Angry;
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
        Image_Player Player = LoadImage<Image_Player>("윤이진");
        Image_Father Father = LoadImage<Image_Father>("윤익희");
        Image_Bot Bot = LoadImage<Image_Bot>("피티");
        Image_Monster Monster = LoadImage<Image_Monster>("몬스터");
        GameMNG.Instance.g_ImageDIc.Add("윤이진", Player);
        GameMNG.Instance.g_ImageDIc.Add("윤익희", Father);
        GameMNG.Instance.g_ImageDIc.Add("피티", Bot);
        GameMNG.Instance.g_ImageDIc.Add("몬스터", Monster);
    }

    private T LoadImage<T>(string CharacterName)where T : _Image ,new()
    {
        //_Image 인스턴스 생성
        T instance = new T();

        //각 이미지 Road
        instance.Default = Resources.Load<Sprite>("Images/" + CharacterName + "/Default");
        instance.Smile = Resources.Load<Sprite>("Images/" + CharacterName + "/Smile");
        instance.Sad = Resources.Load<Sprite>("Images/" + CharacterName + "/Sad");
        instance.Angry = Resources.Load<Sprite>("Images/" + CharacterName + "/Angry");

        //Road된 이미지 반환
        return instance;
    }
}
