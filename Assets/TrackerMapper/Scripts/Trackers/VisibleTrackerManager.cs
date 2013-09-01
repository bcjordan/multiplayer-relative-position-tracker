using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisibleTrackerManager : Manager
{
    public float debugGizmoRadius;
    public Color debugGizmoColor;

    private IList<Tracker> _trackers = new List<Tracker>();

    public delegate void SeeingMultipleTrackersEventHandler(IList<Tracker> trackers);
    public event SeeingMultipleTrackersEventHandler SeeMultipleTrackers;

    void OnGUI()
    {
        GUI.Label(new Rect(140, 60, 350, 40), _trackers.Count.ToString() +
                  " visible tracker" + (_trackers.Count == 1 ? "" : "s"));
    }
    
    void OnDrawGizmos()
    {
        foreach (Tracker tracker in _trackers)
        {
            Gizmos.DrawSphere(tracker.transform.position, debugGizmoRadius);
        }
    }

    void Update()
    {
        if (_trackers.Count >= 2)
        {
            if (SeeMultipleTrackers != null)
            {
                SeeMultipleTrackers(_trackers);
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
        if (!_trackers.Contains(tracker))
        {
            _trackers.Add(tracker);
        }
    }

    public void SetInvisible(Tracker tracker)
    {
        _trackers.Remove(tracker);
    }

    public bool HasVisibleTrackers()
    {
        return _trackers.Count > 0;
    }

    public Tracker TryGetFirstVisibleTracker()
    {
        if (_trackers.Count > 0)
        {
            return _trackers[0];
        }

        return null;
    }
}
