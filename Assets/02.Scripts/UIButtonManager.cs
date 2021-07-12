using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class UIButtonManager : MonoBehaviour
{
    private bool state = true;
    private GameObject menu;
    private GameObject menuDetail;
    [SerializeField]
    private GameObject SNSPanel;
    
    //Menu Button Group
    [SerializeField]
    private GameObject Btn_Back;
    [SerializeField]
    private GameObject Btn_Capture;
    [SerializeField]
    private GameObject Btn_GrowSlider;
    [SerializeField]
    private GameObject Btn_Detail;


    private GameObject arManager;

    public GameObject growthSliderObj;

    public GameObject DetailPanel;


    public TMP_Text[] componentTexts;

    public void Start()
    {

        menu = GameObject.FindWithTag("PANEL-MENUBTN");
        menuDetail = GameObject.FindWithTag("PANEL-MENUBACK");

        menu.SetActive(state);
        menuDetail.SetActive(!state);
        SNSPanel.SetActive(false);
        growthSliderObj.SetActive(false);

        arManager = GameObject.FindGameObjectWithTag("ARManager");

        arManager.GetComponent<ARButtonManager>().SelectOn += new EventHandler(SelectOn);
        arManager.GetComponent<ARButtonManager>().SelectOff += new EventHandler(SelectOff);
        arManager.GetComponent<ARButtonManager>().TargetLost += new EventHandler(UIBackToDefault);

        // default state : false
        SetInteractable(false);
    }

    //Event : Called if Target selected.
    void SelectOn(object sender, EventArgs e)
    {
        SetInteractable(true);
    }
    //Event : Called if there's no selected target.
    void SelectOff(object sender, EventArgs e)
    {
        growthSliderObj.GetComponentInChildren<Slider>().value = 0;
        growthSliderObj.SetActive(false);
        SetInteractable(false);
    }
    //Event : Called if selected target lost.
    void UIBackToDefault(object sender, EventArgs args)
    {
        DetailPanel.SetActive(false);
        growthSliderObj.SetActive(false);
        growthSliderObj.GetComponentInChildren<Slider>().value = 0;
        SNSPanel.SetActive(false);
        menuDetail.SetActive(true);
        SetARTargetingState(true);
    }
#region Button
    
    // Menu on
    public void OnTouchMenu()
    {
        menu.SetActive(!state);
        menuDetail.SetActive(state);
    }
    //Menu off
    public void OnTouchMenuBack()
    {
        menu.SetActive(state);
        menuDetail.SetActive(!state);
        growthSliderObj.SetActive(false);

    }
    
    // Info Button Clicked.
    public void OnInfoButtonClick()
    {
        // set AR targeting state false
        SetARTargetingState(false);

        // get data from firebase;
        SetDetailPanelData();
        // disable menu and enable info
        menuDetail.SetActive(false);
        DetailPanel.SetActive(true);
    }

    // Growth Button clicked.
    public void OnGrowthButtonClick()
    {
        // set AR targeting state false
        SetARTargetingState(false);

        menuDetail.SetActive(false);
        growthSliderObj.SetActive(true);
    }

    // Growth slider changed.
    public void OnGrowthSliderChanged(GameObject slider)
    {
        arManager.GetComponent<ARButtonManager>().OnGrowthSliderChanged(slider);
    }

    // Capture Button Clicked.
    public void OnCaptureButtonClick()
    {
        // set AR targeting state false
        SetARTargetingState(false);

        menuDetail.SetActive(false);
        SNSPanel.SetActive(true);   
    }

    // SNS Share cancel clicked.
    public void OnTouchSNSCancel()
    {
        menuDetail.SetActive(true);
        SNSPanel.SetActive(false);        
    }


    // Exit button clicked.
    public void OnExitButtonClick(GameObject targetUIObj)
    {
        
        // set AR targeting state true
        SetARTargetingState(true);
        
        growthSliderObj.GetComponentInChildren<Slider>().value = 0;
        targetUIObj.SetActive(false);
        menuDetail.SetActive(true);
    }



#endregion

    bool SetDetailPanelData()
    {
        IDictionary      = GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<FBManager>().iTargetDataDict;

        if(data == null)
        {
            return false;
        }

        componentTexts[0].text = "식재시기 : " + (string)data["식재시기"] + "\n" +  "개화시기 : " + (string)data["개화시기"];
        componentTexts[1].text = "최적온도 : " + (string)data["최적온도"] + "\n" + "월동온도 : " + (string)data["월동온도"];
        componentTexts[2].text = "키 : " + (string)data["키"] + "\n" + "꽃크기 : " + (string)data["꽃크기"];
        componentTexts[3].text = "물주기 : " + (string)data["물주기"];
        componentTexts[4].text = "환경조건 : " + (string)data["환경조건"] + "\n" + "난이도 : " + (string)data["난이도"];
        componentTexts[5].text = "용도 : " + (string)data["용도"] + "\n" + "분류 : " + (string)data["분류"];

        componentTexts[6].text = (string)data["재배포인트"];

        return true;

    }
    void SetInteractable(bool state)
    {
        Btn_Capture.GetComponent<Button>().interactable = state;
        Btn_GrowSlider.GetComponent<Button>().interactable = state;
        Btn_Detail.GetComponent<Button>().interactable = state;
    }

    void SetARTargetingState(bool state)
    {
        this.arManager.GetComponent<ARButtonManager>().ARTargetingStateChanged(state);
    }
}
