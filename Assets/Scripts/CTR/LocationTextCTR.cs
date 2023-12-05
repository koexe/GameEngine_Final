using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LocationTextCTR : MonoBehaviour, IPointerClickHandler
{
    public GameObject LocationTextScorllBar;
    public MainSceneMNG mainSceneMNG;
    // Start is called before the first frame update
    void Start()
    {
        LocationTextScorllBar = Resources.Load<GameObject>("Prefab/LocationScrollView");
        mainSceneMNG = GameObject.Find("MainScreenMNG").GetComponent<MainSceneMNG>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitScrollBar(GameObject scrollBar)
    {
        
        GameObject button = Resources.Load<GameObject>("Prefab/Button");
        //Debug.Log(LocationTextScorllBar.transform.GetChild(0).GetChild(0).name);

        Transform Content = scrollBar.transform.GetChild(0).GetChild(0);
        Vector2 Size = new Vector2(180, 50 * GameMNG.Instance.g_AllLocationInfoList.Count);

        
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }

        int CountTemp = 0; 
        for (int i = 0; i < GameMNG.Instance.g_AllLocationInfoList.Count; i++)
        {
            if (mainSceneMNG.CheckTrigger(GameMNG.Instance.g_AllLocationInfoList[i].LocationShowTrigger))
            {
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameMNG.Instance.g_AllLocationInfoList[i].LocationName;
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().font = Resources.Load<TMP_FontAsset>("Font/malgun668 SDF 1");
                button.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 30.0f - 50 * CountTemp, 0);
                CountTemp++;

                GameObject button_Temp = Instantiate(button, Content);
                button_Temp.name = GameMNG.Instance.g_AllLocationInfoList[i].LocationName;
                button_Temp.transform.GetComponent<Button>().onClick.AddListener(delegate { GameObject.Find("MainScreenMNG").GetComponent<MainSceneMNG>().ChangeLocation(button_Temp.name); });
            }
        }



        scrollBar.GetComponent<RectTransform>().sizeDelta = Size;
        Content.GetComponent<RectTransform>().sizeDelta = Size;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameObject.Find("ScrollBar") == null)
        {
            GameObject ScrollBar = Instantiate(LocationTextScorllBar, GameObject.Find("Canvas").transform);
            ScrollBar.name = "ScrollBar";
            InitScrollBar(ScrollBar);
        }
        else
        {
            Destroy(GameObject.Find("ScrollBar"));
        }
        
    }
}
