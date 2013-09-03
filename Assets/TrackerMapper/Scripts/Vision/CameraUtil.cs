using UnityEngine;
using System.Collections;

public class CameraUtil
{
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

