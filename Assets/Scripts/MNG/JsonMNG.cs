using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class JsonMNG : MonoBehaviour
{
    private static JsonMNG _instance;
    private TextAsset DialogJson;
    private TextAsset CharacterJson;
    private TextAsset LocationJson;

    public List<LocationData> locationInfo_ALL;





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


        LocationList Loc = ParseJson<LocationList>(LocationJson);
        CharacterList CharacterTemp = ParseJson<CharacterList>(CharacterJson);
        DialogList Dial = ParseJson<DialogList>(DialogJson);
        if(Loc == null || CharacterTemp == null || Dial == null)
        {
            Debug.Log("Json Parsing Error Occured. Plase Chack Json File exist or Check Json file formet");
        }


        List<CharacterData> CharacterTempList = new List<CharacterData>();
        LocationData Loc_con_all = new LocationData();
        locationInfo_ALL = new List<LocationData>();

        foreach (Location Location_Temp in Loc.Locations)
        {
            LocationData Loc_all_temp = new LocationData();

            Loc_all_temp.LocationName = Location_Temp.LocationName;
            Loc_all_temp.CharacterList = new List<CharacterData>();
            Loc_all_temp.LocationDescriptionID = Location_Temp.LocationDescription;
            Loc_all_temp.LocationBackGroundImage = Resources.Load<Sprite>("Images/Backgrounds/" + Location_Temp.LocationBackgroundImage);
            Loc_all_temp.LocationShowTrigger = Location_Temp.LocationShowTrigger;
            Loc_all_temp.BackGroundMusic = Location_Temp.BackGroundMusic;
            Loc_all_temp.LocationContainCharacter = Location_Temp.LocationContainCharacter;

            locationInfo_ALL.Add(Loc_all_temp);

        }

        foreach (Character character in CharacterTemp.Characters)
        {
            CharacterData character_Contains_Quest_Temp = new CharacterData();

            character_Contains_Quest_Temp.Name = character.Name;
            character_Contains_Quest_Temp.Location = character.Location;
            character_Contains_Quest_Temp.LinkedQuests = character.LinkedQuests;
            character_Contains_Quest_Temp.Dialog_Info = new Dictionary<string, List<Dialogs>>();
            character_Contains_Quest_Temp.CharacterBasicDialog = character.CharacterBasicDialog;

            CharacterTempList.Add(character_Contains_Quest_Temp);

        }

        foreach (Dialogs dialogs in Dial.Dialog_ALL)
        {
            if(dialogs.Dialog.Count != dialogs.Dialog_CharacterInfo.Count)
                Debug.Log("Dialog info index is not match in " + dialogs.DialogID + " Please Check this DialogID");
            
            GameMNG.Instance.g_PlayerTriggerDic.Add(dialogs.DialogID, false);

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
                    Debug.Log("Character Quest DialogID Match Error in " + dialogs.DialogID );
                }
                else
                {
                    CharacterData character = CharacterTempList[index];

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
        foreach (CharacterData chartemp in CharacterTempList)
        {
            for (int i = 0; i < locationInfo_ALL.Count; i++)
            {
                if(chartemp.Location == locationInfo_ALL[i].LocationName)
                {
                    if (locationInfo_ALL[i].CharacterList == null)
                    {
                        locationInfo_ALL[i].CharacterList = new List<CharacterData>();
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
        public List<List<DialogCharacterInfo>> Dialog_CharacterInfo;
        public List<Choice> Choices;
    }
    [System.Serializable]
    public class DialogCharacterInfo
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

    public class DialogList
    {
        public List<Dialogs> Dialog_ALL;
    }

    public class LocationList
    {
        public List<Location> Locations;
    }
    public class CharacterList
    {
        public List<Character> Characters;
    }
    public class LocationData
    {
        public string LocationName;
        public List<string> LocationContainCharacter;
        public List<CharacterData> CharacterList;
        public string LocationDescriptionID;
        public Sprite LocationBackGroundImage;
        public Dialogs DescriptionDialog;
        public string BackGroundMusic;
        public List<Trigger> LocationShowTrigger;
    }
    public class CharacterData
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
