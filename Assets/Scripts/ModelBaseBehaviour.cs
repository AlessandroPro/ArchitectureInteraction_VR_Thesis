using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class ModelBaseBehaviour : Interactable
{
    public GameObject slider;
    private Quaternion initialRotation;
    private Rigidbody baseBody;
    private Vector3 relativeControllerPos;

    public GameObject ghostHand;
    private GameObject[] spokes;
    public GameObject spokePrefab;

    // Start is called before the first frame update
    void Start()
    {
        baseBody = GetComponent<Rigidbody>();
        relativeControllerPos = Vector3.zero;
        grabbed = false;
        spokes = new GameObject[16];

        float spokeAngle = 360 / spokes.Length;
        for(int i = 0; i < spokes.Length; i++)
        {
            spokes[i] = Instantiate(spokePrefab, transform);
            spokes[i].transform.RotateAround(transform.position, Vector3.up, i * spokeAngle);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    override public void HandleEnter(SteamVR_Behaviour_Pose pose)
    {
        controllerPose = pose;
    }

    override public void HandleExit()
    {
        //Debug.Log("EXITTTTTTTTTT");
    }

    public override void HandleStay(Vector3 hitPoint)
    {
        if (!grabbed)
        {
            Vector3 centerToHit = hitPoint - transform.position;
            centerToHit = new Vector3(centerToHit.x, 0, centerToHit.z);

            float angle = Vector3.SignedAngle(centerToHit, transform.forward, Vector3.up);

            if (angle < 0)
            {
                angle = 360f + angle;
            }
            angle = 360 - angle;
            Debug.Log(angle);

            float spokeAngleInterval = 360f / spokes.Length;
            float spokeIndexFloat = angle / spokeAngleInterval;
            int spokeIndex = Mathf.RoundToInt(spokeIndexFloat);

            if (spokeIndex >= spokes.Length)
            {
                spokeIndex = 0;
            }

            ghostHand.transform.position = spokes[spokeIndex].transform.position;
        }
    }

    override public void ShowHighlight() { }

    override public void HideHighlight() { }

    override public void HandleButtonClickDown() { }

    override public void HandleButtonClickHold() { }

    override public void HandleButtonClickUp() { }

    override public void HandleTriggerDown(Vector3 hitPoint)
    {
        baseBody.angularVelocity = Vector3.zero;
        slider.transform.position = controllerPose.transform.position;
        Vector3 lookAtPos = new Vector3(transform.position.x, slider.transform.position.y, transform.position.z);
        slider.transform.LookAt(lookAtPos);
        slider.SetActive(true);
        initialRotation = transform.rotation;
        grabbed = true;
    }

    override public void HandleTriggerHold()
    {
        if(grabbed)
        {
            //Get the position of the controller relative to the slider
            relativeControllerPos = slider.transform.InverseTransformPoint(controllerPose.transform.position);

            Quaternion target = Quaternion.Euler(0, -relativeControllerPos.x * 20f, 0) * initialRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5f);
        }
        
    }

    override public void HandleTriggerUp()
    {
        if (grabbed)
        {
            Vector3 relativeControllerVelocity = slider.transform.InverseTransformDirection(controllerPose.GetVelocity());
            baseBody.angularVelocity = new Vector3(0, -relativeControllerVelocity.x * 2, 0);
            slider.SetActive(false);
            grabbed = false;
        }
    }

    override public Transform GetGrabHandle(Vector3 hitPoint)
    {
        return ghostHand.transform;
    }

    /*
    private void OnDrawGizmos()
    {
        Vector3 relativeControllerVelocity = slider.transform.InverseTransformDirection(controllerPose.GetVelocity());
        //Debug.Log(relativeControllerVelocity);
        //Debug.Log(controllerPose.GetVelocity());
        Vector3 actualVel = controllerPose.transform.TransformDirection(controllerPose.GetVelocity());
        Gizmos.DrawRay(slider.transform.position, relativeControllerVelocity * 5);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(slider.transform.position, controllerPose.GetVelocity() * 5);
    }*/

    /*

    override public void HandleEnter()
    {
        Debug.Log("WORKS");
    }

    override public void HandleExit()
    {
        Debug.Log("WORKS");
    }

    override public void ShowHighlight() { }

    override public void HideHighlight() { }

    override public void HandleButtonClickDown() { }

    override public void HandleButtonClickHold() { }

    override public void HandleButtonClickUp() { }

    override public void HandleTriggerDown() { }

    override public void HandleTriggerHold() { }

    override public void HandleTriggerUp() { }
    */
}
