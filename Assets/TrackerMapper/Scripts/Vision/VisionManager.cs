using UnityEngine;
using System.Collections;

public class VisionManager : Manager
{
    public Camera cameraToUse;

    void Start()
    {
        cameraToUse = Camera.main;
    }

    public bool IsVisible(GameObject objectToCheck)
    {
        if (cameraToUse == null)
        {
            Debug.Log("Null camera, re-start scene.");
            return false;
        }

        Camera camera = cameraToUse.GetComponent<Camera>();

        return CameraUtil.IsWithinFrustrum(objectToCheck.renderer, camera) &&
            !CameraUtil.IsOccluded(objectToCheck.transform.position, camera.transform.position);
    }
}
