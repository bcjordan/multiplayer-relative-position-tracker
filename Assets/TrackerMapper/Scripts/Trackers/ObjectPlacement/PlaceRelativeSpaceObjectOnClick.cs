using UnityEngine;
using System.Collections;

public class PlaceRelativeSpaceObjectOnClick : MonoBehaviour
{
    public float distanceToPlaceTowardsCamera;

    public int prefabIDToPlace;

    void OnMouseDown()
    {
        Tracker tracker = GetComponent<Tracker>();
        TrackerGraphNode trackerGraphNode = Managers.GetManager<TrackerMapManager>().GetNodeForID(tracker.uniqueID);

        PositionRotationTransform newObjectTransform = PositionRotationTransform.FromTo(tracker.gameObject, Managers.GetManager<VisionManager>().cameraToUse.gameObject);
        newObjectTransform.position = newObjectTransform.position.normalized * distanceToPlaceTowardsCamera;

        Managers.GetManager<TrackerNetworkingManager>().PlaceRelativeSpaceObject(trackerGraphNode.uniqueTrackerID, newObjectTransform, prefabIDToPlace);
    }
}
