using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DIalogTextCTR : MonoBehaviour, IPointerClickHandler 
{
    public MainSceneMNG MainSceneMNG;
    private void Start()
    {
        MainSceneMNG = GameObject.Find("MainScreenMNG").GetComponent<MainSceneMNG>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        MainSceneMNG.OnMouseDownEvent_DIalogWindow();
    }
}
