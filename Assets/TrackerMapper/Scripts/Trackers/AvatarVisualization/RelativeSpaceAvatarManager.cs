using UnityEngine;
using System.Collections;

public class RelativeSpaceAvatarManager : Manager
{
    public GameObject avatarPrefab;

    private string avatarTrackerID;
    private PositionRotationTransform avatarPositionRotationTransform;

    private GameObject avatarInstance;

    public void UpdateAvatarPosition(string trackerID, PositionRotationTransform positionRotationTransform)
    {
        avatarTrackerID = trackerID;
        avatarPositionRotationTransform = positionRotationTransform;
    }

    void Start()
    {
        Managers.GetManager<PositionedTrackerUpdateManager>().TrackerPositionUpdate += OnTrackerPosition;
    }

    void Destroy()
    {
        Managers.GetManager<PositionedTrackerUpdateManager>().TrackerPositionUpdate -= OnTrackerPosition;
    }

    void Update()
    {
        UpdateSharedRelativeTransform();
    }

    void UpdateSharedRelativeTransform()
    {
        Tracker firstVisibleTracker = Managers.GetManager<VisibleTrackerManager>().TryGetFirstVisibleTracker();
        if (firstVisibleTracker == null)
        {
            return;
        }

        GameObject cameraObject = Managers.GetManager<VisionManager>().cameraToUse.gameObject;
        PositionRotationTransform trackerToCamera = PositionRotationTransform.FromTo(firstVisibleTracker.gameObject, cameraObject);
        Managers.GetManager<TrackerNetworkingManager>().ShareAvatarPosition(firstVisibleTracker.uniqueID, trackerToCamera);
    }

    void OnTrackerPosition(string trackerID, PositionRotationTransform trackerAbsolutePositionRotation)
    {
        if (trackerID != avatarTrackerID)
        {
            return;
        }

        PositionRotationTransform absolutePositionRotationTransform = avatarPositionRotationTransform.AddTo(trackerAbsolutePositionRotation);

        if (avatarInstance == null)
        {
            avatarInstance = (GameObject)Instantiate(avatarPrefab, absolutePositionRotationTransform.position, Quaternion.Euler(absolutePositionRotationTransform.rotation));
        }

        avatarInstance.transform.position = absolutePositionRotationTransform.position;
        Quaternion newAvatarQuaternion = avatarInstance.transform.rotation;
        newAvatarQuaternion.eulerAngles = absolutePositionRotationTransform.rotation;
        avatarInstance.transform.rotation = newAvatarQuaternion;
    }
}
