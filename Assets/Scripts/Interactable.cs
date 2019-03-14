using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public Transform controller;
    public Transform user;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void HandleEnter();

    public abstract void HandleExit();

    public abstract void ShowHighlight();

    public abstract void HideHighlight();

    public abstract void HandleButtonClickDown();

    public abstract void HandleButtonClickHold();

    public abstract void HandleButtonClickUp();

    public abstract void HandleTriggerDown(Vector3 hitPoint);

    public abstract void HandleTriggerHold();

    public abstract void HandleTriggerUp();


}
