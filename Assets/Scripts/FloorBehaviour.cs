﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class FloorBehaviour : Interactable
{
    public GameObject fullScaleModel;
    public GameObject smallScaleModel;
    public GameObject smallScaleModelTable;
    private Vector3 centerPos;

    public GameObject avatar;
    public Transform cameraRig;
    public GameObject cuttingPlane;

    private Quaternion initialRotation;

    public bool isSmallScale;
    public bool useCuttingPlane;
    private bool isGhosted;

    // Start is called before the first frame update
    void Start()
    {
        centerPos = GetComponent<Renderer>().bounds.center;

        relativeControllerPos = Vector3.zero;

        grabbed = false;


        avatar.SetActive(false);

        if (cuttingPlane.transform.position.y < centerPos.y)
        {
            GetComponent<Collider>().enabled = false;
            isGhosted = true;
        }
        else if (cuttingPlane.transform.position.y > centerPos.y)
        {
            GetComponent<Collider>().enabled = true;
            isGhosted = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        centerPos = GetComponent<Renderer>().bounds.center;

        if (cuttingPlane.transform.position.y < centerPos.y && !isGhosted)
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
        ShowButtonSet(modelButton);
    }

    override public void HandleExit()
    {
        slider.SetActive(false);
        avatar.SetActive(false);
        HideButtonSet(buttonPair);
        HideButtonSet(modelButton);
    }

    public override void HandleStay(Vector3 hitPoint)
    {
        if (!grabbed)
        {
            ghostHand.transform.position = hitPoint;
            Vector3 controllerForwardXZ = new Vector3(controllerPose.transform.forward.x, 0, controllerPose.transform.forward.z);
            avatar.transform.LookAt(avatar.transform.position + controllerForwardXZ);
            if (!isSmallScale)
            {
                //smallScaleModelTableGhosted.transform.position = hitPoint;
                //smallScaleModelTable.transform.position = hitPoint;
            }
        }
        avatar.transform.position = ghostHand.transform.position;
    }

    override public void ShowHighlight() { }

    override public void HideHighlight() { }

    override public void HandleButtonClickDown()
    {
        if(!grabbed)
        {
            modelButton.HighlightButton(0);
            if(!isSmallScale)
            {
                tableTeleporter.SetNewPositions(ghostHand.transform.position, useCuttingPlane);
                actionLogger.logAction(ActionLogger.Actions.model_move);
            }
        }
    }

    override public void HandleButtonClickHold() { }

    override public void HandleButtonClickUp()
    {
        buttonPair.RemoveButtonHighlights();
        modelButton.RemoveButtonHighlights();
    }

    override public void HandleTriggerDown(Vector3 hitPoint)
    {
        slider.transform.position = controllerPose.transform.position;
        Vector3 lookAtPos = new Vector3(ghostHand.transform.position.x, slider.transform.position.y, ghostHand.transform.position.z);
        slider.transform.LookAt(lookAtPos);
        //slider.SetActive(true);
        initialRotation = avatar.transform.rotation;
        grabbed = true;
        SwapButtonSet(buttonPair, modelButton);
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

            if (Mathf.Abs(relativeControllerPos.x) > 1)
            {
                handMovementChoice = true;
            }

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
            SwapButtonSet(modelButton, buttonPair);

            if (buttonChoice)
            {
                actionLogger.logAction(ActionLogger.Actions.avatar_buttons);
            }
            if (handMovementChoice)
            {
                actionLogger.logAction(ActionLogger.Actions.avatar_hand);
            }
            buttonChoice = false;
            handMovementChoice = false;

            StartCoroutine(TeleportFade(avatar.transform.position, avatar.transform.forward));
        }
    }

    override public void HandleTrackPadPos(Vector2 pos)
    {
        if (grabbed)
        {
            if (pos.x <= 0)
            {
                avatar.transform.Rotate(0, -3, 0);
                buttonPair.HighlightButton(1);
            }
            else if (pos.x > 0)
            {
                avatar.transform.Rotate(0, 3, 0);
                buttonPair.HighlightButton(0);
            }
            initialRotation = avatar.transform.rotation;
            slider.transform.position = controllerPose.transform.position;
            Vector3 lookAtPos = new Vector3(avatar.transform.position.x, slider.transform.position.y, avatar.transform.position.z);
            slider.transform.LookAt(lookAtPos);

            buttonChoice = true;
        }
    }

    // Creates a fade out and fade in effect for the entire display
    IEnumerator TeleportFade(Vector3 newLocation, Vector3 direction)
    {
        avatar.SetActive(false);
        SteamVR_Fade.Start(Color.black, 0.4f);
        yield return new WaitForSeconds(0.4f);

        Vector3 userForward = new Vector3(user.transform.forward.x, 0, user.transform.forward.z);

        if (isSmallScale)
        {
            Vector3 posInSmallModel = smallScaleModel.transform.InverseTransformPoint(newLocation);
            Vector3 forwardInSmallModel = smallScaleModel.transform.InverseTransformDirection(direction);

            float angle = Vector3.SignedAngle(userForward, fullScaleModel.transform.TransformDirection(forwardInSmallModel), Vector3.up);
            cameraRig.transform.Rotate(0, angle, 0);

            Vector3 difference = cameraRig.transform.position - user.transform.position;
            difference.y = 0;
            cameraRig.transform.position = fullScaleModel.transform.TransformPoint(posInSmallModel) + difference;
            actionLogger.logAction(ActionLogger.Actions.teleport_model);
        }
        else
        {
            float angle = Vector3.SignedAngle(userForward, direction, Vector3.up);
            cameraRig.transform.Rotate(0, angle, 0);

            Vector3 difference = cameraRig.transform.position - user.transform.position;
            difference.y = 0;
            cameraRig.transform.position = newLocation + difference;
            actionLogger.logAction(ActionLogger.Actions.teleport_local);
        }

        avatar.SetActive(true);
        SteamVR_Fade.Start(Color.clear, 0.4f);
    }
}
