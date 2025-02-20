using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CostPannelUI : MonoBehaviour
{
    //public Transform resourceGroup;
    public CanvasGroup ResourceCanvasGroup;
    public Transform BuildingTextImage;

    private Building building;

    CanvasGroup[] ChildrencanvasGroup;
    
    // Start is called before the first frame update
    void Start()
    {
        ChildrencanvasGroup = ResourceCanvasGroup.GetComponentsInChildren<CanvasGroup>();

        /*检测
        Debug.Log("ChildrencanvasGroup.Length:"+ChildrencanvasGroup.Length);
        */
        for (int i = 0; i < ChildrencanvasGroup.Length; i++)
        {
            int index = i;

            /*检测
            Debug.Log("ChildrencanvasGroup["+ index + "]:"+ChildrencanvasGroup[index].name);
            */
        }
        
        /*
        BuildingTextImage.GetComponentInChildren<TextMeshProUGUI>().text = building.buildingName;

        //Button[] Buildingbuttons = ChildrencanvasGroup[0].GetComponentsInChildren<Button>();

        for (int i = 0; i < ChildrencanvasGroup.Length; i++)
        {
            int index = i;

            CanvasGroup CostCanvasGroup = ChildrencanvasGroup[index].GetComponentInChildren<CanvasGroup>();
            
            //CostCanvasGroup.GetComponentInChildren<TextMeshProUGUI>().text = GetBuildingCostText(b, index);
            CostCanvasGroup.GetComponentInChildren<TextMeshProUGUI>().text = building.resourceCost[index].ToString();
        }
        */ 
    }

    // Update is called once per frame
    void Update()
    {
        ChangeCostInfo();
    }

    public void ShowCostPannelUI(Building b)
    {
        this.building = b;
        //this.gameObject.SetActive(true);
        SetActiveWithCheck(this.gameObject, true);

    }

    string GetBuildingCostText(Building b, int index)
    {
        string buildingName = b.buildingName;
        //int resourceAmount = b.resourceCost.Length;
        //string[] resourceNames = new string[] { "Food", "Wood", "Steel", "Oil", "Gold", "Manpower" };
        //string resourceString = string.Empty;
        //for (int j = 0; j < resourceAmount; j++)
            //resourceString += "\n " + resourceNames[j] + " (" + b.resourceCost[j] + ")";

        //return "<size=23><b>" + buildingName + "</b></size>" + resourceString;
        return b.resourceCost[index].ToString();
    }

    static void SetActiveWithCheck(GameObject go, bool state) 
    {
        if (go == null)
        {
            return;
        }

        if (go.activeSelf != state)
        {
            go.SetActive(state);
        }
    }

    private void ChangeCostInfo() 
    {
        BuildingTextImage.GetComponentInChildren<TextMeshProUGUI>().text = building.buildingName;

        //Button[] Buildingbuttons = ChildrencanvasGroup[0].GetComponentsInChildren<Button>();
        //ChildrencanvasGroup带有ResourceCanvasGroup，即子物体的父物体.GetComponentsInChildren<>()会拿取调用该方法的对象.
        //ChildrencanvasGroup.Length = 7
        for (int i = 1; i < ChildrencanvasGroup.Length; i++)
        {
            int index = i;

            Transform CostResourceTransform = ChildrencanvasGroup[index].GetComponentInChildren<Transform>();
            
            //CostCanvasGroup.GetComponentInChildren<TextMeshProUGUI>().text = GetBuildingCostText(b, index);
            CostResourceTransform.GetComponentInChildren<TextMeshProUGUI>().text = building.resourceCost[index -1].ToString();
        }
    }

}
