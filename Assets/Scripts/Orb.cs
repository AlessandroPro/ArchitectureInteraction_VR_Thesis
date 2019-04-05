using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public LayerMask controllerMask;
    public bool touched;
    
    // Start is called before the first frame update
    void Start()
    {
        turnOff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & controllerMask) != 0)
        {
            touched = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & controllerMask) != 0)
        {
            touched = false;
        }
    }

    public void turnOn()
    {
        gameObject.SetActive(true);
    }

    public void turnOff()
    {
        gameObject.SetActive(false);
        touched = false;
    }
}
