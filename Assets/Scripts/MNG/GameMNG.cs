using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMNG : MonoBehaviour
{
    private static GameMNG _instance;
    public JsonMNG.LocationData g_cCurrentLocationInfo;
    public List<JsonMNG.LocationData> g_AllLocationInfoList;
    public Dictionary<string,bool> g_PlayerTriggerDic = new Dictionary<string,bool>();
    public Dictionary<string, ImageMNG._Image> g_ImageDIc;

    public int FontSize_Character = 30;
    public int FontSize_Choice = 50;



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
        public bool First_Trigger;
        public bool Second_Trigger;
    }
    public void SaveFunc()
    {
        string path = Application.persistentDataPath + "Save.json";
        string SaveData = JsonConvert.SerializeObject(g_PlayerTriggerDic);
        File.WriteAllText(path,SaveData);
        Debug.Log("Save Complete");
    }
    public void LoadFunc()
    {
        string path = Application.persistentDataPath + "Save.json";
        if (File.Exists(path))
        {
            System.IO.File.Delete(path);
            SaveFunc();
            string JsonDataTemp = File.ReadAllText(path);
            g_PlayerTriggerDic = JsonConvert.DeserializeObject<Dictionary<string, bool>>(JsonDataTemp);
            Debug.Log(path);

        }
        else
        {
            SaveFunc();
            LoadFunc();
            Debug.Log("Savefile missed. created new Savefile");
        }
    }
    public void ChangeScene(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }
}
