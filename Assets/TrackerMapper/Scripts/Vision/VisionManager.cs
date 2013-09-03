using UnityEngine;
using System.Collections;

public class VisionManager : Manager
{
    public GameObject cameraToUse;

    void Start()
    {
        cameraToUse = Camera.main.gameObject;
    }

    public bool IsVisible(GameObject objectToCheck)
    {
        if (cameraToUse == null)
        {
            Debug.Log("Null camera, re-start scene.");
            return false;
        }

        Camera camera = cameraToUse.GetComponent<Camera>();
        return !IsOccluded(objectToCheck.transform.position, camera.transform.position) &&
            IsWithinFrustrum(objectToCheck.renderer, camera);
    }

    public static bool IsOccluded(Vector3 positionA, Vector3 positionB)
    {
        return Physics.Linecast(positionA, positionB);
    }

    public static bool IsWithinFrustrum(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
}
