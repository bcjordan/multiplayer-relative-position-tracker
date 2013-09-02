using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RelativeSpacePlacedObjectManager : Manager
{
    public List<GameObject> prefabsByIndex;

    private IDictionary<string, IList<PlacedObject>> placedObjectsByNode = new Dictionary<string, IList<PlacedObject>>();

    void Start()
    {
        Managers.GetManager<PositionedTrackerUpdateManager>().TrackerPositionUpdate += OnTrackerPosition;
    }

    void Destroy()
    {
        Managers.GetManager<PositionedTrackerUpdateManager>().TrackerPositionUpdate -= OnTrackerPosition;
    }

    void OnTrackerPosition(string trackerID, PositionRotationTransform trackerAbsolutePositionRotation)
    {
        foreach (PlacedObject placedObject in GetObjectsForGraphNode(trackerID))
        {
            if (placedObject.instanceObject == null)
            {
                PositionRotationTransform absolutePositionRotationTransform = trackerAbsolutePositionRotation.AddTo(placedObject.positionRotationTransform);
                placedObject.instanceObject = (GameObject)Instantiate(placedObject.prefabObject, absolutePositionRotationTransform.position, Quaternion.Euler(absolutePositionRotationTransform.rotation));
            }
        }
    }

    public void PlaceObjectAtGraphNode(string trackerID, PositionRotationTransform positionRotationTransform, int prefabID)
    {
        GetObjectsForGraphNode(trackerID).Add(new PlacedObject(positionRotationTransform, prefabsByIndex[prefabID]));
    }

    public void PlaceObjectAtGraphNode(TrackerGraphNode trackerGraphNode, PositionRotationTransform positionRotationTransform, GameObject prefabObject)
    {
        GetObjectsForGraphNode(trackerGraphNode.uniqueTrackerID).Add(new PlacedObject(positionRotationTransform, prefabObject));
    }

    IList<PlacedObject> GetObjectsForGraphNode(string uniqueTrackerID)
    {
        if (!placedObjectsByNode.ContainsKey(uniqueTrackerID))
        {
            placedObjectsByNode[uniqueTrackerID] = new List<PlacedObject>();
        }

        return placedObjectsByNode[uniqueTrackerID];
    }

    void OnGUI()
    {
        GUI.Label(new Rect(140, 80, 350, 40), placedObjectsByNode.Count.ToString() +
                  " placed object" + (placedObjectsByNode.Count == 1 ? "" : "s"));
    }
}
