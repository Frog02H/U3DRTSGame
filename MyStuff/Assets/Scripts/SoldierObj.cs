using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierObj : MonoBehaviour
{
    //动画的切换
    private Animator animator;
    //移动方式
    private NavMeshAgent agent;
    //设置是否处于选中状态
    private GameObject ChosenEffect;


    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponentInChildren<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        ChosenEffect = this.transform.Find("Cube").gameObject;

        SetSelSelf(false);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsMove", agent.velocity.magnitude > 0);
    }

    public void Move(Vector3 pos)
    {
        agent.SetDestination(pos);
    }
    
    public void SetSelSelf(bool isSel)
    {
        ChosenEffect.SetActive(isSel);
    }
}
