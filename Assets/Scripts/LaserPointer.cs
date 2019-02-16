using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class LaserPointer : MonoBehaviour
{

    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean teleportAction;

    public LineRenderer laserLine;
    public Vector3 hitPoint; // 4

    // 1
    public Transform cameraRigTransform;
    // 2
    public GameObject teleportReticlePrefab;
    // 3
    private GameObject reticle;
    // 4
    private Transform teleportReticleTransform;
    // 5
    public Transform headTransform;
    // 6
    public Vector3 teleportReticleOffset;
    // 7
    public LayerMask teleportMask;
    // 8
    private bool shouldTeleport;



    // Start is called before the first frame update
    void Start()
    {
        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // 1
        //if (teleportAction.GetState(handType))
        // {
        // 2
        RaycastHit hit;

            if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 100))
            {
                hitPoint = hit.point;
                ShowLaser();
                // 1
                reticle.SetActive(true);
                // 2
                teleportReticleTransform.position = hitPoint + teleportReticleOffset;
                // 3
                shouldTeleport = true;
            }
        else
        {
            reticle.SetActive(false);
            hitPoint = transform.position + (transform.forward * 10f);
            ShowLaser();
        }
       // }
       // else // 
       // {
        //    laser.enabled = true;
       //     reticle.SetActive(false);
       // }

        if (teleportAction.GetStateUp(handType) && shouldTeleport)
        {
            Teleport();
        }


    }

    private void ShowLaser()
    {
        laserLine.enabled = true;

        laserLine.SetPosition(0, transform.position);
        laserLine.SetPosition(1, hitPoint);
    }

    private void Teleport()
    {
        // 1
        shouldTeleport = false;
        // 2
        reticle.SetActive(false);
        // 3
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        // 4
        difference.y = 0;
        // 5
        cameraRigTransform.position = hitPoint + difference;
    }


}
