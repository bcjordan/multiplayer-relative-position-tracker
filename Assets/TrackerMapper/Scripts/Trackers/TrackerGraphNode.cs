using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackerGraphNode
{
    public string uniqueTrackerID;

    private IDictionary<TrackerGraphNode, PositionRotationTransform> nextGraphNodes = new Dictionary<TrackerGraphNode, PositionRotationTransform>();

    public TrackerGraphNode(string newTrackerID)
    {
        uniqueTrackerID = newTrackerID;
    }

    public bool ContainsNeighbor(TrackerGraphNode graphNode)
    {
        return nextGraphNodes.ContainsKey(graphNode);
    }

    public void AddNeighbor(TrackerGraphNode childGraphNode, PositionRotationTransform positionRotationTransform)
    {
        nextGraphNodes[childGraphNode] = positionRotationTransform;
    }

    public IEnumerable<KeyValuePair<PositionRotationTransform, TrackerGraphNode>> EachAbsoluteTrackerNode(PositionRotationTransform startingPositionRotation)
    {
        return EachAbsoluteTrackerNodeExcluding(startingPositionRotation, new List<TrackerGraphNode>());
    }

    public IEnumerable<KeyValuePair<PositionRotationTransform, TrackerGraphNode>> EachAbsoluteTrackerNodeExcluding(PositionRotationTransform currentPositionRotation, IList<TrackerGraphNode> excludingNodes)
    {
        if (excludingNodes.Contains(this))
        {
            yield break;
        }

        excludingNodes.Add(this);
        yield return new KeyValuePair<PositionRotationTransform, TrackerGraphNode>(currentPositionRotation, this);

        foreach (KeyValuePair<TrackerGraphNode, PositionRotationTransform> pair in nextGraphNodes)
        {
            PositionRotationTransform nextTransform = currentPositionRotation.AddTo(pair.Value);

            foreach (KeyValuePair<PositionRotationTransform, TrackerGraphNode> remainingNodes in pair.Key.EachAbsoluteTrackerNodeExcluding(nextTransform, excludingNodes))
            {
                yield return new KeyValuePair<PositionRotationTransform, TrackerGraphNode>(remainingNodes.Key, remainingNodes.Value);
            }
        }
    }
}

