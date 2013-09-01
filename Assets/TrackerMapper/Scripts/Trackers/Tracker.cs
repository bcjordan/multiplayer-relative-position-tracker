using UnityEngine;
using System.Collections;

public class Tracker : MonoBehaviour
{
    void Update()
    {
        bool visibleFromCamera = Managers.GetManager<VisionManager>().IsVisible(gameObject);
        Managers.GetManager<VisibleTrackerManager>().SetVisibility(this, visibleFromCamera);
    }
}
