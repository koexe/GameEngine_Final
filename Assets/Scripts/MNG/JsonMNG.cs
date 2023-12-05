using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static JsonMNG;
using System;

public class JsonMNG : MonoBehaviour
{
    private static JsonMNG _instance;
    private TextAsset DialogJson;
    private TextAsset CharacterJson;
    private TextAsset LocationJson;

    public List<LocationInfo_Contains_ALL> locationInfo_ALL;





    // Start is called before the first frame update
    void Awake()
    {
        InitJson();

        GC.Collect();
    }

    private void InitJson()
    {
        DialogJson = Resources.Load<TextAsset>("Json/Dialogs_main");
        CharacterJson = Resources.Load<TextAsset>("Json/Character");
        LocationJson = Resources.Load<TextAsset>("Json/Location");


        //Json 파일의 기본적인 요소로 초기화함
        LocationContainer Loc = ParseJson<LocationContainer>(LocationJson);
        CharacterContainer CharacterTemp = ParseJson<CharacterContainer>(CharacterJson);
        DialogContainer Dial = ParseJson<DialogContainer>(DialogJson);
        if(Loc == null || CharacterTemp == null || Dial == null)
        {
            Debug.Log("Json Parsing Error Occured. Plase Chack Json File exist or Check Json file formet");
        }


        List<Character_Contains_Quest> CharacterTempList = new List<Character_Contains_Quest>();
        LocationInfo_Contains_ALL Loc_con_all = new LocationInfo_Contains_ALL();
        locationInfo_ALL = new List<LocationInfo_Contains_ALL>();

        foreach (Location Location_Temp in Loc.Locations)
        {
            // Loc_all_temp 의 기본적인 요소들을 초기화
            LocationInfo_Contains_ALL Loc_all_temp = new LocationInfo_Contains_ALL();

            Loc_all_temp.LocationName = Location_Temp.LocationName;
            Loc_all_temp.CharacterList = new List<Character_Contains_Quest>();
            Loc_all_temp.LocationDescriptionID = Location_Temp.LocationDescription;
            Loc_all_temp.LocationBackGroundImage = Resources.Load<Sprite>("Images/Backgrounds/" + Location_Temp.LocationBackgroundImage);
            Loc_all_temp.LocationShowTrigger = Location_Temp.LocationShowTrigger;
            //Debug.Log("Images/" + Location_Temp.LocationBackgroundImage + ".jpg");
            Loc_all_temp.BackGroundMusic = Location_Temp.BackGroundMusic;
            Loc_all_temp.LocationContainCharacter = Location_Temp.LocationContainCharacter;

            locationInfo_ALL.Add(Loc_all_temp);

        }

        foreach (Character character in CharacterTemp.Characters)
        {
            // character_Contains_Quest_Temp 의 기본적인 요소들을 초기화
            Character_Contains_Quest character_Contains_Quest_Temp = new Character_Contains_Quest();

            character_Contains_Quest_Temp.Name = character.Name;
            character_Contains_Quest_Temp.Location = character.Location;
            character_Contains_Quest_Temp.LinkedQuests = character.LinkedQuests;
            character_Contains_Quest_Temp.Dialog_Info = new Dictionary<string, List<Dialogs>>();
            character_Contains_Quest_Temp.CharacterBasicDialog = character.CharacterBasicDialog;

            CharacterTempList.Add(character_Contains_Quest_Temp);

        }

        foreach (Dialogs dialogs in Dial.Dialog_ALL)
        {
            //Dialog info 와 dialog의 index를 비교
            if(dialogs.Dialog.Count != dialogs.Dialog_CharacterInfo.Count)
                Debug.Log("Dialog info index is not match in " + dialogs.DialogID + " Please Check this DialogID");
            
            GameMNG.Instance.g_PlayerTriggerDic.Add(dialogs.DialogID, false);

            //Dialog를 Character_Contains_Quest 형식에 QuestID를 기준으로 넣고, CharacterTempList를 초기화함
            if (dialogs.DialogType == "Description")
            {
                for(int i = 0;i<locationInfo_ALL.Count;i++)
                {
                    if (dialogs.DialogID == locationInfo_ALL[i].LocationDescriptionID)
                        locationInfo_ALL[i].DescriptionDialog = dialogs;
                }
            }
            else if (dialogs.DialogType == "Quest")
            {
                int index = CharacterTempList.FindIndex(item => item.LinkedQuests.Any(LinkedQuest => LinkedQuest == dialogs.QuestID));
                if (index == -1)
                {
                    Debug.Log("Character Quest DialogID Match Error in " +dialogs.DialogID );
                }
                else
                {
                    Character_Contains_Quest character = CharacterTempList[index];

                    if (character.Dialog_Info == null)
                    {
                        character.Dialog_Info = new Dictionary<string, List<Dialogs>>();
                    }

                    if (!character.Dialog_Info.ContainsKey(dialogs.QuestID))
                    {
                        character.Dialog_Info[dialogs.QuestID] = new List<Dialogs>();
                    }

                    character.Dialog_Info[dialogs.QuestID].Add(dialogs);

                    CharacterTempList[index] = character;
                }
            }
            else
                Debug.Log("Dialog Type Error");
        }

        foreach (Character_Contains_Quest chartemp in CharacterTempList)
        {
            for (int i = 0; i < locationInfo_ALL.Count; i++)
            {
                if(chartemp.Location == locationInfo_ALL[i].LocationName)
                {
                    if (locationInfo_ALL[i].CharacterList == null)
                    {
                        locationInfo_ALL[i].CharacterList = new List<Character_Contains_Quest>();
                    }
                    else
                    {
                        locationInfo_ALL[i].CharacterList.Add(chartemp);
                    }
                }
            }

        }
        GameMNG.Instance.g_cCurrentLocationInfo = Loc_con_all;
        GameMNG.Instance.g_AllLocationInfoList = locationInfo_ALL;
        GameMNG.Instance.LoadFunc();
    }

    private T ParseJson<T>(TextAsset json) where T : class
    {
        //Json을 파싱하는 기능입니다. 반환할 타입과 TextAsset으로 변환된 json을 지정합니다.
        try
        {
            if (json != null)
            {
                string jsonText = json.text;
                T container = JsonConvert.DeserializeObject<T>(jsonText);
                return container;
            }
            else
            {
                return null;
            }
        }
        catch (JsonReaderException jsonReaderException)
        {
            Debug.LogError("JSON Parsing Error: " + jsonReaderException.Message);
            return null;
        }
        catch (JsonSerializationException jsonSerializationException)
        {
            Debug.LogError("JSON Serialization Error: " + jsonSerializationException.Message);
            return null;
        }
    }

    [System.Serializable]
    public class Dialogs
    {
        public string DialogType;
        public string QuestID;
        public List<Trigger> QuestShowTrigger;
        public string DialogID;
        public string QuestName;
        public List<string> Dialog;
        public List<List<Dialog_CharacterInfo>> Dialog_CharacterInfo;
        public List<Choice> Choices;
    }
    [System.Serializable]
    public class Dialog_CharacterInfo
    {
        public string Character;
        public bool isTalking;
        public string ImageType;
    }

    [System.Serializable]
    public class Choice
    {
        public List<Trigger> ChoiceTrigger;
        public string ChoiceDialog;
        public string ChoiceLinkedDialogID;
        public string ChoiceAfterFunction;
    }
    [System.Serializable]
    public class Trigger
    {
        public string Trigger_Dialog;
        public bool Type;
    }

    [System.Serializable]
    public class Location
    {
        public string LocationName;
        public List<string> LocationContainCharacter;
        public string LocationBackgroundImage;
        public string LocationDescription;
        public string BackGroundMusic;
        public List<Trigger> LocationShowTrigger;
    }

    [System.Serializable]
    public class Character
    {
        public string Name;
        public string Location;
        public string CharacterBasicDialog;
        public string CharacterImageID;
        public List<string> LinkedQuests;
    }

    public class DialogContainer
    {
        public List<Dialogs> Dialog_ALL;
    }

    public class LocationContainer
    {
        public List<Location> Locations;
    }
    public class CharacterContainer
    {
        public List<Character> Characters;
    }
    public class LocationInfo_Contains_ALL
    {
        public string LocationName;
        public List<string> LocationContainCharacter;
        public List<Character_Contains_Quest> CharacterList;
        public string LocationDescriptionID;
        public Sprite LocationBackGroundImage;
        public Dialogs DescriptionDialog;
        public string BackGroundMusic;
        public List<Trigger> LocationShowTrigger;
    }
    public class Character_Contains_Quest
    {
        public string Name;
        public string Location;
        public List<string> LinkedQuests;
        public Dictionary<string,List<Dialogs>> Dialog_Info;
        public string CharacterBasicDialog;
    }
    public static JsonMNG Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(JsonMNG)) as JsonMNG;
                if (_instance == null)
                {
                    GameObject MNG = new GameObject("JsonManager");
                    _instance = MNG.AddComponent<JsonMNG>();
                }
            }
            return _instance;
        }
    }
}
