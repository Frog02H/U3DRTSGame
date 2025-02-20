using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Actor))]
public class BuildingColliderCatchController : MonoBehaviour
{
/*     void Update()
    {
        if(currentBuilding.IsFinished())
            {
                //老方法agent.isStopped = true;
                //老方法agent.ResetPath();
                //最不推荐agent.enabled = false;
                //agent.SetDestination(agent.transform.position);
                //StopCoroutine(currentTask);
                yield return agent.SetDestination(agent.transform.position);
                StopCoroutine(currentTask);
                //yield break;
            }

    } */

    private void OnTriggerEnter(Collider Collider)
    {
        Debug.Log(Collider.gameObject.name + "进入了Collider!");
        /*
        NavMeshAgent agent = Collider.gameObject.GetComponent<NavMeshAgent>();
        NavMeshObstacle navMeshObstacle = Collider.gameObject.GetComponent<NavMeshObstacle>();
        agent.enabled = false;
        navMeshObstacle.enabled = true;
        navMeshObstacle.carving = true; 
        */
    }

    private void OnTriggerStay(Collider Collider)
    {
        //NavMeshAgent agent = Collider.gameObject.GetComponent<NavMeshAgent>();
        //NavMeshObstacle navMeshObstacle = Collider.gameObject.GetComponent<NavMeshObstacle>();
        Debug.Log(Collider.gameObject.name + "还在Collider里!");
        if(!Collider.gameObject.GetComponent<Builder>().HasBuilding)
        {
            NavMeshAgent agent = Collider.gameObject.GetComponent<NavMeshAgent>();
            NavMeshObstacle navMeshObstacle = Collider.gameObject.GetComponent<NavMeshObstacle>();
            agent.enabled = true;
            navMeshObstacle.enabled = false;
            navMeshObstacle.carving = false;
        }
        //agent.enabled = false;
        //navMeshObstacle.enabled = true;
        //navMeshObstacle.carving = true;
    }

    private void OnTriggerExit(Collider Collider)
    {
        NavMeshAgent agent = Collider.gameObject.GetComponent<NavMeshAgent>();
        NavMeshObstacle navMeshObstacle = Collider.gameObject.GetComponent<NavMeshObstacle>();
        Debug.Log(Collider.gameObject.name + "从Collider里出来了！");
        agent.enabled = true;
        navMeshObstacle.enabled = false;
        navMeshObstacle.carving = false;
    }
//    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
//
    
}
