using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class KeyEventListener : UdonSharpBehaviour
{
    public KeyboardManager keyboard;
    public UdonLogger logger;

    private void Update()
    {
        var player = Networking.LocalPlayer;
        if (player != null && player.IsUserInVR())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            keyboard.Toggle();
            logger.Toggle();
        }
    }
}