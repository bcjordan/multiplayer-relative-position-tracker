using UnityEngine;
using System.Collections;

public class Tracker : MonoBehaviour
{
    void Update()
    {
        bool visibleFromCamera = Managers.GetManager<CameraManager>().IsVisible(gameObject);
        Managers.GetManager<VisibleTrackerManager>().SetVisibility(this, visibleFromCamera);
    }
}
