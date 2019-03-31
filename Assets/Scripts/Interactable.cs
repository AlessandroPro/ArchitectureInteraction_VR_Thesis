using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public abstract class Interactable : MonoBehaviour
{
    public SteamVR_Behaviour_Pose controllerPose;
    public Transform user;
    protected bool grabbed;
    public GameObject ghostHand;
    public GameObject slider;
    public Material laserMat;
    public ButtonSet buttonPair;
    public ButtonSet modelButton;

    public MeshRenderer[] highlightMeshes;
    public Material highlightMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void HandleEnter(SteamVR_Behaviour_Pose pose);

    public abstract void HandleExit();

    public abstract void HandleStay(Vector3 hitPoint);

    public abstract void HandleButtonClickDown();

    public abstract void HandleButtonClickHold();

    public abstract void HandleButtonClickUp();

    public abstract void HandleTriggerDown(Vector3 hitPoint);

    public abstract void HandleTriggerHold();

    public abstract void HandleTriggerUp();

    public abstract void HandleTrackPadPos(Vector2 pos);

    public virtual Material GetLaserMaterial()
    {
        return laserMat;
    }

    public virtual void ShowHighlight()
    {
        foreach (MeshRenderer mesh in highlightMeshes)
        {
            Material bodyMat = mesh.material;
            mesh.materials = new Material[2] { bodyMat, highlightMat };
        }
    }

    public virtual void HideHighlight()
    {
        foreach (MeshRenderer mesh in highlightMeshes)
        {
            Material bodyMat = mesh.material;
            mesh.materials = new Material[1] { bodyMat };
        }
    }

    protected virtual void SwapButtonSet(ButtonSet bsIn, ButtonSet bsOut)
    {
        bsOut.Hide();
        bsIn.Show();
    }
}
