using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class KeyEventListener : UdonSharpBehaviour
{
    public KeyboardManager keyboard;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (Networking.LocalPlayer?.IsUserInVR())  
                return;
                
            keyboard.Toggle();
        }
    }
}
