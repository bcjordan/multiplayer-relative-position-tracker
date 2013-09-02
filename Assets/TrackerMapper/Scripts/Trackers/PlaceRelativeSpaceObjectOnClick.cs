using UnityEngine;
using System.Collections;

public class PlaceRelativeSpaceObjectOnClick : MonoBehaviour
{
    public Vector3 placementPositionOffset;
    public GameObject prefabToPlace;

    void OnMouseDown()
    {
        TrackerGraphNode trackerGraphNode = Managers.GetManager<TrackerMapManager>().GetNodeForTracker(GetComponent<Tracker>());

        PositionRotationTransform newObjectTransform = new PositionRotationTransform(placementPositionOffset, Vector3.zero);
        Managers.GetManager<RelativeSpacePlacedObjectManager>().PlaceObjectAtGraphNode(trackerGraphNode, newObjectTransform, prefabToPlace);
    }
}
