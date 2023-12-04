using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoosableTextCTR : MonoBehaviour, IPointerClickHandler
{
    public MainSceneMNG mainSceneMNG;
    public GameObject Character_QuestText;
    public GameObject Canvas_MainScene;
    public TextMeshProUGUI Text;
    public GameObject CheckImage;


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

            CheckImage.SetActive(mainSceneMNG.CheckSeenDialog(g_cCharacterInfo));
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
            mainSceneMNG.ShowQuest(g_cCharacterInfo);
        }
        else if (g_sTextType == "Quest_Text")
        {
            mainSceneMNG.ChangeDialog(g_cDialogInfo);
            mainSceneMNG.DestroyAllObjectsWithTag("QuestText");
        }
        else if (g_sTextType == "Choice_Text")
        {
            mainSceneMNG.isChangeText = true;
            if(g_cChoiceInfo.ChoiceLinkedDialogID != "") 
            {
                string CurrentQuestID = mainSceneMNG.g_cCurrentDialog.QuestID;
                List<JsonMNG.Dialogs> CurrentCharacterDialogInfo = mainSceneMNG.g_cCurrentCharacter.Dialog_Info[mainSceneMNG.g_cCurrentDialog.QuestID];
                JsonMNG.Dialogs ChoiceLinkedDialog = mainSceneMNG.g_cCurrentCharacter.Dialog_Info[CurrentQuestID]
                    [CurrentCharacterDialogInfo.FindIndex(LinkedQuest => LinkedQuest.DialogID == g_cChoiceInfo.ChoiceLinkedDialogID)];

                mainSceneMNG.ChangeDialog(ChoiceLinkedDialog);
                mainSceneMNG.DestroyAllObjectsWithTag("ChoiceText");

            }
            else
            {
                if (g_cChoiceInfo.ChoiceAfterFunction != "")
                {
                    mainSceneMNG.g_dicChoiceAfterFunc[g_cChoiceInfo.ChoiceAfterFunction]();
                }
                else
                {
                    mainSceneMNG.ChangeDialog(GameMNG.Instance.g_cCurrentLocationInfo.DescriptionDialog);
                }
                mainSceneMNG.DestroyAllObjectsWithTag("ChoiceText");
            }

        }
    }
}