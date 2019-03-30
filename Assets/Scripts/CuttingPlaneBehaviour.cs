using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class CuttingPlaneBehaviour : Interactable
{

    private float initialHeight;
    public GameObject buttonPair;

    public GameObject joint;
    public GameObject cpFixture;
    public Rigidbody cpBody;
    private Vector3 relativeControllerPos;
    void Start()
    {
        cpBody = cpFixture.GetComponent<Rigidbody>();
        relativeControllerPos = Vector3.zero;
        ghostHand.SetActive(false);
        grabbed = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    override public void HandleEnter(SteamVR_Behaviour_Pose pose)
    {
        controllerPose = pose;
        ghostHand.SetActive(true);
    }

    override public void HandleExit()
    {
        slider.SetActive(false);
        ghostHand.SetActive(false);
        //HideButtons();
    }

    public override void HandleStay(Vector3 hitPoint)
    {
        if (!grabbed)
        {
            Vector3 jointPos = joint.transform.position;

            Vector3 offset = new Vector3(0, -0.15f, -0.15f);
            ghostHand.transform.position = jointPos + offset;
            ghostHand.transform.LookAt(new Vector3(jointPos.x, ghostHand.transform.position.y, jointPos.z));
            ghostHand.transform.LookAt(ghostHand.transform.position + Vector3.up);
        }
    }

    override public void ShowHighlight() { }

    override public void HideHighlight() { }

    override public void HandleButtonClickDown() { }

    override public void HandleButtonClickHold() { }

    override public void HandleButtonClickUp() { }

    override public void HandleTriggerDown(Vector3 hitPoint)
    {
        cpBody.velocity = Vector3.zero;
        slider.transform.position = controllerPose.transform.position;
        Vector3 lookAtPos = new Vector3(cpFixture.transform.position.x, slider.transform.position.y, cpFixture.transform.position.z);
        slider.transform.LookAt(lookAtPos);
        slider.SetActive(true);
        initialHeight = cpFixture.transform.position.y;
        grabbed = true;
        //HideButtons();
        ShowButtons();
    }

    override public void HandleTriggerHold()
    {
        if (grabbed)
        {
            //Get the position of the controller relative to the slider
            relativeControllerPos = slider.transform.InverseTransformPoint(controllerPose.transform.position);

            cpFixture.transform.position = new Vector3(cpFixture.transform.position.x, initialHeight + relativeControllerPos.y, cpFixture.transform.position.z);
        }
    }

    override public void HandleTriggerUp()
    {
        if (grabbed)
        {
            Vector3 relativeControllerVelocity = slider.transform.InverseTransformDirection(controllerPose.GetVelocity());
            cpBody.velocity = new Vector3(0, relativeControllerVelocity.y, 0);
            slider.SetActive(false);
            grabbed = false;
            //ShowButtons();
            //HideButtons();
        }
    }

    public override void HandleTrackPadPos(Vector2 pos)
    {
        if (grabbed)
        {
            if (pos.y <= 0)
            {
                cpFixture.transform.Translate(new Vector3(0, -0.01f, 0));
            }
            else if (pos.y > 0)
            {
                cpFixture.transform.Translate(new Vector3(0, 0.01f, 0));
            }
            cpBody.velocity = Vector3.zero;
            initialHeight = cpFixture.transform.position.y;
            slider.transform.position = controllerPose.transform.position;
            Vector3 lookAtPos = new Vector3(cpFixture.transform.position.x, slider.transform.position.y, cpFixture.transform.position.z);
            slider.transform.LookAt(lookAtPos);
        }
    }

    private void HideButtons()
    {
        // buttonPair.SetActive(false);
    }

    private void ShowButtons()
    {
       // buttonPair.SetActive(true);
    }
}
