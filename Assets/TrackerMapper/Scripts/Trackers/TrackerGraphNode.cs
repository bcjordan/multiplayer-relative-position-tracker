using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class TrackerGraphNode
{
    IList<TrackerGraphNode> nextChildren;

    IDictionary<TrackerGraphNode, PositionRotationTransform> nextGraphNodes = new Dictionary<TrackerGraphNode, PositionRotationTransform>();

    public bool ContainsChild(TrackerGraphNode graphNode)
    {
        return nextGraphNodes.ContainsKey(graphNode);
    }

    public void AddChild(TrackerGraphNode childGraphNode, PositionRotationTransform positionRotationTransform)
    {
        nextGraphNodes[childGraphNode] = positionRotationTransform;
    }

    public void DrawGraphTail(PositionRotationTransform currentPositionRotation, float radius, IList<TrackerGraphNode> seenNodes)
    {
        foreach (KeyValuePair<TrackerGraphNode, PositionRotationTransform> pair in nextGraphNodes)
        {
            if (!seenNodes.Contains(pair.Key))
            {
                seenNodes.Add(pair.Key);

                PositionRotationTransform nextTransform = currentPositionRotation.AddTo(pair.Value);
                Gizmos.DrawSphere(nextTransform.position, radius);

                pair.Key.DrawGraphTail(nextTransform, radius, seenNodes);
            }
        }
    }
}

