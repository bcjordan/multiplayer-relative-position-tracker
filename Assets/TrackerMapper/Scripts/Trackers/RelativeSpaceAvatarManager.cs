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
        Tracker firstVisibleTracker = Managers.GetManager<VisibleTrackerManager>().TryGetFirstVisibleTracker();
        if (firstVisibleTracker == null)
        {
            return;
        }

        PositionRotationTransform trackerToCamera = RelativePositionRotationTransform(firstVisibleTracker.gameObject, Managers.GetManager<VisionManager>().cameraToUse);
        Managers.GetManager<TrackerNetworkingManager>().ShareAvatarPosition(firstVisibleTracker.uniqueID, trackerToCamera);
    }

    PositionRotationTransform RelativePositionRotationTransform(GameObject fromTracker, GameObject toAvatar)
    {
        PositionRotationTransform relativePositionRotationTransform = new PositionRotationTransform();
        relativePositionRotationTransform.position = toAvatar.transform.position - fromTracker.transform.position;
        relativePositionRotationTransform.rotation = toAvatar.transform.rotation.eulerAngles - fromTracker.transform.rotation.eulerAngles;
        return relativePositionRotationTransform;
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
