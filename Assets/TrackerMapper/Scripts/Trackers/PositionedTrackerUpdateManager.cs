using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PositionedTrackerUpdateManager : Manager
{
    public delegate void TrackerUpdateEventHandler(string trackerID, PositionRotationTransform trackerAbsolutePositionRotation);
    public event TrackerUpdateEventHandler TrackerPositionUpdate;

    void Update()
    {
        DispatchTrackerPositions();
    }

    void DispatchTrackerPositions()
    {
        if (TrackerPositionUpdate == null)
        {
            return;
        }

        TrackerMapManager trackerMapManager = Managers.GetManager<TrackerMapManager>();
        VisibleTrackerManager visibleTrackerManager = Managers.GetManager<VisibleTrackerManager>();

        Tracker firstTracker = visibleTrackerManager.TryGetFirstVisibleTracker();
        if (firstTracker == null)
        {
            return;
        }

        PositionRotationTransform startingAbsoluteTransform = new PositionRotationTransform(firstTracker.transform);

        foreach (KeyValuePair<PositionRotationTransform, TrackerGraphNode> pair in trackerMapManager.GetNodeForTracker(firstTracker).EachAbsoluteTrackerNode(startingAbsoluteTransform))
        {
            TrackerPositionUpdate(pair.Value.uniqueTrackerID, pair.Key);
        }
    }
}
