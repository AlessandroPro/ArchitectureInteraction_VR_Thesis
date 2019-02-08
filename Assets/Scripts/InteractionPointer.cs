 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class InteractionPointer : MonoBehaviour
{
    public Transform controller;
    public Transform hitObject;

    public GameObject projectilePrefab;
    public GameObject hand;
    public int numPoints = 80;

    private Vector3 xVector;
    private Vector3 yVector;
    private GameObject[] projectiles;
    private float handTime;

    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabAction;

    // Start is called before the first frame update
    void Start()
    {
        projectiles = new GameObject[numPoints];
        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i] = Instantiate(projectilePrefab);
        }

        handTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        xVector = hitObject.position - controller.position;

        float distance = xVector.magnitude;
        Vector3 hypVector = controller.forward;
        float angle = Vector3.Angle(xVector, hypVector);
        if(angle > 50f)
        {
            hypVector = Vector3.RotateTowards(controller.forward, xVector, (angle * Mathf.Deg2Rad) - (50f * Mathf.Deg2Rad), 0.0f);
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
        float interval = timeTotal / projectiles.Length;
        for (int i = 0; i < projectiles.Length; i++)
        {
            float time = interval * i;

            Vector3 xPos = xVector * Vx * time;
            Vector3 yPos = yVector * (Vy * time - 0.5f * 9.8f * time * time);

            projectiles[i].transform.position = controller.position + xPos + yPos;

        }

        handTime += Time.deltaTime*3;
        if(grabAction.GetStateDown(handType))
        {
            Debug.Log("TRETETEE");
            hand.transform.position = controller.position;
            handTime = 0;
        }
        Vector3 xPos1 = xVector * Vx * handTime;
        Vector3 yPos1 = yVector * (Vy * handTime - 0.5f * 9.8f * handTime * handTime);

        hand.transform.position = controller.position + xPos1 + yPos1;
    }
}