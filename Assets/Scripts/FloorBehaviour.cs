using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class FloorBehaviour : Interactable
{
    public GameObject avatar;
    public GameObject scaleBuilding;
    private Vector3 relativeControllerPos;
    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        relativeControllerPos = Vector3.zero;
        grabbed = false;
    }

    // Update is called once per frame
    void Update()
    {
        avatar.transform.position = ghostHand.transform.position;
    }

    override public void HandleEnter(SteamVR_Behaviour_Pose pose)
    {
        controllerPose = pose;
    }

    override public void HandleExit()
    {
        slider.SetActive(false);
        //HideButtons();
    }

    public override void HandleStay(Vector3 hitPoint)
    {
        if (!grabbed)
        {
            ghostHand.transform.position = hitPoint;
        }
    }

    override public void ShowHighlight() { }

    override public void HideHighlight() { }

    override public void HandleButtonClickDown() { }

    override public void HandleButtonClickHold() { }

    override public void HandleButtonClickUp() { }

    override public void HandleTriggerDown(Vector3 hitPoint)
    {
        slider.transform.position = controllerPose.transform.position;
        Vector3 lookAtPos = new Vector3(ghostHand.transform.position.x, slider.transform.position.y, ghostHand.transform.position.z);
        slider.transform.LookAt(lookAtPos);
        slider.SetActive(true);
        initialRotation = avatar.transform.rotation;
        grabbed = true;
        //ShowButtons();
    }

    override public void HandleTriggerHold()
    {
        if (grabbed)
        {
            //Get the position of the controller relative to the slider
            relativeControllerPos = slider.transform.InverseTransformPoint(controllerPose.transform.position);

            Quaternion target = Quaternion.Euler(0, -relativeControllerPos.x * 20f, 0) * initialRotation;
            avatar.transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5f);
        }
    }

    override public void HandleTriggerUp()
    {
        if (grabbed)
        {
            slider.SetActive(false);
            grabbed = false;
            //HideButtons();
        }
    }

    override public void HandleTrackPadPos(Vector2 pos)
    {
        if (grabbed)
        {
            if (pos.x <= 0)
            {
                avatar.transform.Rotate(0, 1, 0);
            }
            else if (pos.x > 0)
            {
                avatar.transform.Rotate(0, -1, 0);
            }
            initialRotation = transform.rotation;
        }
    }

    private void HideButtons()
    {
        //buttonPair.SetActive(false);
    }

    private void ShowButtons()
    {
       //buttonPair.SetActive(true);
    }
}
