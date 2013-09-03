using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisibleTrackerManager : Manager
{
    public float debugGizmoRadius;
    public Color debugGizmoColor;

    private IList<Tracker> trackers = new List<Tracker>();

    public delegate void SeeingMultipleTrackersEventHandler(IList<Tracker> trackers);
    public event SeeingMultipleTrackersEventHandler SeeMultipleTrackers;

    void OnGUI()
    {
        GUI.Label(new Rect(140, 100, 350, 40), trackers.Count.ToString() +
                  " visible tracker" + (trackers.Count == 1 ? "" : "s"));
    }
    
    void OnDrawGizmos()
    {
        foreach (Tracker tracker in trackers)
        {
            Gizmos.DrawSphere(tracker.transform.position, debugGizmoRadius);
        }
    }

    void Update()
    {
        if (trackers.Count >= 2)
        {
            if (SeeMultipleTrackers != null)
            {
                SeeMultipleTrackers(trackers);
            }
        }
    }

    public void SetVisibility(Tracker tracker, bool visible)
    {
        if (visible)
        {
            SetVisible(tracker);
        }
        else
        {
            SetInvisible(tracker);
        }
    }

    public void SetVisible(Tracker tracker)
    {
        if (!trackers.Contains(tracker))
        {
            trackers.Add(tracker);
        }
    }

    public void SetInvisible(Tracker tracker)
    {
        trackers.Remove(tracker);
    }

    public bool HasVisibleTrackers()
    {
        return trackers.Count > 0;
    }

    public Tracker TryGetFirstVisibleTracker()
    {
        if (trackers.Count > 0)
        {
            return trackers[0];
        }

        return null;
    }
}
