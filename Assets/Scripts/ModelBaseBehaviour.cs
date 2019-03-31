using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class ModelBaseBehaviour : Interactable
{
    public GameObject cameraRig;
    private Quaternion initialRotation;
    private Rigidbody baseBody;
    private Vector3 relativeControllerPos;

    private GameObject[] spokes;
    public GameObject spokePrefab;

    private float hiddenButtonsY;
    private float shownButtonsY;
    private float buttonsPosY;
    private float buttonSpeed;

    // Start is called before the first frame update
    void Start()
    {
        baseBody = GetComponent<Rigidbody>();
        relativeControllerPos = Vector3.zero;
        ghostHand.SetActive(false);
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
        ghostHand.transform.LookAt(new Vector3(transform.position.x, ghostHand.transform.position.y, transform.position.z));
    }

    override public void HandleEnter(SteamVR_Behaviour_Pose pose)
    {
        controllerPose = pose;
        ghostHand.SetActive(true);
        ShowHighlight();

        /*
        Vector3 centerToUser = user.transform.position - transform.position;
        Vector3 centreToButtons = buttonPair.transform.position - transform.position;
        float buttonPairYPos = buttonPair.transform.position.y;

        centerToUser = new Vector3(centerToUser.x, 0, centerToUser.z);
        centreToButtons = new Vector3(centreToButtons.x, 0, centreToButtons.z);

        float centreToButtonsRadius = centreToButtons.magnitude;

        buttonPair.transform.position = transform.position + (centerToUser.normalized * centreToButtonsRadius);
        buttonPair.transform.position = new Vector3(buttonPair.transform.position.x, buttonPairYPos, buttonPair.transform.position.z);
        buttonPair.transform.LookAt(buttonPair.transform.position + centerToUser.normalized); 

        ShowArrowButtons();
        */
    }

    override public void HandleExit()
    {
        slider.SetActive(false);
        ghostHand.SetActive(false);
        HideHighlight();
        SwapButtonSet(modelButton, buttonPair);

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
            angle = 360f - angle;

            float spokeAngleInterval = 360f / spokes.Length;
            float spokeIndexFloat = angle / spokeAngleInterval;
            int spokeIndex = Mathf.RoundToInt(spokeIndexFloat);

            if (spokeIndex >= spokes.Length)
            {
                spokeIndex = 0;
            }

            Vector3 offset = (spokes[spokeIndex].transform.position - transform.position).normalized * 0.2f;
            offset = new Vector3(offset.x, 0, offset.z);
            ghostHand.transform.position = spokes[spokeIndex].transform.position + offset;
        }
    }



    override public void HandleButtonClickDown() { }

    override public void HandleButtonClickHold() { }

    override public void HandleButtonClickUp()
    {
        buttonPair.RemoveButtonHighlights();
        modelButton.RemoveButtonHighlights();
    }

    override public void HandleTriggerDown(Vector3 hitPoint)
    {
        baseBody.angularVelocity = Vector3.zero;
        slider.transform.position = controllerPose.transform.position;
        Vector3 lookAtPos = new Vector3(transform.position.x, slider.transform.position.y, transform.position.z);
        slider.transform.LookAt(lookAtPos);
        //slider.SetActive(true);
        initialRotation = transform.rotation;
        grabbed = true;
        SwapButtonSet(buttonPair, modelButton);
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
            float angle = Vector3.SignedAngle(Vector3.forward, cameraRig.transform.forward, Vector3.up);
            Vector3 velocity = Quaternion.Euler(0, angle, 0) * controllerPose.GetVelocity();
            Vector3 relativeControllerVelocity = slider.transform.InverseTransformDirection(velocity);
            //if (Mathf.Abs(relativeControllerVelocity.y) > 0.1f)
            //{
                baseBody.angularVelocity = new Vector3(0, -relativeControllerVelocity.x * 2, 0);
            //}
            slider.SetActive(false);
            grabbed = false;
            //ShowArrowButtons();
            SwapButtonSet(modelButton, buttonPair);
        }
    }

    public override void HandleTrackPadPos(Vector2 pos)
    {
        if(grabbed)
        {
            if(pos.x <= 0)
            {
                transform.Rotate(0, 1, 0);
                buttonPair.HighlightButton(1);
            }
            else if(pos.x > 0)
            {
                transform.Rotate(0, -1, 0);
                buttonPair.HighlightButton(0);
            }
            initialRotation = transform.rotation;
            baseBody.angularVelocity = Vector3.zero;
            slider.transform.position = controllerPose.transform.position;
            Vector3 lookAtPos = new Vector3(transform.position.x, slider.transform.position.y, transform.position.z);
            slider.transform.LookAt(lookAtPos);
        }
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

}
