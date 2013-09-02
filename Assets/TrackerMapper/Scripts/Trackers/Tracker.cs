using UnityEngine;
using System.Collections;

public class Tracker : MonoBehaviour
{
    public string uniqueID;

    void Update()
    {
        bool visibleFromCamera = Managers.GetManager<VisionManager>().IsVisible(gameObject);
        Managers.GetManager<VisibleTrackerManager>().SetVisibility(this, visibleFromCamera);
    }
}
