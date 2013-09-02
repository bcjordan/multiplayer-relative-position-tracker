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

    [RPC]
    void HelloRPC(string hello)
    {
        Debug.Log(hello);
    }
}
