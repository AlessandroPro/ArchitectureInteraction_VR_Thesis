using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealAvatar : MonoBehaviour
{
    public GameObject camera;
    public GameObject fullScaleModel;
    public GameObject smallScaleModel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetNewTransform();
    }

    private void SetNewTransform()
    {
        Vector3 worldAvatarPos = new Vector3(camera.transform.position.x, camera.transform.parent.position.y, camera.transform.position.z);
        Vector3 fullModelPos = fullScaleModel.transform.InverseTransformPoint(worldAvatarPos);
        transform.position = smallScaleModel.transform.TransformPoint(fullModelPos);

        Vector3 fullModelCameraDir = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z);
        float fullAngle = Vector3.SignedAngle(fullScaleModel.transform.forward, fullModelCameraDir, Vector3.up);
        float smallAngle = Vector3.SignedAngle(smallScaleModel.transform.forward, transform.forward, Vector3.up);
        transform.Rotate(0, fullAngle - smallAngle, 0);
    }
}
