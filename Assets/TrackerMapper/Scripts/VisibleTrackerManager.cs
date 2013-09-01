using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisibleTrackerManager : Manager
{
    public float debugGizmoRadius;

    private IList<Tracker> _trackers = new List<Tracker>();

    void OnGUI()
    {
        GUI.Label(new Rect(140, 60, 350, 40), _trackers.Count.ToString() +
                  " visible tracker" + (_trackers.Count == 1 ? "" : "s"));
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

    void OnDrawGizmos()
    {
        foreach (Tracker tracker in _trackers)
        {
            Gizmos.DrawSphere(tracker.transform.position, debugGizmoRadius);
        }
    }
}
