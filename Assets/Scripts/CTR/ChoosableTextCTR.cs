using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoosableTextCTR : MonoBehaviour, IPointerClickHandler
{
    public MainSceneMNG mainSceneMNG;
    public GameObject Character_QuestText;
    public GameObject Canvas_MainScene;
    public TextMeshProUGUI Text;

    //�ؽ�Ʈ ������ �����ϱ� ���Ͽ� TextType string �� ���, Character_Name_Text, Quest_Text, Choice_Text�� ����

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
                        GameObject Character_QuestText_Temp = Instantiate(Character_QuestText, mainSceneMNG.Background_Dialog.transform);
                        Character_QuestText_Temp.GetComponentInChildren<TextMeshProUGUI>().text = g_cCharacterInfo.Dialog_Info[g_cCharacterInfo.LinkedQuests[i]][0].QuestName;
                        Character_QuestText_Temp.GetComponentInChildren<TextMeshProUGUI>().fontSize = GameMNG.Instance.FontSize_Choice;
                        Character_QuestText_Temp.GetComponent<ChoosableTextCTR>().g_sTextType = "Quest_Text";
                        Character_QuestText_Temp.GetComponent<ChoosableTextCTR>().g_cDialogInfo = g_cCharacterInfo.Dialog_Info[g_cCharacterInfo.LinkedQuests[i]][0];

                        Character_QuestText_Temp.name = g_cCharacterInfo.Dialog_Info[g_cCharacterInfo.LinkedQuests[i]][0].QuestName;
                        Character_QuestText_Temp.tag = "QuestText";
                        RectTransform rect = Character_QuestText_Temp.transform.GetComponent<RectTransform>();

                        Vector3 Pos = new Vector3(-560.0f, 0.0f - 50 * TextAmount, 0.0f);
                        TextAmount++;
                        rect.anchoredPosition = Pos;
                    }
                }
            }
        }
        else if (g_sTextType == "Quest_Text")
        {
            mainSceneMNG.ChangeDialog(g_cDialogInfo);
            mainSceneMNG.DestroyAllObjectsWithTag("QuestText");
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