using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test: MonoBehaviour{
    //public Transform sphere;

    void Update(){
        Vector3 dir = transform.forward;  //正方体指向球体的向量dir = 球体坐标 - 正方体坐标
        Quaternion ang = Quaternion.LookRotation(dir);  //创建一个 使正方体朝向球体的旋转角
        transform.rotation = ang;  //使正方体朝向球体

        Debug.DrawRay(transform.position, transform.forward * 100, Color.blue);  //绘制正方体forward
        Debug.DrawRay(transform.position, dir, Color.green);  //绘制向量dir
        Debug.DrawRay(transform.position, ang.eulerAngles, Color.red);
    }
}


    /* RaycastHit hitInfo;
        Vector3 capsuleColliderCenterInWorldSpace=GetComponent<CapsuleCollider> ().transform.TransformPoint (GetComponent<CapsuleCollider>().center);
        bool isHit=Physics.Raycast (capsuleColliderCenterInWorldSpace,new Vector3(0f,-1f,0f),out hitInfo,100f,LayerMask.GetMask("floor"));

        Vector3 forward=GetComponent<Rigidbody>().transform.forward;
       
        Vector3 newUp;
        if (isHit) {
            newUp = hitInfo.normal;
        } else {
            newUp = Vector3.up;
        }

　　　　　//limit lean angle (if do not need to limit lean angle, remove the code block below)

　　　　　{

　　　　　　　const float maxDegreeFromWorldUp = 45.0f;//lean angle limited to 45 degree
            //clamp newUp to let is not exceed 45 degree from WorldUp(Vector3.up)
            newUp=Vector3.RotateTowards (Vector3.up, newUp, maxDegreeFromWorldUp / 180.0f * Mathf.PI, 0.0f);

　　　　　}
        Vector3 left = Vector3.Cross (forward,newUp);//note: unity use left-hand system, and Vector3.Cross obey left-hand rule.
        Vector3 newForward = Vector3.Cross (newUp,left);
        Quaternion oldRotation=GetComponent<Rigidbody>().transform.rotation;
        Quaternion newRotation = Quaternion.LookRotation (newForward, newUp);

　　　　　float kSoftness=0.1f;//if do not want softness, change the value to 1.0f
        GetComponent<Rigidbody> ().MoveRotation (Quaternion.Lerp(oldRotation,newRotation,kSoftness)); */