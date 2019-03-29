﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTeleporter : MonoBehaviour
{
    public GameObject cuttingPlane;
    public GameObject smallScaleModelTable;
    public GameObject cameraRig;

    private Vector3 defaultCuttingPlanePos;
    private Vector3 cuttingPlaneTargetPos;
    private Vector3 modelTableTargetPos;

    private float planeSpeed;
    private Vector3 planePosStartDiff;
    private Vector3 planePosEndDiff;

    // Start is called before the first frame update
    void Start()
    {
        defaultCuttingPlanePos = new Vector3(0, 1000, 0);
        cuttingPlaneTargetPos = defaultCuttingPlanePos;
        modelTableTargetPos = smallScaleModelTable.transform.position;
        planeSpeed = 6;
        planePosStartDiff = new Vector3(0, 4, 0);
        planePosEndDiff = new Vector3(0, 0.15f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float step = planeSpeed * Time.deltaTime;
        cuttingPlane.transform.position = Vector3.MoveTowards(cuttingPlane.transform.position, cuttingPlaneTargetPos, step);
    }

    public void SetNewPositions(Vector3 targetTablePos, bool useCuttingPlane)
    {
        smallScaleModelTable.transform.position = targetTablePos;

        float yDiff = Mathf.Abs((targetTablePos + planePosEndDiff).y - cuttingPlane.transform.position.y);
        if (useCuttingPlane && yDiff > 0.01f)
        {
            cuttingPlane.transform.position = targetTablePos + planePosStartDiff;
            cuttingPlaneTargetPos = targetTablePos + planePosEndDiff;
        }
        else if(!useCuttingPlane)
        {
            cuttingPlane.transform.position = defaultCuttingPlanePos;
        }
    }
}
