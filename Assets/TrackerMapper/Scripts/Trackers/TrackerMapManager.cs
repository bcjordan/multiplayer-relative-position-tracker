using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackerMapManager : Manager
{
    private IDictionary<string, TrackerGraphNode> trackerGraphNodesByID = new Dictionary<string, TrackerGraphNode>();

    public Color debugFirstNodeColor;
    public float debugFirstNodeRadius;

    public Color debugAdjacentNodesColor;
    public float debugAdjacentNodesRadius;

    void Start()
    {
        Managers.GetManager<VisibleTrackerManager>().SeeMultipleTrackers += OnMultipleTrackersSeen;
    }

    void Destroy()
    {
        Managers.GetManager<VisibleTrackerManager>().SeeMultipleTrackers -= OnMultipleTrackersSeen;
    }

    void OnDrawGizmos()
    {
        VisibleTrackerManager visibleTrackerManager = Managers.GetManager<VisibleTrackerManager>();

        if (!visibleTrackerManager.HasVisibleTrackers())
        {
            return;
        }

        Tracker firstTracker = visibleTrackerManager.TryGetFirstVisibleTracker();

        if (!trackerGraphNodesByID.ContainsKey(firstTracker.uniqueID))
        {
            return;
        }

        TrackerGraphNode firstNode = GetNodeForID(firstTracker.uniqueID);

        Gizmos.color = debugFirstNodeColor;
        Gizmos.DrawSphere(firstTracker.transform.position, debugFirstNodeRadius);

        Gizmos.color = debugAdjacentNodesColor;

        PositionRotationTransform startingAbsoluteTransform = new PositionRotationTransform(firstTracker.transform);

        foreach (KeyValuePair<PositionRotationTransform, TrackerGraphNode> pair in firstNode.EachAbsoluteTrackerNode(startingAbsoluteTransform))
        {
            Gizmos.DrawSphere(pair.Key.position, debugAdjacentNodesRadius);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(140, 0, 350, 40), trackerGraphNodesByID.Count.ToString() +
                  " mapped tracker" + (trackerGraphNodesByID.Count == 1 ? "" : "s"));
    }

    void OnMultipleTrackersSeen(IList<Tracker> trackers)
    {
        for (int itemA = 0; itemA < trackers.Count - 1; itemA++)
        {
            for (int itemB = itemA + 1; itemB < trackers.Count; itemB++)
            {
                MapAdjacentTrackerPair(trackers[itemA], trackers[itemB]);
            }
        }
    }

	public void MapAdjacentTrackerPair(Tracker trackerA, Tracker trackerB)
    {
        StoreKnownTrackerNode(trackerA.uniqueID);
        StoreKnownTrackerNode(trackerB.uniqueID);

        TrackerGraphNode trackerANode = trackerGraphNodesByID[trackerA.uniqueID];
        TrackerGraphNode trackerBNode = trackerGraphNodesByID[trackerB.uniqueID];

        if (!trackerANode.ContainsNeighbor(trackerBNode))
        {
            trackerANode.AddNeighbor(trackerBNode, RelativePositionRotationTransform(trackerA, trackerB));
        }

        if (!trackerBNode.ContainsNeighbor(trackerANode))
        {
            trackerBNode.AddNeighbor(trackerANode, RelativePositionRotationTransform(trackerB, trackerA));
        }
    }

    PositionRotationTransform RelativePositionRotationTransform(Tracker fromTracker, Tracker toTracker)
    {
        PositionRotationTransform relativePositionRotationTransform = new PositionRotationTransform();
        relativePositionRotationTransform.position = toTracker.transform.position - fromTracker.transform.position;
        relativePositionRotationTransform.rotation = toTracker.transform.rotation.eulerAngles - fromTracker.transform.rotation.eulerAngles;
        return relativePositionRotationTransform;
    }

    void StoreKnownTrackerNode(string trackerID)
    {
        if (!trackerGraphNodesByID.ContainsKey(trackerID))
        {
            trackerGraphNodesByID[trackerID] = new TrackerGraphNode(trackerID);
        }
    }

    public TrackerGraphNode GetNodeForTracker(Tracker tracker)
    {
        StoreKnownTrackerNode(tracker.uniqueID);
        return trackerGraphNodesByID[tracker.uniqueID];
    }

    public TrackerGraphNode GetNodeForID(string trackerID)
    {
        StoreKnownTrackerNode(trackerID);
        return trackerGraphNodesByID[trackerID];
    }
}
