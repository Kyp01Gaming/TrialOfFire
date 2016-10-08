﻿using UnityEngine;
                    
public class CameraController : MonoBehaviour {
    public enum Direction
    {
        Forward, Back, Left, Right
    }
    //Direction the Camera faces at start
    public Direction direction = Direction.Forward;
    //References to Transforms
    public Transform pivotTranform;
    public Transform cameraTranform;
    public Transform playerTranfrom;
    private Transform ownTranform;
    //Angle offsets
    public float maxOffsetAngle = 50.0f;
    public float minOffsetAngle = 35.0f;
    //Offsets
    public float offsetXZ = 7.5f; //Distance from Player
    public float offsetY = 2.0f; //Hight from ground
    public float offsetA = 45.0f; //Angle the camera watches
    //Speed
    public float rotationSpeed = 2.0f;
    //Position References
    private Vector3 maxOffsetPos;
    private Vector3 minOffsetPos;

    public bool invertY = false;
    public bool invertX = false;
                
    void Start() 
    {
        //Set References
        ownTranform = transform;
        
        if (playerTranfrom == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null) {
                playerTranfrom = obj.transform;
            }
        }

        //Error Handeling/Checking
        string message = "";
        if (playerTranfrom == null)
        {
            message += "Transform \'playerTransform\' missing";
        }
        if (pivotTranform == null)
        {
            if (message != "")
            {
                message += ", ";
            }
            message += "Transform \'pivotTranform\' missing";
        }
        if (cameraTranform == null)
        {
            if (message != "")
            {
                message += ", ";
            }
            message += "Transform \'cameraTranform\' missing";
        }
        if (ownTranform == null)
        {
            if (message != "")
            {
                message += ", ";
            }
            message += "Transform \'ownTranform\' missing";
        }  
        if (message != "")
        {
            GameManager.inst.ErrorSystem(message, this, true, 0);
        }

        float angle = 0.0f;
        Vector3 dir = Vector3.forward;
        switch(direction)
        {
            case Direction.Forward:
                angle = Vector3.Angle(Vector3.back, -playerTranfrom.forward);
                dir = -ownTranform.forward;
                break;
            case Direction.Back:
                angle = Vector3.Angle(Vector3.forward, playerTranfrom.forward);
                dir = ownTranform.forward;
                break;
            case Direction.Right:
                angle = Vector3.Angle(Vector3.left, -playerTranfrom.right);
                dir = -ownTranform.right;
                break;
            case Direction.Left:
                angle = Vector3.Angle(Vector3.right, playerTranfrom.right);
                dir = ownTranform.right;
                break;
        }
        ownTranform.position = playerTranfrom.position;
        ownTranform.Rotate(Vector3.up, angle);
        pivotTranform.localPosition = new Vector3(0.0f, offsetY, 0.0f);
        cameraTranform.localPosition = dir * offsetXZ;
        maxOffsetPos = minOffsetPos = cameraTranform.position;
        pivotTranform.Rotate(ownTranform.right, offsetA);
        cameraTranform.LookAt(pivotTranform);
    }

    void Update()
    {
        //Input
        float hori = -Input.GetAxis("Mouse X");
        float vert = Input.GetAxis("Mouse Y");

        //maxOffsetPos = Quaternion.AngleAxis(maxOffsetAngle, pivotTranform.right) * maxOffsetPos;
        //minOffsetPos = Quaternion.AngleAxis(minOffsetAngle, pivotTranform.right) * minOffsetPos;

        //Offset
        ownTranform.position = playerTranfrom.position;

        //Rotations
        //if (Vector3.Angle(cameraTranform.position, maxOffsetPos) > 0.05f && Vector3.Angle(cameraTranform.position, minOffsetPos) > 0.05f)
        //{
            pivotTranform.Rotate(ownTranform.right, vert * rotationSpeed * Time.deltaTime);
        //}
        ownTranform.Rotate(Vector3.up, hori * rotationSpeed * Time.deltaTime);                                                      
        cameraTranform.LookAt(pivotTranform);
    }

    /*  Draws stuff in the editor.
     *  Yellow = Direction
     *  Megenta = OffsetY
     *  Cyan = OffsetA + OffsetXZ
     *  Blue = max/min Offset Angle + OffsetXZ
     */
    void OnDrawGizmosSelected()
    {
        //Direction
        Gizmos.color = Color.yellow;
        Vector3 camDir = Vector3.zero;
        Vector3 rightDir = Vector3.zero;
        switch(direction)
        {
            case Direction.Forward:
                camDir = Vector3.forward;
                rightDir = Vector3.right;
                break;
            case Direction.Back:
                camDir = Vector3.back;
                rightDir = Vector3.left;
                break;
            case Direction.Right:
                camDir = Vector3.right;
                rightDir = Vector3.back;
                break;
            case Direction.Left:
                camDir = Vector3.left;
                rightDir = Vector3.forward;
                break;
        }                
        Gizmos.DrawLine(transform.position, transform.position + camDir * offsetXZ);

        //OffsetY   
        Vector3 offY = transform.position + Vector3.up * offsetY;     
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + offY);

        //Base for Angles                    
        Vector3 based = -camDir * offsetXZ;
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(offY, offsetXZ);

        //OffsetA                                     
        Vector3 offA = Quaternion.AngleAxis(offsetA, rightDir) * based;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(offY, offA + Vector3.up * offsetY);

        //Max/Min Offset
        Vector3 max = Quaternion.AngleAxis(maxOffsetAngle, rightDir) * based;
        Vector3 min = Quaternion.AngleAxis(-minOffsetAngle, rightDir) * based;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(offY, max + Vector3.up * offsetY);
        Gizmos.DrawLine(offY, min + Vector3.up * offsetY);
    } 
}
