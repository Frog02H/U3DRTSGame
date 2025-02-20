using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderBack : Actor
{
    Building currentBuilding;
    public Vector3 hitInfoPoint;
    public CapsuleCollider BuilderCollider;
    public bool IsHit = false;
    public bool HasBuilding = false;
    private void Start()
    {
        BuilderCollider = GetComponent<CapsuleCollider>();
        BuilderCollider.enabled = true;
        animationEvent.attackEvent.AddListener(DoWork);
        /*检测
        Debug.Log("DoWork已经向attackEvent插入监听器。");
        */
        //BuilderCollider.enabled = true;
    }

    /*
    void Update()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, hitInfoPoint);
        //Gizmos.DrawLine(hitInfoPoint, hitInfoPoint + Vector3.up);
        //Debug.DrawLine(transform.position, hitInfoPoint, Color.green);
        //Debug.DrawLine(hitInfoPoint, hitInfoPoint + Vector3.up, Color.red);
        //OnDrawGizmos();
    }
    */

    /*
    public void OnDrawGizmos()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.localPosition, hitInfoPoint);
        Gizmos.DrawCube(hitInfoPoint, new Vector3(0.1f, 0.1f, 0.1f));
    }
    */
    public void CheckJobPosition(Building job)
    {
        Vector3 jobPosition = job.transform.position;
        /*测试
        BuilderCollider.enabled = false;
        agent.enabled = false;
        */
        
        //
        Ray ray = new(transform.position, jobPosition);
        RaycastHit hitInfo;

        LayerMask layerMask = 1 << LayerMask.NameToLayer("UnFinBuilding");
            
            Debug.Log("↓↓↓↓↓↓-" + BuilderCollider.gameObject.name + "的Raycast的if前-↓↓↓↓↓↓");
            if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity ,layerMask.value))
            {
                jobPosition.x = hitInfo.point.x;
                jobPosition.z = hitInfo.point.z;
                Debug.Log("碰撞点的建筑为：" + hitInfo.collider.name);
                Debug.Log(BuilderCollider.gameObject.name + "的hitInfo已经更新。");
                hitInfoPoint = jobPosition;
                transform.LookAt(job.transform);
                SetDestination(jobPosition);
                IsHit = true;
            }
            else
            {
                Debug.Log(BuilderCollider.gameObject.name + "的Raycast没有碰撞啊！");
                IsHit = false;
            }
            Debug.Log("↑↑↑↑↑↑-" + BuilderCollider.gameObject.name + "的Raycast的if后-↑↑↑↑↑↑");
        
            /*
            agent.enabled = true;
            BuilderCollider.enabled = true;
            */

            //SetDestination(jobPosition);
    }

    public void CheckJobPositionWithBoxCollider(Building job)
    {
        Vector3 jobPosition = job.transform.position;
        HasBuilding = true;
        /*测试
        BuilderCollider.enabled = false;
        agent.enabled = false;
        */
        
        /*
        Ray ray = new(transform.position, jobPosition);
        RaycastHit hitInfo;

        LayerMask layerMask = 1 << LayerMask.NameToLayer("UnFinBuilding");
            
            Debug.Log("↓↓↓↓↓↓-" + BuilderCollider.gameObject.name + "的Raycast的if前-↓↓↓↓↓↓");
            if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity ,layerMask.value))
            {
                jobPosition.x = hitInfo.point.x;
                jobPosition.z = hitInfo.point.z;
                Debug.Log("碰撞点的建筑为：" + hitInfo.collider.name);
                Debug.Log(BuilderCollider.gameObject.name + "的hitInfo已经更新。");
                hitInfoPoint = jobPosition;
                transform.LookAt(job.transform);
                SetDestination(jobPosition);
                IsHit = true;
            }
            else
            {
                Debug.Log(BuilderCollider.gameObject.name + "的Raycast没有碰撞啊！");
                IsHit = false;
            }
            Debug.Log("↑↑↑↑↑↑-" + BuilderCollider.gameObject.name + "的Raycast的if后-↑↑↑↑↑↑");
            
            /*
            agent.enabled = true;
            BuilderCollider.enabled = true;
            */

            transform.LookAt(jobPosition);

            SetDestination(jobPosition);
    }

    public void GiveJob(Building job)
    {
        currentBuilding = job;

        if (currentTask != null)
        {
            StopCoroutine(currentTask);
        }

        //BuilderCollider.enabled = false;
        currentTask = StartCoroutine(StartJob());
        //BuilderCollider.enabled = true;

        /*
        if(!IsHit)
        {
            StopCoroutine(currentTask);
            SetDestination(currentBuilding.transform.position);
            CheckJobPosition(job);
            currentTask = StartCoroutine(StartJob());
            //IsHit = true;
        }
        */

        IEnumerator StartJob()
        {
            //////////////////////////////////////////////////////////////////////////  
            /*
            //Vector3 jobPosition = job.transform.position;
            Vector3 jobPosition = job.transform.position;
            
            ////
            Vector2 randomPosition = Random.insideUnitCircle.normalized * currentBuilding.radius;
            jobPosition.x += randomPosition.x;
            jobPosition.z += randomPosition.y;
            SetDestination(jobPosition);
            ////

            //测试
            BuilderCollider.enabled = false;
            agent.enabled = false;
            //

            //Ray ray = new Ray(transform.position, job.transform.position);
            Ray ray = new(transform.position, jobPosition);
            RaycastHit hitInfo;
            //int layerMask = LayerMask.GetMask("UnFinBuilding");
            //int layerMask = 1 << LayerMask.NameToLayer("UnFinBuilding");
            //int layerMask_Use = 1 << LayerMask.NameToLayer("UnFinBuilding");
            //int layerMask_NoUse = 0 << LayerMask.NameToLayer("Player");
            //LayerMask layerMask = layerMask_Use | layerMask_NoUse;
            //LayerMask layerMask = (1 << 9) | (0 << 11);
            //LayerMask layerMask = (1 << LayerMask.NameToLayer("UnFinBuilding")) | (0 << LayerMask.NameToLayer("Player"));
            LayerMask layerMask = 1 << LayerMask.NameToLayer("UnFinBuilding");
            
            ////
            while(!IsHit)
            {
            ////

                Debug.Log("↓↓↓↓↓↓-" + BuilderCollider.gameObject.name + "的Raycast的if前-↓↓↓↓↓↓");
                if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity ,layerMask.value))
                {
                    //Vector2 WorkerPosition = hitInfo.point;
                    jobPosition.x = hitInfo.point.x;
                    jobPosition.z = hitInfo.point.z;
                    //jobPosition.y = job.transform.position.y;
                    //Debug.DrawLine(transform.position,hitInfo.point,Color.red);
                    Debug.Log("碰撞点的建筑为：" + hitInfo.collider.name);
                    Debug.Log(BuilderCollider.gameObject.name + "的hitInfo已经更新。");
                    hitInfoPoint = jobPosition;
                    //agent.enabled = true;
                    transform.LookAt(currentBuilding.transform);
                    SetDestination(jobPosition);
                    IsHit = true;
                }
                else
                {
                    Debug.Log(BuilderCollider.gameObject.name + "的Raycast没有碰撞啊！");
                    IsHit = false;
                }
                Debug.Log("↑↑↑↑↑↑-" + BuilderCollider.gameObject.name + "的Raycast的if后-↑↑↑↑↑↑"); 
            ////    
            }
            ////
            //测试
            agent.enabled = true;
            BuilderCollider.enabled = true;
            //
            */
            //////////////////////////////////////////////////////////////////////////   
            yield return WaitForNavMesh();

            //transform.LookAt(currentBuilding.transform);
            //我们自己添加的transform.LookAt(currentBuilding.transform);

            //测试
            //BuilderCollider.enabled = true;
            //

            while (!currentBuilding.IsFinished())
            {
                yield return new WaitForSeconds(1);
                if (!currentBuilding.IsFinished())
                {
                    animator.SetTrigger("Attack");
                    //我们自己添加的transform.LookAt(currentBuilding.transform);
                }
            }

            //
            //currentBuilding.IsChangeLayer();
            job.IsChangeLayer();
            HasBuilding = false;
            //
            //IsHit = false;
            //
            currentBuilding = null;
            currentTask = null;
        }
    }
    public bool HasTask()
    {
        return currentTask != null;
    }
    override public void StopTask()
    {
        base.StopTask();
        currentBuilding = null;
    }

    void DoWork()
    {
        /*检测
        Debug.Log("currentBuilding:"+ currentBuilding.name);
        */
        if (currentBuilding)
        {
            currentBuilding.Build(10);
            /*检测
            Debug.Log("After" + BuilderCollider.gameObject.name + "'s currentBuilding.Build(10)!!!!!!");
            */
        }
    }
}
