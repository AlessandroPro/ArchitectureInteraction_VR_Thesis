using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHandler : MonoBehaviour
{

    public GameObject arrow;
    public GameObject cuttingPlane;
    public GameObject particlesPrefab;
    public Orb[] orbs;
    private Orb activeOrb;
    private float arrowRotation;
    private int orbIndex;

    // Start is called before the first frame update
    void Start()
    {
        arrowRotation = 0;
        orbIndex = 0;

        if(orbs.Length > 0)
        {
            activeOrb = orbs[0];
            activeOrb.turnOn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        arrowRotation += 180 * Time.deltaTime;
        if (arrowRotation > 360)
        {
            arrowRotation -= 360;
        }

        if(activeOrb)
        {
            arrow.transform.LookAt(activeOrb.transform.position);

            if(activeOrb.touched)
            {
                ChangeToNextOrb();
            }

            if(cuttingPlane.transform.position.y <= activeOrb.transform.position.y && activeOrb.gameObject.activeSelf)
            {
                activeOrb.turnOff();
            }
            else if(cuttingPlane.transform.position.y > activeOrb.transform.position.y && !activeOrb.gameObject.activeSelf)
            {
                activeOrb.turnOn();
            }
        }
        arrow.transform.RotateAround(arrow.transform.position, arrow.transform.forward, arrowRotation);
    }



    private void ChangeToNextOrb()
    {
        Instantiate(particlesPrefab, activeOrb.transform.position, activeOrb.transform.rotation);
        activeOrb.turnOff();
        orbIndex++;

        if (orbIndex >= orbs.Length)
        {
            orbIndex = 0;
        }

        if (orbs.Length > 0)
        {
            activeOrb = orbs[orbIndex];
            activeOrb.turnOn();
        }   
    }
}
