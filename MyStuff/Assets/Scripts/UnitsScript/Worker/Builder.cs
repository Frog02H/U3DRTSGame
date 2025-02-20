using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Actor
{
    Building currentBuilding;
    public Vector3 hitInfoPoint;
    public CapsuleCollider BuilderCollider;
    public bool IsHit = false;
    public bool HasBuilding = false;
    //public CatchedByColliderController catchedByColliderController;
    private void Start()
    {
        BuilderCollider = GetComponent<CapsuleCollider>();
        BuilderCollider.enabled = true;
        animationEvent.attackEvent.AddListener(DoWork);
        /*检测
        Debug.Log("DoWork已经向attackEvent插入监听器。");
        */
        //catchedByColliderController.currentBuilding = currentBuilding;
    }

    public void CheckJobPosition(Building job)
    {
        Vector3 jobPosition = job.transform.position;
        
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
    
    }

    public void CheckJobPositionWithBoxCollider(Building job)
    {
        Vector3 jobPosition = job.transform.position;

        //
        HasBuilding = true;
        //catchedByColliderController.currentBuilding = job;
        //

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

        currentTask = StartCoroutine(StartJob());

        IEnumerator StartJob()
        {
            //catchedByColliderController.currentBuilding = currentBuilding;

            yield return WaitForNavMesh();

            //ChangeUnitNavMeshSettingsFT();

            while (!currentBuilding.IsFinished())
            {
                yield return new WaitForSeconds(1);
                if (!currentBuilding.IsFinished())
                {
                    animator.SetTrigger("Attack");
                    //我们自己添加的transform.LookAt(currentBuilding.transform);
                }
            }
            
            //currentBuilding.IsChangeLayer();
            job.IsChangeLayer();
            HasBuilding = false;
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

    public void ChangeUnitNavMeshSettingsFT()
    {
        agent.enabled = false;
        navMeshObstacle.enabled = true;
        navMeshObstacle.carving = true;
        //agent.isStopped = true;
        //agent.ResetPath();
    }

    public void ChangeUnitNavMeshSettingsTF()
    {
        agent.enabled = true;
        navMeshObstacle.enabled = false;
        navMeshObstacle.carving = false;
    }
}
