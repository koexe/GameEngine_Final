using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceTextCTR : MonoBehaviour, IPointerClickHandler
{
    public MainSceneMNG mainSceneMNG;
    public GameObject Character_QuestText;
    public GameObject Canvas_MainScene;

    //텍스트 종류를 구분하기 위하여 TextType string 을 사용, Character_Name_Text, Quest_Text, Choice_Text로 구분

    public string g_sTextType;
    public int g_iCharacterIndex;

    public JsonMNG.Character_Contains_Quest g_cCharacterInfo;
    public JsonMNG.Dialogs g_cDialogInfo;
    public JsonMNG.Choice g_cChoiceInfo;


    private void Start()
    {
        Canvas_MainScene = GameObject.Find("Canvas");
        mainSceneMNG = GameObject.Find("MainScreenMNG").GetComponent<MainSceneMNG>();
        Character_QuestText = Resources.Load<GameObject>("Prefab/ChoosableText");

        if(g_sTextType == "Character_Name_Text")
        {
            g_cCharacterInfo = GameMNG.Instance.g_cCurrentLocationInfo.CharacterList[g_iCharacterIndex];
            g_cDialogInfo = null;
            g_cChoiceInfo = null;
        }
        if (g_sTextType == "Quest_Text")
        {
            g_cCharacterInfo = null;
            g_cChoiceInfo = null;
        }
        if(g_sTextType == "Choice_Text")
        {
            g_cCharacterInfo = null;
            g_cDialogInfo = null;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (g_sTextType == "Character_Name_Text")
        {
            mainSceneMNG.ChangeCharacter(g_cCharacterInfo);
            int TextAmount = 0;
            for (int i = 0; i < g_cCharacterInfo.LinkedQuests.Count; i++)
            {
                if (GameObject.Find(g_cCharacterInfo.Dialog_Info[g_cCharacterInfo.LinkedQuests[i]][0].QuestName) == null)
                {
                    bool isShow_Temp = true;
                    if (g_cCharacterInfo.Dialog_Info[g_cCharacterInfo.LinkedQuests[i]][0].QuestShowTrigger.Count != 0)
                    {
                        for (int j = 0; j < g_cCharacterInfo.Dialog_Info[g_cCharacterInfo.LinkedQuests[i]][0].QuestShowTrigger.Count; j++)
                        {

                            if (GameMNG.Instance.g_PlayerTriggerDic[g_cCharacterInfo.Dialog_Info[g_cCharacterInfo.LinkedQuests[i]][0].QuestShowTrigger[j].Trigger_Dialog] 
                                != g_cCharacterInfo.Dialog_Info[g_cCharacterInfo.LinkedQuests[i]][0].QuestShowTrigger[j].Type)
                            {
                                isShow_Temp = false;
                            }
                        }
                    }
                    if (isShow_Temp == true)
                    {
                        
                        Character_QuestText.GetComponent<TextMeshProUGUI>().text = g_cCharacterInfo.Dialog_Info[g_cCharacterInfo.LinkedQuests[i]][0].QuestName;
                        Character_QuestText.GetComponent<ChoiceTextCTR>().g_sTextType = "Quest_Text";
                        Character_QuestText.GetComponent<ChoiceTextCTR>().g_cDialogInfo = g_cCharacterInfo.Dialog_Info[g_cCharacterInfo.LinkedQuests[i]][0];
                        GameObject Character_QuestText_Temp = Instantiate(Character_QuestText, Canvas_MainScene.transform);
                        Character_QuestText_Temp.name = g_cCharacterInfo.Dialog_Info[g_cCharacterInfo.LinkedQuests[i]][0].QuestName;
                        Character_QuestText_Temp.tag = "QuestText";
                        RectTransform rect = Character_QuestText_Temp.transform.GetComponent<RectTransform>();
                        rect.anchorMax = new Vector2(0.5f, 0.0f);
                        rect.anchorMin = new Vector2(0.5f, 0.0f);

                        Vector3 Pos = new Vector3(-400.0f, 300.0f - 50 * TextAmount, 0.0f);
                        TextAmount++;
                        rect.anchoredPosition = Pos;
                    }
                }
            }
        }
        else if (g_sTextType == "Quest_Text")
        {
            mainSceneMNG.ChangeDialog(g_cDialogInfo);
            Destroy(gameObject);
        }
        else if (g_sTextType == "Choice_Text")
        {
            JsonMNG.Dialogs ChoiceLinkedDialog = mainSceneMNG.g_cCurrentCharacter.Dialog_Info[mainSceneMNG.g_cCurrentDialog.QuestID]
                [mainSceneMNG.g_cCurrentCharacter.Dialog_Info[mainSceneMNG.g_cCurrentDialog.QuestID].
                FindIndex(LinkedQuest => LinkedQuest.DialogID == g_cChoiceInfo.ChoiceLinkedDialogID)];
            mainSceneMNG.ChangeDialog(ChoiceLinkedDialog);
            GameObject[] DestroyTemp = GameObject.FindGameObjectsWithTag("ChoiceText");

            for (int i = 0; i < DestroyTemp.Length; i++)
            {
                Destroy(DestroyTemp[i]);
            }
        }
    }
}