using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainSceneMNG : MonoBehaviour
{
    public TextMeshProUGUI TMP_DialogText;
    public TextMeshProUGUI TMP_CharacterText;
    public TextMeshProUGUI TMP_LocationText;
    public GameObject Canvas_MainScene;
    public JsonMNG.Dialogs g_cCurrentDialog;
    public JsonMNG.Character_Contains_Quest g_cCurrentCharacter;
    private int m_iCurrentDialogIndex;
    public bool isChangeText = true;

    private string m_sLocation;
    // Start is called before the first frame update
    void Start()
    {
        GameMNG.Instance.g_cCurrentLocationInfo = GameMNG.Instance.g_AllLocationInfoList[0];
        FirstInit();
    }

    #region 외부에서 사용할 기능들
    public void ChangeDialog(JsonMNG.Dialogs dialogs)
    {
        // 현재 진행되는 다이얼로그를 변경하는 기능
        m_iCurrentDialogIndex = 0;
        g_cCurrentDialog = dialogs;
        TMP_DialogText.text = g_cCurrentDialog.Dialog[m_iCurrentDialogIndex];
        isChangeText = true;

    }
    public void ChangeCharacter(JsonMNG.Character_Contains_Quest character)
    {
        //캐릭터 클릭 시 해당 캐릭터의 퀘스트와 다이얼로그를 보여주는 기능
        TMP_DialogText.text = character.CharacterBasicDialog;
        g_cCurrentCharacter = character;
        isChangeText = false;

    }

    public void ChangeLocation(string Name)
    {
        Debug.Log(Name);

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
                m_iCurrentDialogIndex++;
                if (m_iCurrentDialogIndex >= g_cCurrentDialog.Dialog.Count && g_cCurrentDialog.Choices.Count == 0)
                {
                    m_iCurrentDialogIndex = 0;
                }
                else if (m_iCurrentDialogIndex >= g_cCurrentDialog.Dialog.Count && g_cCurrentDialog.Choices.Count != 0)
                {
                    m_iCurrentDialogIndex = g_cCurrentDialog.Dialog.Count - 1;

                    for (int i = 0; i < g_cCurrentDialog.Choices.Count; i++)
                    {
                        GameObject CharTemp = Resources.Load<GameObject>("Prefab/ChoosableText");

                        CharTemp.GetComponent<ChoiceTextCTR>().mainSceneMNG = gameObject.transform.GetComponent<MainSceneMNG>();
                        CharTemp.GetComponent<ChoiceTextCTR>().g_cChoiceInfo = g_cCurrentDialog.Choices[i];
                        CharTemp.GetComponent<ChoiceTextCTR>().g_sTextType = "Choice_Text";
                        CharTemp.GetComponent<TextMeshProUGUI>().text = g_cCurrentDialog.Choices[i].ChoiceDialog;


                        GameObject CharTemp_Temp = Instantiate(CharTemp, Canvas_MainScene.transform);

                        RectTransform rect = CharTemp_Temp.transform.GetComponent<RectTransform>();
                        rect.anchorMax = new Vector2(0.5f, 0.0f);
                        rect.anchorMin = new Vector2(0.5f, 0.0f);
                        rect.sizeDelta = new Vector2(1000, 80);

                        Vector3 Pos = new Vector3(-200.0f, 300.0f, 0.0f);
                        Pos.y -= 80 * i;

                        CharTemp_Temp.transform.tag = "ChoiceText";


                        rect.anchoredPosition = Pos;
                    }



                }

                TMP_DialogText.text = g_cCurrentDialog.Dialog[m_iCurrentDialogIndex];
            }
        }
    }


    #endregion
    #region 내부에서 사용할 기능들
    private void FirstInit()
    {
        //초기화 함수, 텍스트 요소들을 초기화합니다.
        Canvas_MainScene = GameObject.Find("Canvas");
        TMP_DialogText = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
        TMP_LocationText = GameObject.Find("LocationText").GetComponent<TextMeshProUGUI>();
        TMP_FontAsset font = Resources.Load<TMP_FontAsset>("Font/BasicFont");
        g_cCurrentCharacter = null;

        TMP_DialogText.font = font;
        TMP_LocationText.font = font;

        InitCharacter();
        InitText();
    }

    private void ChangeLocationInit(JsonMNG.LocationInfo_Contains_ALL location)
    {
        //장소를 바꿨을 때의 초기화입니다.
        GameMNG.Instance.g_cCurrentLocationInfo = location;
        g_cCurrentCharacter = null;
        GameObject[] Destroys = GameObject.FindGameObjectsWithTag("CharacterText");
        for(int i = 0; i< Destroys.Length;i++)
        {
            Destroy(Destroys[i]);
        }

        InitCharacter();
        InitText();

    }
    private void InitCharacter()
    {
        //장소에 속한 캐릭터들을 초기화하는 기능입니다.
        for (int i = 0; i < GameMNG.Instance.g_cCurrentLocationInfo.CharacterList.Count; i++)
        {
            GameObject CharTemp = Resources.Load<GameObject>("Prefab/ChoosableText");

            CharTemp.GetComponent<ChoiceTextCTR>().mainSceneMNG = gameObject.transform.GetComponent<MainSceneMNG>();
            CharTemp.GetComponent<ChoiceTextCTR>().g_iCharacterIndex = i;
            CharTemp.GetComponent<ChoiceTextCTR>().g_sTextType = "Character_Name_Text";
            CharTemp.GetComponent<TextMeshProUGUI>().text = GameMNG.Instance.g_cCurrentLocationInfo.CharacterList[i].Name;


            GameObject CharTemp_Temp = Instantiate(CharTemp, Canvas_MainScene.transform);

            RectTransform rect = CharTemp_Temp.transform.GetComponent<RectTransform>();
            CharTemp_Temp.transform.tag = "CharacterText";
            rect.anchorMax = new Vector2(0.0f, 0.5f);
            rect.anchorMin = new Vector2(0.0f, 0.5f);
            rect.sizeDelta = new Vector2(300, 80);
            Vector3 Pos = new Vector3(220.0f, 280.0f, 0.0f);
            Pos.x -= 80 * i;
            rect.anchoredPosition = Pos;
        }
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
}
