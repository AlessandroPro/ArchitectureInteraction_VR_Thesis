 using System.Collections;
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
    public int numPoints = 100;

    private Vector3 xVector;
    private Vector3 yVector;
    private GameObject[] grabSegments;
    private float handTime;
    private Vector3 connectFromPoint;
    private Vector3 connectToPoint;
    private float grabberTime;
    public GameObject grabHandPrefab;
    private GameObject grabHand;


    // Start is called before the first frame update
    void Start()
    {
        connectFromPoint = transform.position;
        connectToPoint = new Vector3(0, 0, 0);
        grabHand = Instantiate(grabHandPrefab, transform.position, transform.rotation);

        grabSegments = new GameObject[numPoints];
        for (int i = 0; i < grabSegments.Length; i++)
        {
            grabSegments[i] = Instantiate(grabSegmentPrefab);
        }

        handTime = 0;
        grabberTime = 0;
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
                    selected.HandleEnter();
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
            if (grabAction.GetStateDown(handType))
            {
                HideLaser();
                selected.HandleTriggerDown(hitPoint);
                LaunchGrabber();
            }
            else if(grabAction.GetStateUp(handType))
            {
                ShowLaser();
                selected.HandleTriggerUp();
                RetractGrabber();
            }
        }

        UpdateLaser();
        UpdateGrabber();
    }

    private void UpdateGrabber()
    {
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

        }

        grabberTime += Time.deltaTime * 4;
        grabHand.transform.position = Vector3.Lerp(connectFromPoint, connectToPoint, grabberTime);
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
        grabberTime = 0;
        connectToPoint = hitPoint;
        connectFromPoint = transform.position;
    }

    private void RetractGrabber()
    {
        grabberTime = 0;
        connectFromPoint = grabHand.transform.position;
        connectToPoint = transform.position;

    }
}