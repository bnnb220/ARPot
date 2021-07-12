using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ARButtonManager : MonoBehaviour
{

    public Image targetPanel;

    private Camera arCam;

    private List<GameObject> targetObjList = new List<GameObject>();

    private GameObject selectedTarget;

    private Vector2 camCenterPos;

    private Color tpColor;

    private bool arTargetingState = true;

    public event EventHandler SelectOn;
    public event EventHandler SelectOff;
    public event EventHandler TargetLost;

    void Start()
    {
        arCam = Camera.main;
        camCenterPos = new Vector2(Screen.width/2, Screen.height/2);
        tpColor = targetPanel.color;

    }
    void Update()
    {
        TargetSearch();
    }

#region TARGETING
    public void OnVuTargetFound(GameObject newTarget)
    {
        if(!targetObjList.Contains(newTarget))
        {
            targetObjList.Add(newTarget);
        }
    }
    
    public void OnVuTargetLost(GameObject oldTarget)
    {
        if(targetObjList.Contains(oldTarget))
        {
            targetObjList.Remove(oldTarget);
        }

        if(selectedTarget != null)
        {
            if(oldTarget.name == selectedTarget.name)
            {
                TargetLost(this, EventArgs.Empty);  
            }
        }
    }

    public void ARTargetingStateChanged(bool state)
    {
        this.arTargetingState = state;
    }

    void TargetSearch()
    {
        if(arTargetingState)
        {
            GameObject nearestTarget = SearchTargetInArea();
            SelectedTargetUpdate(nearestTarget);
        }
    }

    GameObject SearchTargetInArea()
    {
        GameObject nearestTarget = null;

        float minDist = 987654321;
        foreach(var obj in targetObjList)
        {
            Vector3 objScreenPos = arCam.WorldToScreenPoint(obj.transform.position);
            Vector2 objectXYPos = new Vector2(objScreenPos.x, objScreenPos.y);

            float curDist = Vector2.Distance(camCenterPos, objectXYPos);

            if(minDist > curDist)
            {
                nearestTarget = obj;
                minDist = curDist;
            }
        }
        return nearestTarget;
    }

    void SelectedTargetUpdate(GameObject nearestTarget)
    {
        if(nearestTarget)
        {
            if(!this.selectedTarget)
            {
                this.selectedTarget = nearestTarget;
                SetSelectedTargetColor(this.selectedTarget, true);

                SelectOn(this, EventArgs.Empty);
                GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<FBManager>().LoadData(this.selectedTarget.name);

            }
            //When selection changed
            else if(this.selectedTarget.name != nearestTarget.name)
            {
                SetSelectedTargetColor(this.selectedTarget, false);
                this.selectedTarget = nearestTarget;
                SetSelectedTargetColor(this.selectedTarget, true);

                SelectOn(this, EventArgs.Empty);
                GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<FBManager>().LoadData(this.selectedTarget.name);

            }
        }
        else if(this.selectedTarget)
        {
            SetSelectedTargetColor(this.selectedTarget, false);
            this.selectedTarget = null;
            SelectOff(this, EventArgs.Empty);
        }
    }

    void SetSelectedTargetColor(GameObject obj, bool isSelected)
    {
        if(isSelected)
        {
            obj.transform.Find("Canvas").transform.Find("Panel").GetComponent<Image>().color = new Color(this.tpColor.r, this.tpColor.g, this.tpColor.b, 0.8f);
        }
        else
        {
            obj.transform.Find("Canvas").transform.Find("Panel").GetComponent<Image>().color = new Color(this.tpColor.r, this.tpColor.g, this.tpColor.b, this.tpColor.a);
        }
    }
#endregion


#region GROWTHSLIDER

    public void OnGrowthSliderChanged(GameObject slider)
    {
        if(selectedTarget)
        {
            float value = slider.GetComponent<Slider>().value;
            Image[] images = selectedTarget.transform.Find("Canvas").transform.Find("GrowingImgGroup").GetComponentsInChildren<Image>();
            GrowthImageTransparency(images, value);
        }
    }

    void GrowthImageTransparency(Image[] images, float value)
    {
        Color imgColor;

        if(value == 0)
        {
            imgColor = images[0].color;
            images[0].color = new Color(imgColor.r, imgColor.g, imgColor.b, 0);
            imgColor = images[1].color;
            images[1].color = new Color(imgColor.r, imgColor.g, imgColor.b, 0);
            imgColor = images[2].color;
            images[2].color = new Color(imgColor.r, imgColor.g, imgColor.b, 0);
        }
        if(value > 0 && value <= 1)
        {
            imgColor = images[0].color;
            imgColor = new Color(imgColor.r, imgColor.g, imgColor.b, 1.0f - value);
            images[0].color = imgColor;
        }

        if(value > 0.5 && value <= 1)
        {
            imgColor = images[1].color;
            imgColor = new Color(imgColor.r, imgColor.g, imgColor.b, (value - 0.5f)*2.0f);
            images[1].color = imgColor;
        }

        if(value > 1 && value <= 1.5)
        {
            imgColor = images[1].color;
            imgColor = new Color(imgColor.r, imgColor.g, imgColor.b, 3.0f - (2.0f * value));
            images[1].color = imgColor;
        }

        if(value > 1 && value < 2)
        {
            imgColor = images[2].color;
            imgColor = new Color(imgColor.r, imgColor.g, imgColor.b, value - 1);
            images[2].color = imgColor;
        }
        if(value == 2)
        {
            imgColor = images[0].color;
            images[0].color = new Color(imgColor.r, imgColor.g, imgColor.b, 0);
            imgColor = images[1].color;
            images[1].color = new Color(imgColor.r, imgColor.g, imgColor.b, 0);
            imgColor = images[2].color;
            images[2].color = new Color(imgColor.r, imgColor.g, imgColor.b, 1);
        }
    }
#endregion  
}
