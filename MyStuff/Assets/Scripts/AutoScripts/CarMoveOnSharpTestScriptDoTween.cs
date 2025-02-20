using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CarMoveOnSharpTestScriptDoTween : MonoBehaviour
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

    }

    private bool IsSlope()
    {
        float slopeHeightMaxDistance = 2f;
        float heightOffset = 1f;

        RaycastHit hit;

        //if (Physics.Raycast(transform.position + transform.forward * heightOffset, Vector3.down, out hit, slopeHeightMaxDistance, LayerMask.GetMask("Ground")))
        if (Physics.Raycast(transform.position + transform.forward * heightOffset, Vector3.down, out hit, slopeHeightMaxDistance))
        //if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeHeightMaxDistance))
        {
            SharpNormal = hit.normal;
            return SharpNormal != Vector3.up;
        }

        return false;
    }


    private void project()
    {
        TankForward = transform.forward;
        
        TankOnFlatForward = transform.forward;
        
        if(IsSlope())
        {
            TankObjForward = transform.forward;
            SharpForward = Vector3.ProjectOnPlane(TankObjForward, SharpNormal);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(SharpForward) , 0.01f);
            //transform.DORotate(SharpForward, 0.01f, RotateMode.Fast);
            transform.DORotateQuaternion(Quaternion.LookRotation(SharpForward),0.01f);
            Change = false;
        }
        else
        {
            Change = false;

            if(TankOnFlatForward.y != 0f || TankObjForward != Vector3.zero)
            {
                TankOnFlatForward = Vector3.ProjectOnPlane(TankOnFlatForward, Vector3.up);
                TankOnFlatForward.y = 0f;
                
                
                //transform.rotation = Quaternion.Lerp(transform.rotation, transform.parent.rotation , 0.01f);
                //transform.DORotate(TankOnFlatForward, 0.01f, RotateMode.Fast);
                transform.DORotateQuaternion(Quaternion.LookRotation(TankOnFlatForward),0.01f);

                TankObjForward = Vector3.zero;
                SharpForward = Vector3.zero;
                SharpNormal = Vector3.zero;
                
                Change = true;
            }

        }

        Debug.DrawRay(transform.position, TankForward * 100, Color.blue);  //绘制Tank的forward
        Debug.DrawRay(transform.position, SharpForward, Color.green);  //绘制向量dir
        Debug.DrawRay(transform.position, SharpNormal, Color.red);
        //Debug.DrawRay(transform.position, Quaternion.LookRotation(SharpForward).eulerAngles, Color.red);

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
