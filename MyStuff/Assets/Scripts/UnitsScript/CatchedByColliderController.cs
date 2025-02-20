using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Actor))]
public class CatchedByColliderController : MonoBehaviour
{
    NavMeshAgent agent;
    Building currentBuilding;
    Builder builder;

    
    // Start is called before the first frame update
    void Start()
    {
        builder = GetComponent<Builder>();
        agent = builder.GetComponent<NavMeshAgent>();
        //agent = GetComponent<NavMeshAgent>();
        currentBuilding = builder.GetComponent<Building>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBuilding.IsFinished())
            {
                //老方法
                //agent.isStopped = true;
                //老方法
                //agent.ResetPath();
                //最不推荐agent.enabled = false;
                agent.SetDestination(agent.transform.position);
                StopCoroutine(builder.currentTask);
                //yield break;
            }
    }
    
}
