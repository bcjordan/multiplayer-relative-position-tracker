using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RelativeSpacePlacedObjectManager : Manager
{
    private IDictionary<TrackerGraphNode, IList<PlacedObject>> placedObjectsByNode = new Dictionary<TrackerGraphNode, IList<PlacedObject>>();

    void Update()
    {
        AttemptToDrawPlacedObjects();
    }

    void AttemptToDrawPlacedObjects()
    {
        Tracker firstTracker = Managers.GetManager<VisibleTrackerManager>().TryGetFirstVisibleTracker();
        if (firstTracker == null)
        {
            return;
        }

        PositionRotationTransform startingAbsoluteTransform = new PositionRotationTransform(firstTracker.transform);

        foreach(KeyValuePair<PositionRotationTransform, TrackerGraphNode> pair in Managers.GetManager<TrackerMapManager>().GetNodeForTracker(firstTracker).EachAbsoluteTrackerNode(startingAbsoluteTransform))
        {
            foreach (PlacedObject placedObject in GetObjectsForGraphNode(pair.Value))
            {
                if (placedObject.instanceObject == null)
                {
                    PositionRotationTransform absolutePositionRotationTransform = pair.Key.AddTo(placedObject.positionRotationTransform);
                    placedObject.instanceObject = (GameObject)Instantiate(placedObject.prefabObject, absolutePositionRotationTransform.position, Quaternion.Euler(absolutePositionRotationTransform.rotation));
                }
            }
        }
    }

    public void PlaceObjectAtGraphNode(TrackerGraphNode trackerGraphNode, PositionRotationTransform positionRotationTransform, GameObject prefabObject)
    {
        GetObjectsForGraphNode(trackerGraphNode).Add(new PlacedObject(positionRotationTransform, prefabObject));
    }

    IList<PlacedObject> GetObjectsForGraphNode(TrackerGraphNode trackerGraphNode)
    {
        if (!placedObjectsByNode.ContainsKey(trackerGraphNode))
        {
            placedObjectsByNode[trackerGraphNode] = new List<PlacedObject>();
        }

        return placedObjectsByNode[trackerGraphNode];
    }

    void OnGUI()
    {
        GUI.Label(new Rect(140, 80, 350, 40), placedObjectsByNode.Count.ToString() +
                  " placed object" + (placedObjectsByNode.Count == 1 ? "" : "s"));
    }
}
