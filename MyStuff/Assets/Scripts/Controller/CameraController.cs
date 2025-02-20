using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform followTransform;
    public Transform cameraTransform;
    public float normalSpeed;
    public float fastSpeed;

    public float movementSpeed;
    public float movementTime;
    public float rotaionAmount;
    public Vector3 zoomAmount;

    Vector3 newPosition;
    Quaternion newRotation;
    Vector3 newZoom;

    Vector3 dragStartPosition;
    Vector3 dragCurrentPosition;
    Vector3 rotateStartPosition;
    Vector3 rotateCurrentPosition;    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(followTransform != null)
        {
            transform.position = followTransform.position;
        }

            HandleMovementInput();
            HandleMouseInput();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            followTransform = null;
        }

    }

    void HandleMouseInput()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount * 25f;
            //因为zoomAmount太小，所以加了个*25f
        }

        if(Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftControl))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }

        if(Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftControl))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);
                
                Vector3 dragPosition = dragStartPosition - dragCurrentPosition;

                if(Input.GetKey(KeyCode.LeftShift))
                {
                    dragPosition *= 4f;
                }

                newPosition = transform.position + dragPosition;
    
            }
        }

        if(Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            rotateStartPosition = Input.mousePosition;
        }

        if(Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;
            
            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }

    void HandleMovementInput()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }

        if(Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if(Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotaionAmount);
            //返回一个旋转，它围绕 z 轴旋转 z 度、围绕 x 轴旋转 x 度、围绕 y 轴旋转 y 度（按该顺序应用）。
        }

        if(Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotaionAmount);
        }

        if(Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }

        if(Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }

        if(Input.GetKey(KeyCode.I))
        {
            if(Quaternion.Angle(transform.rotation, newRotation) <= 30f && Quaternion.Angle(transform.rotation, newRotation) >= -40f)
            {
                newRotation *= Quaternion.Euler(Vector3.right * rotaionAmount);
            }
            else
            {
                newRotation = transform.rotation;
            }
        }

        if(Input.GetKey(KeyCode.K))
        {
            if(Quaternion.Angle(transform.rotation, newRotation) <= 30f && Quaternion.Angle(transform.rotation, newRotation) >= -40f)
            {
                newRotation *= Quaternion.Euler(Vector3.right * -rotaionAmount);
            }
            else
            {
                newRotation = transform.rotation;
            }
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
