using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class TrackerGraphNode
{
    private IDictionary<TrackerGraphNode, PositionRotationTransform> nextGraphNodes = new Dictionary<TrackerGraphNode, PositionRotationTransform>();
    
    public bool ContainsChild(TrackerGraphNode graphNode)
    {
        return nextGraphNodes.ContainsKey(graphNode);
    }

    public void AddChild(TrackerGraphNode childGraphNode, PositionRotationTransform positionRotationTransform)
    {
        nextGraphNodes[childGraphNode] = positionRotationTransform;
    }

    public IEnumerable<PositionRotationTransform> EachAbsoluteTransformInGraph(PositionRotationTransform startingPositionRotation)
    {
        return EachAbsoluteTransformInGraphExcluding(startingPositionRotation, new List<TrackerGraphNode>());
    }

    public IEnumerable<PositionRotationTransform> EachAbsoluteTransformInGraphExcluding(PositionRotationTransform currentPositionRotation, IList<TrackerGraphNode> excludingNodes)
    {
        if (excludingNodes.Contains(this))
        {
            yield break;
        }

        excludingNodes.Add(this);
        yield return currentPositionRotation;

        foreach (KeyValuePair<TrackerGraphNode, PositionRotationTransform> pair in nextGraphNodes)
        {
            PositionRotationTransform nextTransform = currentPositionRotation.AddTo(pair.Value);

            foreach (PositionRotationTransform positionRotationTransform in pair.Key.EachAbsoluteTransformInGraphExcluding(nextTransform, excludingNodes))
            {
                yield return positionRotationTransform;
            }
        }
    }
}

