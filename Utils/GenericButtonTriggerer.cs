
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GenericButtonTriggerer : UdonSharpBehaviour
{
    void Update()
    {
        var player = Networking.LocalPlayer;
        if (player == null)
        {
            return;
        }

        var handLoc = player.GetBonePosition(HumanBodyBones.LeftIndexDistal);
        if (handLoc == null)
        {
            handLoc = player.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand).position;
        }

        transform.position = handLoc;
    }
}
