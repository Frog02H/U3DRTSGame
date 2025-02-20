using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTargetTestScript : MonoBehaviour
{
    //方向向量目标点 看向点
    public Transform target,looker;
    //两点确定法线方向
    //public Transform n1, n2;
    public Vector3 LookerNormal;
    //跟随点
    public Transform sign;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        project();
    }

    void project()
    {
        //looker.transform.LookAt(target);

        //指向目标的向量
        var dir = target.position - transform.position;
        //平行于X轴平面的法向量 Y轴正方向向量 （1,0,0）
        LookerNormal = transform.up;
        var normal = LookerNormal;
        //投影向量
        var pj = Vector3.ProjectOnPlane(dir, normal);
        //相对本对象位置进行变化
        sign.position = transform.position + pj;

        //绘制方向
        Debug.DrawLine(transform.position, target.position);
        //绘制投影
        Debug.DrawLine(transform.position, sign.position, Color.blue);
        //绘制法线
        Debug.DrawLine(target.position, sign.position,Color.red);
    }

}

