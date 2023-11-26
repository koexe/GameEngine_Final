using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;


public class GameMNG : MonoBehaviour
{
    private static GameMNG _instance;
    public JsonMNG.LocationInfo_Contains_ALL g_cCurrentLocationInfo;
    public List<JsonMNG.LocationInfo_Contains_ALL> g_AllLocationInfoList;
    public Dictionary<string,bool> g_PlayerTriggerDic = new Dictionary<string,bool>();
    public int FontSize_Character = 70;
    public int FontSize_Choice = 50;



    // 인스턴스에 접근하기 위한 프로퍼티
    public static GameMNG Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
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
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
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
}
