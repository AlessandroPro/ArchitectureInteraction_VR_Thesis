using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class ModelBaseBehaviour : Interactable
{
    public GameObject buttonCW;
    public GameObject buttonCCW;
    public GameObject buttonPair;
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
        grabbed = false;
        spokes = new GameObject[16];

        float spokeAngle = 360 / spokes.Length;
        for(int i = 0; i < spokes.Length; i++)
        {
            spokes[i] = Instantiate(spokePrefab, transform);
            spokes[i].transform.RotateAround(transform.position, Vector3.up, i * spokeAngle);
        }

        shownButtonsY = buttonPair.transform.position.y - transform.position.y;
        hiddenButtonsY = shownButtonsY - 0.05f;
        buttonSpeed = 0.2f;

        buttonsPosY = shownButtonsY;
    }

    // Update is called once per frame
    void Update()
    {
        float step = Time.deltaTime * buttonSpeed;
        Vector3 buttonsNewPos = new Vector3(buttonPair.transform.position.x, transform.position.y + buttonsPosY, buttonPair.transform.position.z);
        //buttonPair.transform.position = Vector3.MoveTowards(buttonPair.transform.position, buttonsNewPos, step);
    }

    override public void HandleEnter(SteamVR_Behaviour_Pose pose)
    {
        controllerPose = pose;

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

        ShowButtons();
        */
    }

    override public void HandleExit()
    {
        slider.SetActive(false);
        HideButtons();

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
        //slider.SetActive(true);
        initialRotation = transform.rotation;
        grabbed = true;
        ShowButtons();
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
            //if (Mathf.Abs(relativeControllerVelocity.y) > 0.1f)
            //{
                baseBody.angularVelocity = new Vector3(0, -relativeControllerVelocity.x * 2, 0);
            //}
            slider.SetActive(false);
            grabbed = false;
            //ShowButtons();
            HideButtons();
        }
    }

    public override void HandleTrackPadPos(Vector2 pos)
    {
        if(grabbed)
        {
            if(pos.x <= 0)
            {
                transform.Rotate(0, 1, 0);
            }
            else if(pos.x > 0)
            {
                transform.Rotate(0, -1, 0);
            }
            initialRotation = transform.rotation;
            baseBody.angularVelocity = Vector3.zero;
            slider.transform.position = controllerPose.transform.position;
            Vector3 lookAtPos = new Vector3(transform.position.x, slider.transform.position.y, transform.position.z);
            slider.transform.LookAt(lookAtPos);
        }
    }

    private void HideButtons()
    {
        //buttonsPosY = hiddenButtonsY;
        buttonPair.SetActive(false);
    }

    private void ShowButtons()
    {
        // buttonsPosY = shownButtonsY;
        buttonPair.SetActive(true);
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
