using UnityEngine;
using System.Collections;

public class TrackerNetworkingManager : Manager
{
    void Update()
    {
        if (uLink.Network.peerType == uLink.NetworkPeerType.Disconnected)
        {
            return;
        }

        Debug.Log("Sending call");
        networkView.RPC("HelloRPC", uLink.RPCMode.Others, "Hello uLink. Timestamp = " + System.DateTime.Now.Ticks);
    }

    public void PlaceRelativeSpaceObject(string trackerID, PositionRotationTransform positionRotationTransform, int objectID)
    {
        networkView.RPC("AddRelativeSpaceObject", uLink.RPCMode.All, trackerID, positionRotationTransform, objectID);
    }

    [RPC]
    void AddRelativeSpaceObject(string trackerID, PositionRotationTransform positionRotationTransform, int objectID)
    {
        Managers.GetManager<RelativeSpacePlacedObjectManager>().PlaceObjectAtGraphNode(trackerID, positionRotationTransform, objectID);
    }

    [RPC]
    void HelloRPC(string hello)
    {
        Debug.Log(hello);
    }
}
