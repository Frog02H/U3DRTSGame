using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//[RequireComponent(typeof(Damageable))]
public class Building : MonoBehaviour
{
    public string buildingName;
    [SerializeField] float height;
    public float radius = 5;
    float originalHeight;
    [SerializeField] int totalWorkToComplete = 100;
    //int currentWork;
    public int currentWork;
    public int[] resourceCost = default;
    Transform buildingTransform;
    [HideInInspector] public Damageable attackable;
    public bool isHover = false;
    private bool done;
    [ColorUsage(true, true)]
    [SerializeField] private Color[] stateColors;
    MeshRenderer buildingRender;
    //Cinemachine.CinemachineImpulseSource impulse;
    public BoxCollider rayCastBoxCollider;
    private void Awake()
    {
        attackable = GetComponent<Damageable>();
    }

    void Start()
    {
        buildingTransform = transform.GetChild(0);
        buildingRender = buildingTransform.GetComponent<MeshRenderer>();
        //impulse = GetComponent<Cinemachine.CinemachineImpulseSource>();
        currentWork = 0;
        originalHeight = buildingTransform.localPosition.y;
        buildingTransform.localPosition = Vector3.down * height;
        rayCastBoxCollider = GetComponent<BoxCollider>();
        /*检测
        Debug.Log("height:"+height);
        Debug.Log("originalHeight:"+originalHeight);
        Debug.Log("currentWork:"+currentWork);
        */
    }
    
    private void Update()
    {
        
    }
    
    public void Build(int work)
    {
        currentWork += work;
        buildingTransform.localPosition = Vector3.Lerp(Vector3.down * height, new Vector3(0,originalHeight,0), (float)currentWork / totalWorkToComplete);
        /*检测
        Debug.Log("Building的currentWork:"+currentWork);
        */
        //visual
        buildingTransform.DOComplete();
        buildingTransform.DOShakeScale(.5f, .2f, 10, 90, true);
        //BuildingManager.instance.PlayParticle(transform.position);
    }

    public bool IsFinished()
    {
        if (currentWork >= totalWorkToComplete && !done && buildingRender)
        {
            done = true;
            buildingRender.material.DOColor(stateColors[1], "_EmissionColor", .1f).OnComplete(() => buildingRender.material.DOColor(stateColors[0], "_EmissionColor", .5f));
            /*
            if (impulse)
            {
                impulse.GenerateImpulse();
            }
            */
        }
        return currentWork >= totalWorkToComplete;
    }

    public bool CanBuild(int[] resources)
    {
        bool canBuild = true;
        /*检测
        Debug.Log("resourceCost.Length:"+resourceCost.Length);
        */
        for (int i = 0; i < resourceCost.Length; i++)
        {
            if (resources[i] < resourceCost[i])
            {
                canBuild = false;
                break;
            }
        }
        return canBuild;
    }
    
    public void IsChangeLayer()
    {
        if(gameObject.layer == LayerMask.NameToLayer("FinBuilding"))
        {
            return;
            //如果层级已经更改则跳过此函数
        }

        if (IsFinished())
        {
            int layer = LayerMask.NameToLayer("FinBuilding");
            gameObject.layer = layer;
        }
        
    }

    public int[] Cost()
    {
        return resourceCost;
    }

    private void OnMouseEnter()
    {
        isHover = true;
    }
    private void OnMouseExit()
    {
        isHover = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
