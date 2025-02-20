using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveTest : MonoBehaviour
{
/*     Vector3 lineStart;
    Vector3 lineEnd;
    float timer; 
    public float Delta; // 定时2秒
    public float ZD;
    float speed = 1.0f; */

    //public Transform startMarker;
    //public Transform endMarker;
    Vector3 startPosition;
    Vector3 endPosition;
    public float speed;
    private float startTime;
    public float journeyLength;

    private bool IsChange {get;set;}

    void Start() 
    {
        startTime = Time.time;
        startPosition = transform.position;
        endPosition = transform.position;
        endPosition.z += journeyLength;
    }

    void Update() 
    {
        
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startPosition, endPosition, fracJourney);
        
        if(endPosition.z == transform.position.z)
        {
            IsChange = true;
        }

        if(IsChange)
        {
            endPosition = startPosition;
            startPosition = transform.position;
            startTime = Time.time;
            IsChange = false;
        }

    }

    public void OnMouseDown()
    {
        CameraController.instance.followTransform = transform;
    }

    /* // Start is called before the first frame update
    void Start()
    {
        timer = Delta;
        lineStart = this.transform.position;
        lineEnd = this.transform.position;
        lineEnd.z *= ZD;
    }

    // Update is called once per frame
    void Update()
    {
            //transform.position += moveDirection * ZD * Time.deltaTime;
            transform.position = Vector3.Lerp(lineStart, lineEnd, (Time.time - Delta) * speed / ZD);

            timer -= Time.deltaTime;
            if (timer <= 0) 
            {
                DoSqureMove();
                
                timer = Delta;
        } 

    }

    private void DoSqureMove()
    {
        
        if(lineStart.z <= lineEnd.z)
        {
            lineStart = lineEnd;
            lineEnd.z /= ZD;            
        }
        else
        {
            lineStart = lineEnd;
            lineEnd.z *= ZD;
        }
    } */
}
