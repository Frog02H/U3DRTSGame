using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerSwitchTheirTools : MonoBehaviour
{
/*     
    public bool IsMine = default;
    //挖矿，镐子
    public bool IsLumber = default;
    //伐木，斧头
    public bool IsSow = default;
    //播种，锄头
    public bool IsDig = default;
    //挖矿，镐子
*/

    enum WhichToolInHand
    {
        axe = 0,
        pickaxe = 1,
        hoe = 2,
        shovel = 3
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeTool(WhichToolInHand whichTool)
    {
        switch(whichTool)
        {
            case WhichToolInHand.axe: SetActiveWithCheck(this.transform.GetChild(0).gameObject, true); break;
            case WhichToolInHand.pickaxe: SetActiveWithCheck(this.transform.GetChild(1).gameObject, true); break;
            case WhichToolInHand.hoe: SetActiveWithCheck(this.transform.GetChild(2).gameObject, true); break;
            case WhichToolInHand.shovel: SetActiveWithCheck(this.transform.GetChild(3).gameObject, true); break;
            default: SetActiveWithCheck(this.transform.GetChild(0).gameObject, true); break;
        }
    }

    void SetActiveWithCheck(GameObject gameObject, bool state)
    {
        if(gameObject == null)
        {
            return ;
        }

        if(gameObject.activeSelf != state)
        {
            gameObject.SetActive(state);
        }
    }
}
