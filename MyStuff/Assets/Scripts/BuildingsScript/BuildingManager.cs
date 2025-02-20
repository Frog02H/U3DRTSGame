using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType { Wood, Stone }
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;
    //List<Building> allBuildings = new List<Building>();
    public List<Building> allBuildings = new List<Building>();
    public Building[] buildingPrefabs = default;
    public int[] currentResources = default;

    [SerializeField] private ParticleSystem buildParticle;
    [SerializeField] private ParticleSystem finishParticle;
    BuildingUI ui;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentResources = new int[] { 999, 999, 999, 999, 999, 999};
        ui = FindObjectOfType<BuildingUI>();
        if (ui)
        {
            ui.RefreshResources();
        }
    }

    private void Update()
    {
        foreach (Actor actor in ActorManager.instance.selectedActors)
        {
            if (actor is Builder)
            {
                Builder builder = actor as Builder;
                Debug.DrawLine(builder.transform.position, builder.hitInfoPoint, Color.green);
                Debug.DrawLine(builder.hitInfoPoint, builder.hitInfoPoint + Vector3.up, Color.red);
                //Debug.DrawRay(builder.transform.position, builder.hitInfoPoint * 1000, Color.blue);
                //OnDrawGizmos();
            }
        }
    }

    public void SpawnBuilding(int index, Vector3 position)
    {
        Building building = buildingPrefabs[index];
        if (!building.CanBuild(currentResources))
        {
            return;
        }

        // Create Building,重新new一个（此处做法）或从对象池里拿取一个（此处没写）
        building = Instantiate(buildingPrefabs[index], position, Quaternion.identity);
        allBuildings.Add(building);

        /*
        StartCoroutine(BuildFor5Seconds());

        IEnumerator BuildFor5Seconds()
        {
            while (!building.IsFinished())
            {
                yield return new WaitForSeconds(1);
                if (!building.IsFinished())
                {
                    animator.SetTrigger("Attack");
                    building.Build(10);
                }
            }
        }
        */

        //设置一个监听器，监听是否RemoveBuilding(building)
        //!building.attackable.onDestroy.AddListener(() => RemoveBuilding(building));!
        
        
        // Give builders build task
        foreach (Actor actor in ActorManager.instance.selectedActors)
        {
            if (actor is Builder)
            {
                Builder builder = actor as Builder;
                if (!builder.HasTask())
                {
                    //builder.CheckJobPosition(building);
                    builder.CheckJobPositionWithBoxCollider(building);
                    builder.GiveJob(building);
                }
            }
        }
        
        // Subtract resources
        int[] cost = building.Cost();
        /*检测
        Debug.Log("cost.Length:"+cost.Length);
        */
        for (int i = 0; i < cost.Length; i++)
        {
            currentResources[i] -= cost[i];
            if (ui)
            {
                ui.RefreshResources();
            }
        }

        //
    }

    public List<Building> GetBuildings()
    {
        return allBuildings;
    }
    public Building GetPrefab(int index)
    {
        return buildingPrefabs[index];
    }

    public Building GetRandomBuilding()
    {
        if (allBuildings.Count > 0)
        {
            return allBuildings[Random.Range(0, allBuildings.Count)];
        }
        else
        {
            return null;
        }
    }
    public void RemoveBuilding(Building building)
    {
        allBuildings.Remove(building);
    }
    public void AddResource(ResourceType resourceType, int amount)
    {
        currentResources[(int)resourceType] += amount;

        if(ui)
        {
            ui.RefreshResources();
        }
    }
    public void PlayParticle(Vector3 position)
    {
        if (buildParticle)
        {
            buildParticle.transform.position = position;
            buildParticle.Play();
        }
    }
    
    
    /*    
    public void OnDrawGizmos()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.localPosition, hitInfoPoint);
        Gizmos.DrawCube(hitInfoPoint, new Vector3(0.1f, 0.1f, 0.1f));
    } 
    */
    
}
