using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Damageable))]

/* 
RequireComponent的使用：
当你添加的一个用了RequireComponent组件的脚本，需要的组件将会自动被添加到game object（游戏物体）。
可以有效的避免组装错误。
举个例子一个脚本可能需要刚体总是被添加在相同的game object（游戏物体）上。
用RequireComponent属性的话，这个过程将被自动完成，因此你可以永远不会犯组装错误。
*/

public class Actor : MonoBehaviour
{
    protected Rigidbody actorRigidbody;
    protected NavMeshAgent agent;
    protected NavMeshObstacle navMeshObstacle;
    [HideInInspector] public Damageable damageable;
    [HideInInspector] public Damageable damageableTarget;
    [HideInInspector] public Animator animator;
    [HideInInspector] public AnimationEventListener animationEvent;
    [HideInInspector] public Coroutine currentTask;
    [HideInInspector] public ActorVisualHandler visualHandler;

    public bool isHover = false;
    bool isResource;

    private void Awake()
    {
        actorRigidbody = GetComponent<Rigidbody>();
        damageable = GetComponent<Damageable>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        animationEvent = GetComponentInChildren<AnimationEventListener>();
        visualHandler = GetComponent<ActorVisualHandler>();
        animationEvent.attackEvent.AddListener(Attack);
        isResource = GetComponent<Resource>() ? true : false;

        //
        navMeshObstacle = GetComponent<NavMeshObstacle>();
    }
    public void Update()
    {
        animator.SetFloat("Speed", Mathf.Clamp(agent.velocity.magnitude, 0, 1));
    }

    public void SetDestination(Vector3 destination)
    {
        agent.enabled = true;
        agent.destination = destination;
        //agent.enabled = false;
    }
    public WaitUntil WaitForNavMesh()
    {
        return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
        //agent.pathPending: Is a path  in the process of being computed but not yet ready? (Read Only)
        //remainingDistance也就是(距离终点)剩余移动距离，让它跟stopingDistance(结束距离)进行比较,remainingDistance不是立刻更新的,故需要使用前一个条件解决该问题
    }
    void Attack()
    {
        if (damageableTarget)
        {
            damageableTarget.Hit(10);
        }
    }
    public void AttackTarget(Damageable target)
    {
        StopTask();
        damageableTarget = target;

        currentTask = StartCoroutine(StartAttack());

        IEnumerator StartAttack()
        {
            while (damageableTarget)
            {
                SetDestination(damageableTarget.transform.position);

                yield return WaitForNavMesh();

                while (damageableTarget && Vector3.Distance(damageableTarget.transform.position, transform.position) < 4f)
                {
                    yield return new WaitForSeconds(1);
                    if (damageableTarget)
                    {
                        animator.SetTrigger("Attack");
                    }
                }

            }

            currentTask = null;
        }
    }
    public virtual void StopTask()
    {
        damageableTarget = null;
        if (currentTask != null)
        {
            StopCoroutine(currentTask);
        }
    }

    private void OnMouseEnter()
    {
        isHover = true;
    }
    private void OnMouseExit()
    {
        isHover = false;
    }

}
