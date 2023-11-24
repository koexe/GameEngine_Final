using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GameMNG : MonoBehaviour
{
    private static GameMNG _instance;
    public JsonMNG.LocationInfo_Contains_ALL g_cCurrentLocationInfo;
    public List<JsonMNG.LocationInfo_Contains_ALL> g_AllLocationInfoList;

    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static GameMNG Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameMNG)) as GameMNG;

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
    }

    public class Triggers
    {
        bool First_Trigger;
        bool Second_Trigger;

        public Triggers()
        {


        }
    }
    private void SaveFunc(Triggers Save)
    {
        string path = Application.persistentDataPath + "Save.json";

        string SaveData = JsonUtility.ToJson(Save);
        File.WriteAllText(path,SaveData);
    }
    private void LoadFunc()
    {
        string path = Application.persistentDataPath + "Save.json";
        if (File.Exists(path) == false)
        {
            
        }

    }

}
