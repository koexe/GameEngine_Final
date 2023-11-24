using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GameMNG : MonoBehaviour
{
    private static GameMNG _instance;
    public JsonMNG.LocationInfo_Contains_ALL g_cCurrentLocationInfo;
    public List<JsonMNG.LocationInfo_Contains_ALL> g_AllLocationInfoList;

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
