using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBaseBehaviour : Interactable
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    override public void HandleEnter()
    {
        Debug.Log("ENTER");
    }

    override public void HandleExit()
    {
        Debug.Log("EXITTTTTTTTTT");
    }

    override public void ShowHighlight() { }

    override public void HideHighlight() { }

    override public void HandleButtonClickDown() { }

    override public void HandleButtonClickHold() { }

    override public void HandleButtonClickUp() { }

    override public void HandleTriggerDown(Vector3 hitPoint)
    {

    }

    override public void HandleTriggerHold() { }

    override public void HandleTriggerUp() { }

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
