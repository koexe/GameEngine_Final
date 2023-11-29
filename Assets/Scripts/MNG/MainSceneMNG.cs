using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Reflection;
using System;

public class MainSceneMNG : MonoBehaviour
{
    private TextMeshProUGUI TMP_DialogText;
    private TextMeshProUGUI TMP_CharacterText;
    private TextMeshProUGUI TMP_LocationText;
    private TextMeshProUGUI TMP_CharacterNameText;
    public GameObject Canvas_MainScene;
    private GameObject Background_Character;
    public GameObject Background_Dialog;
    private GameObject ChoosableTextPrefab;
    private GameObject CharacterImage;
    private GameObject CharacterImage_Right;
    private GameObject CharacterImage_Left;
    private GameObject Prefab_CharacterIcon;
    private GameObject Prefab_LogText;
    private Image BackGroundImage;

    public Dictionary<string, ChoiceAfterFuncDelegate> g_dicChoiceAfterFunc;
    public delegate void ChoiceAfterFuncDelegate();

    public JsonMNG.Dialogs g_cCurrentDialog;
    public JsonMNG.Character_Contains_Quest g_cCurrentCharacter;

    private int m_iCurrentDialogIndex;
    public bool isChangeText = true;
    public bool isChangeCharacter = true;
   

    private string m_sLocation;
    // Start is called before the first frame update
    void Start()
    {
        //GameMNG.Instance.g_cCurrentLocationInfo = GameMNG.Instance.g_AllLocationInfoList[0];
        FirstInit();
    }

    #region 장소, 캐릭터, 다이얼로그 변경 기능
    public void ChangeDialog(JsonMNG.Dialogs dialogs)
    {
        isChangeCharacter = false;
        // 현재 진행되는 다이얼로그를 변경하는 기능
        if (GameMNG.Instance.g_PlayerTriggerDic[dialogs.DialogID] == true)
        {
            m_iCurrentDialogIndex = dialogs.Dialog.Count-1;
        }
        else
        {
            m_iCurrentDialogIndex = 0;
        }
        g_cCurrentDialog = dialogs;
        TMP_DialogText.text = g_cCurrentDialog.Dialog[m_iCurrentDialogIndex];
        GameMNG.Instance.g_PlayerTriggerDic[dialogs.DialogID] = true;
        isChangeText = true;
        ChangeImage();
        ChangeName();
    }
    public void ChangeCharacter(JsonMNG.Character_Contains_Quest character)
    {
        //캐릭터 클릭 시 해당 캐릭터의 퀘스트와 다이얼로그를 보여주는 기능
        DestroyAllObjectsWithTag("QuestText");
        DestroyAllObjectsWithTag("ChoiceText");
        TMP_CharacterNameText.text = character.Name;
        TMP_DialogText.text = character.CharacterBasicDialog;
        g_cCurrentCharacter = character;
        isChangeText = false;

    }
    public void ChangeLocation(string Name)
    {
        //장소의 이름으로 Location을 변경하는 기능
        isChangeText = true;
        ChangeLocationInit(GameMNG.Instance.g_AllLocationInfoList[GameMNG.Instance.g_AllLocationInfoList.FindIndex(item => item.LocationName == Name)]);
        Destroy(GameObject.Find("ScrollBar"));
    }


    public void OnMouseDownEvent_DIalogWindow()
    {
        // 대화창 클릭 시 다이얼로그를 앞으로 진행시키는 기능
        if (g_cCurrentDialog != null)
        {
            if (isChangeText)
            {
                ChangeText();
                ChangeImage();
                ChangeName();
                TMP_DialogText.text = g_cCurrentDialog.Dialog[m_iCurrentDialogIndex];
            }
            else {
                ChangeDialog(GameMNG.Instance.g_cCurrentLocationInfo.DescriptionDialog);
            }
        }
    }

    private void ChangeImage()
    {
        for (int j = 0; j < CharacterImage.transform.childCount; j++)
        {
            CharacterImage.transform.GetChild(j).GetComponent<Image>().sprite = null;
            CharacterImage.transform.GetChild(j).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }

        if (g_cCurrentDialog.Dialog_CharacterInfo[m_iCurrentDialogIndex].Count != 0 && g_cCurrentDialog.Dialog_CharacterInfo[m_iCurrentDialogIndex] != null)
        {
            for (int i = 0; i < g_cCurrentDialog.Dialog_CharacterInfo[m_iCurrentDialogIndex].Count; i++)
            {
                CharacterImage.transform.GetChild(i).GetComponent<Image>().sprite =
                    (Sprite)GetFieldValue(GameMNG.Instance.g_ImageDIc[g_cCurrentDialog.Dialog_CharacterInfo[m_iCurrentDialogIndex][i].Character],
                    g_cCurrentDialog.Dialog_CharacterInfo[m_iCurrentDialogIndex][i].ImageType);

                CharacterImage.transform.GetChild(i).GetComponent<Image>().color = new Color(255, 255, 255, 100);
                CharacterImage.transform.GetChild(i).GetComponent<Image>().SetNativeSize();
            }
        }
        //Debug.Log("ChangeImage 호출됨");
    }

    #endregion

    #region Text 제어
    private void ChangeText()
    {   
        m_iCurrentDialogIndex++;
        if (m_iCurrentDialogIndex >= g_cCurrentDialog.Dialog.Count && g_cCurrentDialog.Choices.Count == 0)
        {
            m_iCurrentDialogIndex = 0;
            isChangeCharacter = true;
            ChangeDialog(GameMNG.Instance.g_cCurrentLocationInfo.DescriptionDialog);
        }
        else if (m_iCurrentDialogIndex >= g_cCurrentDialog.Dialog.Count && g_cCurrentDialog.Choices.Count != 0)
        {
            m_iCurrentDialogIndex = g_cCurrentDialog.Dialog.Count - 1;
            ChoiceShowFunc();
        }

        //Debug.Log("ChangeText 호출됨");
    }

    private void ChangeName()
    {
        string Text = "";
        int count = 0;
        for (int i = 0; i < g_cCurrentDialog.Dialog_CharacterInfo[m_iCurrentDialogIndex].Count; i++)
        {
            if (g_cCurrentDialog.Dialog_CharacterInfo[m_iCurrentDialogIndex][i].isTalking)
            {
                if (count > 0)
                { Text += ", ";}
                Text += g_cCurrentDialog.Dialog_CharacterInfo[m_iCurrentDialogIndex][i].Character;
                count++;
            }
        }
        TMP_CharacterNameText.text = Text;
    }
    #endregion

    #region Init 기능들
    private void FirstInit()
    {
        //초기화 함수, 텍스트 요소들을 초기화합니다.
        ChoosableTextPrefab = Resources.Load<GameObject>("Prefab/ChoosableText");
        Prefab_LogText = Resources.Load<GameObject>("Prefab/LogScreen");
        Prefab_CharacterIcon = Resources.Load<GameObject>("Prefab/CharacterIcon");

        Canvas_MainScene = GameObject.Find("Canvas");
        TMP_DialogText = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
        TMP_LocationText = GameObject.Find("LocationText").GetComponent<TextMeshProUGUI>();
        TMP_CharacterNameText = GameObject.Find("NameText").GetComponent <TextMeshProUGUI>();
        Background_Character = GameObject.Find("CharacterBackground");
        Background_Dialog = GameObject.Find("DialogBackground");
        CharacterImage_Right = GameObject.Find("CharacterImage_Right");
        CharacterImage_Left = GameObject.Find("CharacterImage_Left");
        CharacterImage = GameObject.Find("CharacterImage");
        


        TMP_FontAsset font = Resources.Load<TMP_FontAsset>("Font/malgun668 SDF 1");
        g_cCurrentCharacter = null;

        TMP_DialogText.font = font;
        TMP_LocationText.font = font;

        BackGroundImage = GameObject.Find("BackgroundImage").GetComponent<Image>();

        InitChoiceAfterFuncDic();
        ChangeLocation("Scene_1");
    }

    private void ChangeLocationInit(JsonMNG.LocationInfo_Contains_ALL location)
    {
        //장소를 바꿨을 때의 초기화입니다.
        GameMNG.Instance.g_cCurrentLocationInfo = location;
        g_cCurrentCharacter = null;
        g_cCurrentDialog = location.DescriptionDialog;
        BackGroundImage.sprite = location.LocationBackGroundImage;
        if (location.LocationBackGroundImage == null) { Debug.Log("Missing BackgroundImage"); }

        DestroyAllObjectsWithTag("CharacterText");
        DestroyAllObjectsWithTag("ChoiceText");
        DestroyAllObjectsWithTag("QuestText");
        InitCharacter();
        InitText();
        ChangeImage();
        ChangeName();

    }

    private void InitCharacter()
    {
        //장소에 속한 캐릭터들을 초기화하는 기능입니다.
        for (int i = 0; i < GameMNG.Instance.g_cCurrentLocationInfo.CharacterList.Count; i++)
        {
            GameObject CharcterTextTemp = Instantiate(Prefab_CharacterIcon, Background_Character.transform);

            ChoosableTextCTR ChoosableTextCTR_Temp = CharcterTextTemp.transform.GetComponentInChildren<ChoosableTextCTR>();
            ChoosableTextCTR_Temp.mainSceneMNG = gameObject.transform.GetComponent<MainSceneMNG>();
            ChoosableTextCTR_Temp.transform.GetComponentInChildren<ChoosableTextCTR>().g_iCharacterIndex = i;
            ChoosableTextCTR_Temp.g_sTextType = "Character_Name_Text";
            ChoosableTextCTR_Temp.Text.text = GameMNG.Instance.g_cCurrentLocationInfo.CharacterList[i].Name;
            ChoosableTextCTR_Temp.Text.fontSize = 30;
            ChoosableTextCTR_Temp.Text.alignment = TextAlignmentOptions.Center;
            CharcterTextTemp.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + GameMNG.Instance.g_cCurrentLocationInfo.CharacterList[i].Name + "/Icon");



            RectTransform rect = CharcterTextTemp.transform.GetComponent<RectTransform>();


            CharcterTextTemp.transform.tag = "CharacterText";

            Vector3 Pos = new Vector3(0.0f, 200.0f - 180 * i, 0.0f);
            rect.anchoredPosition = Pos;
        }
    }
    public void InitChoiceAfterFuncDic()
    {
        g_dicChoiceAfterFunc = new Dictionary<string, ChoiceAfterFuncDelegate>();


        g_dicChoiceAfterFunc.Add("Move_Scene_2", Move_Scene_2);
    }


    private void InitText()
    {
        //텍스트들을 초기화합니다.
        m_iCurrentDialogIndex = 0;
        g_cCurrentDialog = GameMNG.Instance.g_cCurrentLocationInfo.DescriptionDialog;
        TMP_DialogText.text = g_cCurrentDialog.Dialog[m_iCurrentDialogIndex];
        TMP_LocationText.text = GameMNG.Instance.g_cCurrentLocationInfo.LocationName;
    }
    #endregion

    #region 메뉴 기능

    public void LogShowFunc()
    {
        string LogText_Temp = "";
        for (int i = 0; i <= m_iCurrentDialogIndex; i++)
        {
            string Text = "";
            int count = 0;
            for (int j = 0; j < g_cCurrentDialog.Dialog_CharacterInfo[i].Count; j++)
            {
                if (g_cCurrentDialog.Dialog_CharacterInfo[i][j].isTalking)
                {
                    if (count > 0)
                    { Text += ", "; }
                    Text += g_cCurrentDialog.Dialog_CharacterInfo[i][j].Character;
                    count++;
                }
            }
            LogText_Temp += Text;
            LogText_Temp += "\n";
            
            LogText_Temp += g_cCurrentDialog.Dialog[i];
            LogText_Temp += "\n";
        }
        GameObject LogText_Instance = Instantiate(Prefab_LogText, Canvas_MainScene.transform);
        LogText_Instance.GetComponentInChildren<Button>().onClick.AddListener(DestroyLogText);
        LogText_Instance.GetComponentInChildren<TextMeshProUGUI>().text = LogText_Temp;  
    }

    public void Skip()
    {

    }

    public void ShowMenu()
    {

    }

    #endregion

    #region 유틸리티 기능
    private void ChoiceShowFunc()
    {
        DestroyAllObjectsWithTag("ChoiceText");
        for (int i = 0; i < g_cCurrentDialog.Choices.Count; i++)
        {
            bool isShow_Temp = true;
            if (g_cCurrentDialog.Choices[i].ChoiceTrigger.Count!= 0)
            {
                for (int j = 0; j < g_cCurrentDialog.Choices[i].ChoiceTrigger.Count; j++)
                {
                    if (GameMNG.Instance.g_PlayerTriggerDic[g_cCurrentDialog.Choices[i].ChoiceTrigger[j].Trigger_Dialog] !=
                    g_cCurrentDialog.Choices[i].ChoiceTrigger[j].Type)
                    {
                        isShow_Temp = false;
                    }
                }
            }
            if(isShow_Temp)
            {
                GameObject Choice_Temp = Instantiate(ChoosableTextPrefab, Background_Dialog.transform);
                ChoosableTextCTR choosableTextCTR_Temp = Choice_Temp.GetComponent<ChoosableTextCTR>();
                choosableTextCTR_Temp.mainSceneMNG = gameObject.transform.GetComponent<MainSceneMNG>();
                choosableTextCTR_Temp.g_cChoiceInfo = g_cCurrentDialog.Choices[i];
                choosableTextCTR_Temp.g_sTextType = "Choice_Text";
                choosableTextCTR_Temp.Text.text = g_cCurrentDialog.Choices[i].ChoiceDialog;

                RectTransform rect_Image = Choice_Temp.transform.GetChild(0).GetComponent<RectTransform>();
                RectTransform rect_Text = Choice_Temp.transform.GetChild(1).GetComponent<RectTransform>();
                RectTransform rect_BG = Choice_Temp.transform.GetComponent<RectTransform>();
                Vector2 Size = new Vector2(1000, 80);
                rect_Image.sizeDelta = Size;
                rect_Text.sizeDelta = Size;

                Vector3 Pos = new Vector3(-450.0f, -50.0f - 50 * i, 0.0f);
                Choice_Temp.transform.tag = "ChoiceText";
                rect_BG.anchoredPosition = Pos;
            }
        }
    }
    public void DestroyAllObjectsWithTag(string tag)
    {
        GameObject[] Destroy_Temp = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < Destroy_Temp.Length; i++)
        {
            Destroy(Destroy_Temp[i]);
        }
    }

    private static object GetFieldValue(object instance, string fieldName)
    {
        // Reflection을 사용하여 필드를 찾음
        FieldInfo field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (field != null)
        {
            // 필드의 값을 가져옴
            return field.GetValue(instance);
        }
        else
        {
            // 필드가 없을 경우
            Console.WriteLine("Field not found");
            return null;
        }
    }
    public void DestroyLogText()
    {
        Destroy(GameObject.FindGameObjectWithTag("LogText"));
    }
    #endregion

    #region ChoiceAfterFunc
    public void Move_Scene_2()
    {
        ChangeLocation("Scene_2");
        Debug.Log("asdf");
    }

    #endregion

}
