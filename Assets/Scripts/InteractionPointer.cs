﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class InteractionPointer : MonoBehaviour
{

    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;

    public LineRenderer laserLine;
    public Vector3 hitPoint;
    public LayerMask interactableMask;
    public Interactable selected;

    public GameObject grabSegmentPrefab;
    private int numPoints = 40;

    private Vector3 xVector;
    private Vector3 yVector;
    private GameObject[] grabSegments;
    private float handTime;
    private Vector3 connectFromPoint;
    private Vector3 connectToPoint;

    public GameObject grabHandPrefab;
    private GameObject grabHand;
    private Transform grabHandle;
    private float grabberRate;
    private float grabberFrac;


    // Start is called before the first frame update
    void Start()
    {
        connectFromPoint = transform.position;
        connectToPoint = transform.position;
        grabHand = Instantiate(grabHandPrefab, transform.position, transform.rotation);

        grabberFrac = 0;
        grabberRate = 4;

        grabSegments = new GameObject[numPoints];
        for (int i = 0; i < grabSegments.Length; i++)
        {
            grabSegments[i] = Instantiate(grabSegmentPrefab);
        }

        handTime = 0;
        hitPoint = controllerPose.transform.position + (transform.forward * 100);

        ShowLaser();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(laserLine.enabled)
        {
            if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, Mathf.Infinity, interactableMask))
            {
                hitPoint = hit.point;
                Interactable newSelected = hit.collider.gameObject.GetComponent<Interactable>();
                if (!(selected == newSelected))
                {
                    if (selected)
                    {
                        selected.HandleExit();
                    }
                    selected = newSelected;
                    selected.HandleEnter(controllerPose);
                }
            }
            else
            {
                hitPoint = controllerPose.transform.position + (transform.forward * 100);
                if (selected)
                {
                    selected.HandleExit();
                }
                selected = null;
            }
        }

        if (selected)
        {
            selected.HandleStay(hitPoint);

            if (grabAction.GetStateDown(handType))
            {
                HideLaser();
                selected.HandleTriggerDown(hitPoint);
                grabHandle = selected.GetGrabHandle(hitPoint);
                LaunchGrabber();
            }
            else if(grabAction.GetState(handType))
            {
                selected.HandleTriggerHold();
            }
            else if(grabAction.GetStateUp(handType))
            {
                ShowLaser();
                selected.HandleTriggerUp();
                grabHandle = null;
                RetractGrabber();
            }
        }

        UpdateLaser();
        UpdateGrabber();
    }

    private void UpdateGrabber()
    {
        /*
        xVector = grabHand.transform.position - transform.position;

        float distance = xVector.magnitude;
        Vector3 hypVector = transform.forward;
        float angle = Vector3.Angle(xVector, hypVector);
        if(angle > 50f)
        {
            hypVector = Vector3.RotateTowards(transform.forward, xVector, (angle * Mathf.Deg2Rad) - (50f * Mathf.Deg2Rad), 0.0f);
            hypVector.Normalize();
            angle = Vector3.Angle(xVector, hypVector);
        }
        float hyp = distance / Mathf.Cos(angle * Mathf.Deg2Rad);


        yVector = (hypVector * hyp) - xVector;

        xVector.Normalize();
        yVector.Normalize();

        float Vo = Mathf.Sqrt((distance * 9.8f) / Mathf.Sin(2 * angle * Mathf.Deg2Rad));
        float Vx = Vo * Mathf.Cos(angle * Mathf.Deg2Rad);
        float Vy = Vo * Mathf.Sin(angle * Mathf.Deg2Rad);

        float timeTotal = 2 * Vo * Mathf.Sin(angle * Mathf.Deg2Rad) / 9.8f;
        float interval = timeTotal / grabSegments.Length;
        for (int i = 0; i < grabSegments.Length; i++)
        {
            float time = interval * i;

            Vector3 xPos = xVector * Vx * time;
            Vector3 yPos = yVector * (Vy * time - 0.5f * 9.8f * time * time);

            grabSegments[i].transform.position = transform.position + xPos + yPos;

        }*/


        ///// Bezier curve /////

        Vector3 cp1 = transform.position;
        Vector3 cp3 = grabHand.transform.position;

        xVector = cp3 - cp1;
        Vector3 hypVector = transform.forward;
        float angle = Vector3.Angle(xVector, hypVector);

        if(angle >= 45f)
        {
            angle = 45f;
            hypVector = Vector3.RotateTowards(xVector, transform.forward, Mathf.Deg2Rad * angle, 0);
            hypVector.Normalize();
        }

        float x = xVector.magnitude / 2f;
        float z = x / Mathf.Cos(Mathf.Deg2Rad * angle);

        Vector3 cp2 = cp1 + (z * hypVector);

        float interval = 1f / grabSegments.Length;
        float t = 0;

        Vector3 b12, b23;
        for (int i = 0; i < grabSegments.Length; i++)
        {
            b12 = Vector3.Lerp(cp1, cp2, t);
            b23 = Vector3.Lerp(cp2, cp3, t);

            grabSegments[i].transform.position = Vector3.Lerp(b12, b23, t);
            t += interval;
        }



        if (grabHandle)
        {
            connectToPoint = grabHandle.transform.position;
        }
        else
        {
            connectFromPoint = transform.position;
        }

        grabberFrac += Time.deltaTime * grabberRate;
        grabHand.transform.position = Vector3.Lerp(connectFromPoint, connectToPoint, grabberFrac);
    }

    private void ShowLaser()
    {
        laserLine.enabled = true;

        laserLine.SetPosition(0, transform.position);
        laserLine.SetPosition(1, hitPoint);
    }

    private void HideLaser()
    {
        laserLine.enabled = false;
    }

    private void UpdateLaser()
    {
        laserLine.SetPosition(0, transform.position);
        laserLine.SetPosition(1, hitPoint);
    }

    private void LaunchGrabber()
    {
        connectToPoint = grabHandle.position;
        connectToPoint = hitPoint;
        connectFromPoint = transform.position;

        grabberFrac = 0;
        if(grabberRate < 0)
        {
            grabberRate *= -1;
        }
    }

    private void RetractGrabber()
    {
        connectToPoint = grabHand.transform.position;
        connectFromPoint = transform.position;

        grabberFrac = 1;
        if (grabberRate > 0)
        {
            grabberRate *= -1;
        }
    }
}