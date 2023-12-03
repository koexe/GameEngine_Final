using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingCTR : MonoBehaviour
{
    private float m_fTextShowSpeed;
    private string tempText = "텍스트가 나타나는 속도입니다.";
    public MainSceneMNG mainSceneMNG;
    public Scrollbar g_TextShowSpeedScrollBar;
    public TextMeshProUGUI TextShowSpeed_Text;
    public TextMeshProUGUI TextShowSpeed_TextTemp;
    public Button CommitButton;
    private Coroutine cr_ShowTextShowSpeed;


    // Start is called before the first frame update
    void Start()
    {
        mainSceneMNG = GameObject.Find("MainScreenMNG").GetComponent<MainSceneMNG>();
        g_TextShowSpeedScrollBar.value = mainSceneMNG.g_fTextShowSpeed;
        CommitButton = gameObject.transform.GetComponentInChildren<Button>();
        CommitButton.onClick.AddListener(onclickEvent_commitSetting);
        cr_ShowTextShowSpeed = StartCoroutine(ShowText_Temp(tempText));
    }

    // Update is called once per frame
    void Update()
    { 
        m_fTextShowSpeed = (Mathf.Round(g_TextShowSpeedScrollBar.value * 100)/100);

        TextShowSpeed_Text.text = m_fTextShowSpeed.ToString();
    }
    public void onclickEvent_commitSetting()
    {
        mainSceneMNG.CommitSetting(m_fTextShowSpeed);
        if(cr_ShowTextShowSpeed != null)
            StopCoroutine(cr_ShowTextShowSpeed);
        mainSceneMNG.DestroyAllObjectsWithTag("Setting");
        
    }
    private IEnumerator ShowText_Temp(string text)
    {
        while (true)
        {
            string textTemp = "";
            for (int i = 0; i < text.Length; i++)
            {
                textTemp += text[i].ToString();
                TextShowSpeed_TextTemp.text = textTemp;
                yield return new WaitForSeconds(m_fTextShowSpeed);
            }
        }
    }

}