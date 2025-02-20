using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    CanvasGroup ParentcanvasGroup;
    CanvasGroup ChildcanvasGroup;
    bool isPlacing = false;
    int currentIndex = 0;

    public Transform resourceGroup;

    public CanvasGroup CostcanvasGroup;

    Mesh buildingPreviewMesh;
    [SerializeField] Material buildingPreviewMat;
    private void Awake()
    {
        ParentcanvasGroup = GetComponent<CanvasGroup>();
    }
    void Start()
    {
        CanvasGroup[] ChildrencanvasGroup = GetComponentsInChildren<CanvasGroup>();
        //Debug.Log("ChildrencanvasGroup[0]:"+ ChildrencanvasGroup[0]);
        
        Button[] Buildingbuttons = ChildrencanvasGroup[0].GetComponentsInChildren<Button>();
        for (int i = 0; i < Buildingbuttons.Length; i++)
        {
            int index = i;
            /*检测
            Debug.Log("index:"+ index);
            */
            Buildingbuttons[index].onClick.AddListener(() => SelectBuilding(index));

            //Building b = BuildingManager.instance.buildingPrefabs[index];
            //Buildingbuttons[index].GetComponentInChildren<TextMeshProUGUI>().text = GetButtonText(b);
        }
/*        
        Button[] Actionbuttons = ChildrencanvasGroup[1].GetComponentsInChildren<Button>();
        for (int i = 0; i < Actionbuttons.Length; i++)
        {
            int index = i;
            //Actionbuttons[index].onClick.AddListener(() => SelectBuilding(index));

            //Building b = BuildingManager.instance.buildingPrefabs[index];
            //Actionbuttons[index].GetComponentInChildren<TextMeshProUGUI>().text = GetButtonText(b);
        }
*/        
    }

    private void Update()
    {
        if (isPlacing)
        {
            Vector3 position = Utilities.MouseToTerrainPosition();

            Graphics.DrawMesh(buildingPreviewMesh, position, Quaternion.identity, buildingPreviewMat, 0);
            
            //if (Input.GetMouseButtonDown(0))
            if (Input.GetKey(KeyCode.G))
            {
                /*检测
                Debug.Log("在Update中的currentIndex:"+ currentIndex);
                Debug.Log("在Update中的position:"+ position);
                */
                isPlacing = false;
                BuildingManager.instance.SpawnBuilding(currentIndex, position);
                ParentcanvasGroup.alpha = 1;
                //isPlacing = false;
            }
        }

        
    }

    void SelectBuilding(int index)
    {
        //CanvasGroup CostcanvasGroup = GetComponentInParent<CanvasGroup>();
        //SetActive(CostcanvasGroup.gameObject, true);
        //CostPannelUI调用！显示消耗的资源
        Building b = BuildingManager.instance.buildingPrefabs[index];
        CostcanvasGroup.GetComponent<CostPannelUI>().ShowCostPannelUI(b);
        

        currentIndex = index;
        /*检测
        Debug.Log("在SelectBuilding中的currentIndex:"+ currentIndex);
        */
        //ActorManager.instance.DeselectActors();
        ParentcanvasGroup.alpha = 0;
        isPlacing = true;
        buildingPreviewMesh = BuildingManager.instance.GetPrefab(index).GetComponentInChildren<MeshFilter>().sharedMesh;
    }

    string GetButtonText(Building b)
    {
        string buildingName = b.buildingName;
        int resourceAmount = b.resourceCost.Length;
        string[] resourceNames = new string[] { "Food", "Wood", "Steel", "Oil", "Gold", "Manpower" };
        string resourceString = string.Empty;
        for (int j = 0; j < resourceAmount; j++)
            resourceString += "\n " + resourceNames[j] + " (" + b.resourceCost[j] + ")";

        return "<size=23><b>" + buildingName + "</b></size>" + resourceString;
    }

    public void RefreshResources()
    {
        for (int i = 0; i < resourceGroup.childCount; i++)
        {
            resourceGroup.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = BuildingManager.instance.currentResources[i].ToString();
        }
    }
}
