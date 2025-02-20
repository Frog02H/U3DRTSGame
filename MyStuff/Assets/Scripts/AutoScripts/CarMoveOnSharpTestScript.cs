using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMoveOnSharpTestScript : MonoBehaviour
{

    public Vector3 SharpNormal;
    public Vector3 SharpForward;
    public Vector3 TankObjForward;
    public Vector3 TankOnFlatForward;
    public Vector3 TankForward;

    public bool Change = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        project();

/*         RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);
        } */
    }

    private bool IsSlope()
    {
        float slopeHeightMaxDistance = 2f;
        float heightOffset = 1f;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * heightOffset, Vector3.down, out hit, slopeHeightMaxDistance, LayerMask.GetMask("Ground")))
        //if (Physics.Raycast(transform.position + transform.forward * heightOffset, Vector3.down, out hit, slopeHeightMaxDistance))
        {
            SharpNormal = hit.normal;
            return SharpNormal != Vector3.up;

            //Quaternion NextRot = Quaternion.LookRotation(Vector3.Cross(hit.normal, Vector3.Cross(transform.forward,hit.normal)), hit.normal);

            //transform.MoveRotation(Quaternion.Lerp(transform.rotation, NextRot, 0.1f));
        }

        return false;
    }


    private void project()
    {
        TankForward = transform.forward;
/*         if(TankOnFlatForward == Vector3.zero)
        {
            TankOnFlatForward = transform.forward;
        }   */     
        
        TankOnFlatForward = transform.forward;
        
        if(IsSlope())
        {
            TankObjForward = transform.forward;
            //TankOnFlatForward = TankObjForward;
            SharpForward = Vector3.ProjectOnPlane(TankObjForward, SharpNormal);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(SharpForward) , 0.01f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(SharpForward), 0.01f);
            Change = false;
        }
        else
        {
            //TankOnFlatForward = transform.forward;
            Change = false;
            //if(TankObjForward != Vector3.zero && TankOnFlatForward.y != 0f)
            if(TankOnFlatForward.y != 0f || TankObjForward != Vector3.zero)
            //if(TankOnFlatForward != Vector3.zero || TankObjForward != Vector3.zero)
            {
                TankOnFlatForward = Vector3.ProjectOnPlane(TankOnFlatForward, Vector3.up);
                TankOnFlatForward.y = 0f;
                //transform.rotation = Quaternion.LookRotation(TankOnFlatForward);
                //transform ParentTransform = GetComponentInParent<transform>();
                //transform.rotation = ParentTransform.rotation;
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(TankOnFlatForward, Vector3.up)) , 0.01f);
                //Quaternion.LookRotation(TankOnFlatForward).eulerAngles;
                //Quaternion.Euler(yourAngles);
                //!transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(TankOnFlatForward) , 0.01f);
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(TankOnFlatForward), 0.01f);
                
                transform.rotation = Quaternion.Lerp(transform.rotation, transform.parent.rotation , 0.01f);
                //transform.Rotate(Vector3.up  * -transform.rotation.y);
                //transform.localEulerAngles = TankOnFlatForward;
                //transform.rotation.y.set(0f);
                TankObjForward = Vector3.zero;
                SharpForward = Vector3.zero;
                SharpNormal = Vector3.zero;
                
                //TankOnFlatForward.y = 0f;
                Change = true;
                //transform.rotation = Quaternion.LookRotation(TankOnFlatForward);
            }

        }

/*         if(transform.rotation.y != 0f)
        {
            transform.rotation = transform.parent.rotation;
        } */

        Debug.DrawRay(transform.position, TankObjForward * 100, Color.blue);  //绘制Tank的forward
        Debug.DrawRay(transform.position, SharpForward, Color.green);  //绘制向量dir
        Debug.DrawRay(transform.position, Quaternion.LookRotation(SharpForward).eulerAngles, Color.red);

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
