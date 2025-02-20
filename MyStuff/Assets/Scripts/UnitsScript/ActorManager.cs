using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class ActorManager : MonoBehaviour
{
    public bool BoxOrLine = default;
//Box的选择框
    public static ActorManager instance;
    [SerializeField] LayerMask actorLayer = default;
    [SerializeField] Transform selectionArea = default;
    public List<Actor> allActors = new List<Actor>();
    //[SerializeField] List<Actor> selectedActors = new List<Actor>();
    public List<Actor> selectedActors = new List<Actor>();
    Camera mainCamera;
    Vector3 startDrag;
    Vector3 endDrag;
    Vector3 dragCenter;
    Vector3 dragSize;
    bool dragging;

//Line的选择框
    public bool isMouseDown;

    public LineRenderer line;
    public Vector3 beginDownInputPos;
    public Vector3 endDownInputPos;
    public Vector3 rightUpPos;
    public Vector3 leftDownPos;

    public RaycastHit hitInfo;
    public Vector3 TempPos;
    public Vector3 beginWorldPos;
//

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        mainCamera = Camera.main;
        foreach (Actor actor in GetComponentsInChildren<Actor>())
        {
            allActors.Add(actor);
        }
        BoxOrLine = false;
        selectionArea.gameObject.SetActive(false);
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            dragging = false;
            return;
        }

        if(BoxOrLine)//0为box，1为line
        {
            SelectUpdateWithLine();
        }
        else
        {
            SelectUpdate();
        }
    }

    void SelectUpdate()
    {
        selActorObj();
        //自添加
        ControlSoldierMove();
    }

    void selActorObj()
    {
        if (Input.GetMouseButtonDown(1))
        {
            startDrag = Utilities.MouseToTerrainPosition();
            endDrag = startDrag;
        }
        else if (Input.GetMouseButton(1))
        {
            endDrag = Utilities.MouseToTerrainPosition();

            if (Vector3.Distance(startDrag, endDrag) > 1)
            {
                selectionArea.gameObject.SetActive(true);
                dragging = true;
                dragCenter = (startDrag + endDrag) / 2;
                dragSize = (endDrag - startDrag);
                selectionArea.transform.position = dragCenter;
                selectionArea.transform.localScale = dragSize + Vector3.up;
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if (dragging)
            {
                SelectActors();
                //dragging = false;
                selectionArea.gameObject.SetActive(false);
            }
            dragging = false;
            /*
            else
            {
                SetTask();
            }
            */
        }
    }

    void SetTask()
    {
        if (selectedActors.Count == 0)
        {
            return;
        }
        Collider collider = Utilities.CameraRay().collider;
        //if (collider.CompareTag("Terrain"))
        if (collider.CompareTag("Ground"))
        {
            foreach (Actor actor in selectedActors)
            {
                actor.SetDestination(Utilities.MouseToTerrainPosition());
            }
        }
        else if (!collider.CompareTag("Player"))
        {
            if (collider.TryGetComponent(out Damageable damageable))
            {
                foreach (Actor actor in selectedActors)
                {
                    actor.AttackTarget(damageable);
                }
            }
        }


    }

    void SelectActors()
    {
        DeselectActors();
        dragSize.Set(Mathf.Abs(dragSize.x / 2), 1, Mathf.Abs(dragSize.z / 2));
        RaycastHit[] hits = Physics.BoxCastAll(dragCenter, dragSize, Vector3.up, Quaternion.identity, 0, actorLayer.value);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out Actor actor))
            {
                selectedActors.Add(actor);
                actor.visualHandler.Select();
            }
        }
    }
    public void DeselectActors()
    {
        foreach (Actor actor in selectedActors)
        {
            actor.visualHandler.Deselect();
        }
        selectedActors.Clear();
    }

    private void OnDrawGizmos()
    {
        Vector3 center = (startDrag + endDrag) / 2;
        Vector3 size = (endDrag - startDrag);
        size.y = 1;
        Gizmos.DrawWireCube(center, size);
    }

    void SelectUpdateWithLine()
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

            beginWorldPos = Utilities.MouseToTerrainPosition();
            /* 
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000 , 1 << LayerMask.NameToLayer("Ground")))
            {
                beginWorldPos = hitInfo.point;
            } 
            */

        }
        else if(Input.GetMouseButtonUp(1))
        {
            isMouseDown = false;
            line.positionCount = 0;

            //frontPos = Vector3.zero;
            
            DeselectActors();
            //执行actor.visualHandler.Deselect();
            //执行selectedActors.Clear();

            TempPos = Utilities.MouseToTerrainPosition();
            //在这调整高度
            Vector3 center = new Vector3((TempPos.x + beginWorldPos.x) / 2, 1, (TempPos.z + beginWorldPos.z) / 2);
            Vector3 half = new Vector3(Mathf.Abs(TempPos.x - beginWorldPos.x) / 2, 1, Mathf.Abs(TempPos.z - beginWorldPos.z) / 2);

            SelectActorsWithLine(center, half);
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

        /*             
        if(selectedActors.Count == 0)
            {
                return;
            } 
        */

            SetTask();
        }
    }

    private void SelectActorsWithLine(Vector3 center, Vector3 half)
    {
        Collider[] colliders = Physics.OverlapBox(center, half);

                for(int i = 0; i < colliders.Length; i++)
                {
                    Actor obj = colliders[i].GetComponent<Actor>();
                    
                    if(obj != null)
                    {
                        selectedActors.Add(obj);
                        obj.visualHandler.Select();
                    }
                }                
    }
}
