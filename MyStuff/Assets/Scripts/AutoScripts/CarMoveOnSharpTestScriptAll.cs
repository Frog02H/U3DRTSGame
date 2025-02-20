using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class CarMoveOnSharpTestScriptAll : MonoBehaviour
{

    public Vector3 SharpNormal;
    public Vector3 SharpForward;
    public Vector3 SharpRight;
    //public Vector3 TankChildForward;
    //public Vector3 TankChildRight;
    public Vector3 TankParentForward;
    public Vector3 TankParentRight;
    public Vector3 TankParentUp;
    public Vector3 cross;

    public bool Change = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        project();

    }

    private bool IsSlope()
    {
        float slopeHeightMaxDistance = 2f;
        float heightOffset = 1f;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * heightOffset, Vector3.down, out hit, slopeHeightMaxDistance, LayerMask.GetMask("Ground")))
        //if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeHeightMaxDistance, LayerMask.GetMask("Ground")))
        {
            SharpNormal = hit.normal;
            return SharpNormal != Vector3.up;
        }

        return false;
    }


    private void project()
    {

        TankParentForward = transform.parent.forward;
        TankParentRight = transform.parent.right;
        TankParentUp = transform.parent.up;
        
        if(IsSlope())
        {
            //TankChildForward = transform.forward;
            //TankChildRight = transform.right;

            SharpForward = Vector3.ProjectOnPlane(TankParentForward, SharpNormal);
            SharpRight = Vector3.ProjectOnPlane(TankParentRight, SharpNormal);

            //Vector3.Angle(transform.parent.right, SharpRight);

            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(SharpForward) , 0.01f);
            
            
            //cross = Vector3.Cross(TankParentForward, TankChildForward);

            /*
            cross = Vector3.Cross(TankParentForward, TankChildForward);
            if (cross.y < 0)
            {
                Debug.Log("Left");
                //Quaternion AngleAxisChangeRotation = Quaternion.AngleAxis(Vector3.Angle(TankParentRight, SharpRight), TankParentForward);
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(SharpForward) * AngleAxisChangeRotation, 0.01f);
            }
            else
            {
                Debug.Log("right");
                //Quaternion AngleAxisChangeRotation = Quaternion.AngleAxis(Vector3.Angle(TankParentRight, SharpRight), TankParentForward);
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(SharpForward) * AngleAxisChangeRotation, 0.01f);
            } 
            */
            
            Quaternion AngleAxisChangeRotation = Quaternion.AngleAxis(Vector3.Angle(TankParentRight, SharpRight) , SharpForward);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(SharpForward) * AngleAxisChangeRotation, 0.01f);

            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(SharpForward) , 0.01f);
                        
            //transform.rotation = Quaternion.AngleAxis(Vector3.Angle(transform.parent.right, SharpRight), transform.parent.forward);
            //Debug.Log("Vector3.Angle(SharpRight, transform.parent.right):"+ Vector3.Angle(SharpRight, TankParentRight));
            //Debug.Log("transform.parent.right:"+ transform.parent.right);
            //Debug.Log("transform.right:"+ transform.right);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(SharpRight) , 0.01f);
            //transform.DORotate(SharpForward, 0.01f);

            Change = true;
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, transform.parent.rotation, 0.01f);

            //TankChildForward = Vector3.zero;
            SharpForward = Vector3.zero;
            SharpRight = Vector3.zero;
            SharpNormal = Vector3.zero;

        }

        Debug.DrawRay(transform.position, TankParentForward * 100, Color.red);  //绘制TankObj的forward
        Debug.DrawRay(transform.position, TankParentRight * 100, Color.blue);  //绘制TankObj的forward
        Debug.DrawRay(transform.position, SharpForward * 5, Color.blue);  //绘制向量TankObj斜坡时的forward
        Debug.DrawRay(transform.position, SharpRight * 5, Color.red);  //绘制向量TankObj斜坡时的Right
        Debug.DrawRay(transform.position, SharpNormal * 5, Color.green);  //绘制向量斜坡法线

        Change = false;
    }

    //判断玩家是否处于上下坡
/*  
    public void DownRoad()
    {
        // 从玩家位置发射射线朝下检测斜坡
        Ray slopeRay = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
 
        if (Physics.Raycast(slopeRay, out hit))
        {
 
            if (hit.collider.name.Equals("Road 16"))
            {
                // 检测到碰撞物体
                Vector3 slopeNormal = hit.normal;
                float slopeAngle = Vector3.Angle(slopeNormal, Vector3.forward);
                float f = Mathf.Abs(slopeAngle - 90);
                Debug.Log(slopeAngle);
                // 如果夹角大于0度则为下坡
                if (slopeAngle - 90 > 0)
                {
                    transform.rotation = Quaternion.Euler(90-slopeAngle, 0, 0);
                    if (slopeAngle - 90 > 25)
                    {
                        transform.rotation = Quaternion.Euler(-25, 0, 0);
                    }
                }
                else if (slopeAngle - 90<0)
                {
                    transform.rotation = Quaternion.Euler(90-slopeAngle , 0, 0);
                    if (slopeAngle - 90 < -25)
                    {
                        transform.rotation = Quaternion.Euler(25, 0, 0);
                    }
                }
                else if (f<3.0f && f>0.0f)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }  
        }
    }
 */
 }
