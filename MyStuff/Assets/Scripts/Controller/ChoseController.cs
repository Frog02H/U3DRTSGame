using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChoseController : MonoBehaviour
{
    private bool isMouseDown;

    private LineRenderer line;

    private Vector3 beginDownInputPos;
    private Vector3 endDownInputPos;
    private Vector3 rightUpPos;
    private Vector3 leftDownPos;

    private RaycastHit hitInfo;
    private Vector3 beginWorldPos;

    private List<SoldierObj> soldierObjs = new List<SoldierObj>();

    private Vector3 frontPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        line = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        selSoliderObj();
        ControlSoldierMove();
    }

    private void selSoliderObj()
    {
        if(Input.GetMouseButtonDown(1))
        {
            beginDownInputPos = Input.mousePosition;
            isMouseDown = true;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000 , 1 << LayerMask.NameToLayer("Ground")))
            {
                beginWorldPos = hitInfo.point;
            }

        }
        else if(Input.GetMouseButtonUp(1))
        {
            isMouseDown = false;
            line.positionCount = 0;

            frontPos = Vector3.zero;

            for(int i = 0; i < soldierObjs.Count; i++)
            {
                soldierObjs[i].SetSelSelf(false);
            }
            soldierObjs.Clear();

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000 , 1 << LayerMask.NameToLayer("Ground")))
            {
                Vector3 center = new Vector3((hitInfo.point.x + beginWorldPos.x) / 2, 1, (hitInfo.point.z + beginWorldPos.z) / 2);
                Vector3 half = new Vector3(Mathf.Abs(hitInfo.point.x - beginWorldPos.x) / 2, 1, Mathf.Abs(hitInfo.point.z - beginWorldPos.z) / 2);

                Collider[] colliders = Physics.OverlapBox(center, half);

                for(int i = 0; i < colliders.Length; i++)
                {
                    SoldierObj obj = colliders[i].GetComponent<SoldierObj>();
                    
                    if(obj != null)
                    {
                        obj.SetSelSelf(true);
                        soldierObjs.Add(obj);
                    }
                }

            }

        }

        if(isMouseDown)
        {
            endDownInputPos = Input.mousePosition;

            rightUpPos.x = endDownInputPos.x;
            rightUpPos.y = beginDownInputPos.y;
            rightUpPos.z = 5;

            leftDownPos.x = beginDownInputPos.x;
            leftDownPos.y = endDownInputPos.y;
            leftDownPos.z = 5;

            beginDownInputPos.z = 5;
            endDownInputPos.z = 5;

            line.positionCount = 4;
            line.SetPosition(0, Camera.main.ScreenToWorldPoint(beginDownInputPos));
            line.SetPosition(1, Camera.main.ScreenToWorldPoint(rightUpPos));
            line.SetPosition(2, Camera.main.ScreenToWorldPoint(endDownInputPos));
            line.SetPosition(3, Camera.main.ScreenToWorldPoint(leftDownPos));

        }
    }

    private void ControlSoldierMove()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(soldierObjs.Count == 0)
            {
                return;
            }

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000, 1 << LayerMask.NameToLayer("Ground")))
            {
                for(int i = 0; i < soldierObjs.Count; i++)
                {
                    soldierObjs[i].Move(hitInfo.point);
                }
            }
        }
    }
/*
    private List<Vector3> GetTargetPos(Vector3 targetPos)
    {
        Vector3 nowForward = Vector3.zero;
        Vector3 nowRigth = Vector3.zero;

        if(frontPos != Vector3.zero)
        {
            nowForward = (targetPos - frontPos).normalized;
        }
        else
        {
            nowForward = (targetPos - soldierObjs[0].transform.position).normalized;
        }
        nowRigth = Quaternion.Euler(0, 90, 0) * nowForward;
        
        frontPos = targetPos;
    }
*/    
}
