using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

///<Summary>A Logger for Udon. Attaches to a player's hand if they're in VR, or sits at some point in space if they're in desktop mode.</Summary>
public class LoggerButtons : UdonSharpBehaviour
{
    public void Update()
    {
        VRCPlayerApi player = Networking.LocalPlayer;
        if (player != null && player.IsUserInVR())
        {
            gameObject.transform.position = player.GetBonePosition(HumanBodyBones.RightHand);
            gameObject.transform.rotation = player.GetBoneRotation(HumanBodyBones.RightHand);
        }
    }

}
