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
    private int m_iCurrentDialogIndex;
    public bool isChangeText = true;

    private string m_sLocation;
    // Start is called before the first frame update
    void Start()
    {
        GameMNG.Instance.g_cCurrentLocationInfo = GameMNG.Instance.g_AllLocationInfoList[0];
        FirstInit();
    }

    #region �ܺο��� ����� ��ɵ�
    public void ChangeDialog(JsonMNG.Dialogs dialogs)
    {
        // ���� ����Ǵ� ���̾�α׸� �����ϴ� ���
        m_iCurrentDialogIndex = 0;
        g_cCurrentDialog = dialogs;
        TMP_DialogText.text = g_cCurrentDialog.Dialog[m_iCurrentDialogIndex];
        isChangeText = true;

    }
    public void ChangeCharacter(JsonMNG.Character_Contains_Quest character)
    {
        //ĳ���� Ŭ�� �� �ش� ĳ������ ����Ʈ�� ���̾�α׸� �����ִ� ���
        TMP_DialogText.text = character.CharacterBasicDialog;
        isChangeText = false;

    }
    public void OnMouseDownEvent_DIalogWindow()
    {
        // ��ȭâ Ŭ�� �� ���̾�α׸� ������ �����Ű�� ���
        if (g_cCurrentDialog != null)
        {
            if (isChangeText)
            {
                m_iCurrentDialogIndex++;
                if (m_iCurrentDialogIndex >= g_cCurrentDialog.Dialog.Count)
                {
                    m_iCurrentDialogIndex = 0;
                }

                TMP_DialogText.text = g_cCurrentDialog.Dialog[m_iCurrentDialogIndex];
            }
        }
    }
    #endregion
    #region ���ο��� ����� ��ɵ�
    private void FirstInit()
    {
        //�ʱ�ȭ �Լ�, �ؽ�Ʈ ��ҵ��� �ʱ�ȭ�մϴ�.
        Canvas_MainScene = GameObject.Find("Canvas");
        TMP_DialogText = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
        TMP_LocationText = GameObject.Find("LocationText").GetComponent<TextMeshProUGUI>();
        TMP_FontAsset font = Resources.Load<TMP_FontAsset>("Font/BasicFont");

        TMP_DialogText.font = font;
        TMP_LocationText.font = font;

        for(int i = 0; i< GameMNG.Instance.g_cCurrentLocationInfo.CharacterList.Count; i++)
        {
            GameObject CharTemp = Resources.Load<GameObject>("Prefab/ChoosableText");

            CharTemp.GetComponent<ChoiceTextCTR>().mainSceneMNG = gameObject.transform.GetComponent<MainSceneMNG>();
            CharTemp.GetComponent<ChoiceTextCTR>().g_iCharacterIndex = i;
            CharTemp.GetComponent<ChoiceTextCTR>().g_sTextType = "Character_Name_Text";
            CharTemp.GetComponent<TextMeshProUGUI>().text = GameMNG.Instance.g_cCurrentLocationInfo.CharacterList[i].Name;


            GameObject CharTemp_Temp = Instantiate(CharTemp,Canvas_MainScene.transform);

            RectTransform rect = CharTemp_Temp.transform.GetComponent<RectTransform>();
            rect.anchorMax = new Vector2(0.0f, 0.5f);
            rect.anchorMin = new Vector2(0.0f, 0.5f);
            Vector3 Pos = new Vector3(220.0f, 280.0f, 0.0f);
            Pos.x -= 80 * i;
            rect.anchoredPosition = Pos;
        }
        m_iCurrentDialogIndex = 0;
        g_cCurrentDialog = GameMNG.Instance.g_cCurrentLocationInfo.DescriptionDialog;

        TMP_DialogText.text = g_cCurrentDialog.Dialog[m_iCurrentDialogIndex];

        TMP_LocationText.text = GameMNG.Instance.g_cCurrentLocationInfo.LocationName;
    }
    #endregion
}
