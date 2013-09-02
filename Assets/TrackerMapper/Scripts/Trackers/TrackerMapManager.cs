using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackerMapManager : Manager
{
    private IDictionary<Tracker, TrackerGraphNode> knownTrackerGraphNodes = new Dictionary<Tracker, TrackerGraphNode>();

    public Color debugFirstNodeColor;
    public float debugFirstNodeRadius;

    public Color debugAdjacentNodesColor;
    public float debugAdjacentNodesRadius;

    void Start()
    {
        Managers.GetManager<VisibleTrackerManager>().SeeMultipleTrackers += OnMultipleTrackersSeen;
    }

    void OnDrawGizmos()
    {
        VisibleTrackerManager visibleTrackerManager = Managers.GetManager<VisibleTrackerManager>();

        if (!visibleTrackerManager.HasVisibleTrackers())
        {
            return;
        }

        Tracker firstTracker = visibleTrackerManager.TryGetFirstVisibleTracker();

        if (!knownTrackerGraphNodes.ContainsKey(firstTracker))
        {
            return;
        }

        TrackerGraphNode firstNode = knownTrackerGraphNodes[firstTracker];

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
        GUI.Label(new Rect(140, 0, 350, 40), knownTrackerGraphNodes.Count.ToString() +
                  " mapped tracker" + (knownTrackerGraphNodes.Count == 1 ? "" : "s"));
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
        StoreKnownTrackerNode(trackerA);
        StoreKnownTrackerNode(trackerB);

        TrackerGraphNode trackerANode = knownTrackerGraphNodes[trackerA];
        TrackerGraphNode trackerBNode = knownTrackerGraphNodes[trackerB];

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

    void StoreKnownTrackerNode(Tracker tracker)
    {
        if (!knownTrackerGraphNodes.ContainsKey(tracker))
        {
            knownTrackerGraphNodes[tracker] = new TrackerGraphNode();
        }
    }

    public TrackerGraphNode GetNodeForTracker(Tracker tracker)
    {
        StoreKnownTrackerNode(tracker);
        return knownTrackerGraphNodes[tracker];
    }
}
