using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

public class ImmobilizeToggleKey : UdonSharpBehaviour
{
    public KeyboardManager manager;
    public Image buttonImage;
    private bool activated;

    public void PressKey()
    {
        if (activated)
        {
            Deactivate();
            return;
        }

        Activate();
    }

    public void Activate()
    {
        if (Networking.LocalPlayer != null)
        {
            Networking.LocalPlayer.Immobilize(true);
        }

        buttonImage.color = Color.blue;
        activated = true;
    }

    public void Deactivate()
    {
        if (Networking.LocalPlayer != null)
        {
            Networking.LocalPlayer.Immobilize(false);
        }

        buttonImage.color = Color.white;
        activated = false;
    }
}