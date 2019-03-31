using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSet : MonoBehaviour
{

    public Material highlightMat;
    public Material defaultMat;
    public MeshRenderer[] buttonMeshes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide()
    {
        transform.gameObject.SetActive(false);
    }

    public void Show()
    {
        transform.gameObject.SetActive(true);
    }

    public void HighlightButton(int buttonIndex)
    {
        for(int i = 0; i < buttonMeshes.Length; i++)
        {
            if(i == buttonIndex)
            {
                buttonMeshes[i].material = highlightMat;
            }
            else
            {
                buttonMeshes[i].material = defaultMat;
            }
        }
    }

    public void RemoveButtonHighlights()
    {
        foreach(MeshRenderer mesh in buttonMeshes)
        {
            mesh.material = defaultMat;
        }
    }
}
