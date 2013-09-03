using UnityEngine;
using System.Collections;

public class TrackerNetworkingManager : Manager
{
    public void PlaceRelativeSpaceObject(string trackerID, PositionRotationTransform positionRotationTransform, int objectID)
    {
        if (uLink.Network.peerType == uLink.NetworkPeerType.Disconnected)
        {
            return;
        }

        networkView.RPC("AddRelativeSpaceObject", uLink.RPCMode.All, trackerID, positionRotationTransform, objectID);
    }

    [RPC]
    void AddRelativeSpaceObject(string trackerID, PositionRotationTransform positionRotationTransform, int objectID)
    {
        Managers.GetManager<RelativeSpacePlacedObjectManager>().PlaceObjectAtGraphNode(trackerID, positionRotationTransform, objectID);
    }

    public void ShareAvatarPosition(string trackerID, PositionRotationTransform positionRotationTransform)
    {
        if (uLink.Network.peerType == uLink.NetworkPeerType.Disconnected)
        {
            return;
        }

        networkView.RPC("UpdateRelativeSpaceAvatar", uLink.RPCMode.Others, trackerID, positionRotationTransform);
    }

    [RPC]
    void UpdateRelativeSpaceAvatar(string trackerID, PositionRotationTransform positionRotationTransform)
    {
        Managers.GetManager<RelativeSpaceAvatarManager>().UpdateAvatarPosition(trackerID, positionRotationTransform);
    }
}
