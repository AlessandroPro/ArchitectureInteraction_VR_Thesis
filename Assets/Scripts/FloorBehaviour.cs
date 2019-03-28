using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class FloorBehaviour : Interactable
{
    public GameObject fullScaleModel;
    public GameObject smallScaleModel;
    private Vector3 centerPos;

    public GameObject avatar;
    public Transform cameraRig;
    public GameObject cuttingPlane;
    private Vector3 relativeControllerPos;
    private Quaternion initialRotation;

    public bool isSmallScale;
    private bool isGhosted;

    // Start is called before the first frame update
    void Start()
    {
        centerPos = GetComponent<Renderer>().bounds.center;

        relativeControllerPos = Vector3.zero;

        grabbed = false;
        isGhosted = false;

        avatar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(cuttingPlane.transform.position.y < centerPos.y && !isGhosted)
        {
            GetComponent<Collider>().enabled = false;
            isGhosted = true;
        }
        else if(cuttingPlane.transform.position.y > centerPos.y && isGhosted)
        {
            GetComponent<Collider>().enabled = true;
            isGhosted = false;
        }
    }

    override public void HandleEnter(SteamVR_Behaviour_Pose pose)
    {
        controllerPose = pose;
        avatar.SetActive(true);
    }

    override public void HandleExit()
    {
        slider.SetActive(false);
        avatar.SetActive(false);
        //HideButtons();
    }

    public override void HandleStay(Vector3 hitPoint)
    {
        if (!grabbed)
        {
            ghostHand.transform.position = hitPoint;
            Vector3 controllerForwardXZ = new Vector3(controllerPose.transform.forward.x, 0, controllerPose.transform.forward.z);
            avatar.transform.LookAt(avatar.transform.position + controllerForwardXZ);
        }
        avatar.transform.position = ghostHand.transform.position;
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
            //Vector3 sliderForwardXZ = new Vector3(slider.transform.forward.x, 0, slider.transform.forward.z);
            //avatar.transform.LookAt(avatar.transform.position + sliderForwardXZ);
            //initialRotation = slider.transform.rotation;

            //Get the position of the controller relative to the slider
            relativeControllerPos = slider.transform.InverseTransformPoint(controllerPose.transform.position);

            Quaternion target = Quaternion.Euler(0, relativeControllerPos.x * 50f, 0) * initialRotation;
            avatar.transform.rotation = Quaternion.Slerp(avatar.transform.rotation, target, Time.deltaTime * 5f);
        }
    }

    override public void HandleTriggerUp()
    {
        if (grabbed)
        {
            slider.SetActive(false);
            grabbed = false;
            //HideButtons();

            TeleportUser();
        }
    }

    override public void HandleTrackPadPos(Vector2 pos)
    {
        if (grabbed)
        {
            if (pos.x <= 0)
            {
                avatar.transform.Rotate(0, -3, 0);
            }
            else if (pos.x > 0)
            {
                avatar.transform.Rotate(0, 3, 0);
            }
            initialRotation = avatar.transform.rotation;
            slider.transform.position = controllerPose.transform.position;
            Vector3 lookAtPos = new Vector3(avatar.transform.position.x, slider.transform.position.y, avatar.transform.position.z);
            slider.transform.LookAt(lookAtPos);
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

    private void TeleportUser()
    {
        Vector3 userForward = new Vector3(user.transform.forward.x, 0, user.transform.forward.z);

        if (isSmallScale)
        {
            Vector3 posInSmallModel = smallScaleModel.transform.InverseTransformPoint(avatar.transform.position);
            Vector3 forwardInSmallModel = smallScaleModel.transform.InverseTransformDirection(avatar.transform.forward);

            float angle = Vector3.SignedAngle(userForward, fullScaleModel.transform.TransformDirection(forwardInSmallModel), Vector3.up);
            cameraRig.transform.Rotate(0, angle, 0);

            Vector3 difference = cameraRig.transform.position - user.transform.position;
            difference.y = 0;
            cameraRig.transform.position = fullScaleModel.transform.TransformPoint(posInSmallModel) + difference;
        }
        else
        {
            float angle = Vector3.SignedAngle(userForward, avatar.transform.forward, Vector3.up);
            cameraRig.transform.Rotate(0, angle, 0);

            Vector3 difference = cameraRig.transform.position - user.transform.position;
            difference.y = 0;
            cameraRig.transform.position = avatar.transform.position + difference;
        }
    }
}
