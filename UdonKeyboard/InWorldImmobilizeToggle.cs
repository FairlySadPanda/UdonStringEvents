using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

public class InWorldImmobilizeToggleKey : UdonSharpBehaviour
{
    public InWorldKeyboardManager manager;
    public KeyEventListener keyEventListener;
    public Image buttonImage;
    private bool toggle;

    public void PressKey()
    {
        if (Networking.LocalPlayer != null)
        {
            if (toggle)
            {
                Deactivate();
                return;
            }

            Networking.LocalPlayer.Immobilize(true);
            buttonImage.color = Color.blue;
            toggle = true;
        }
    }

    public void Deactivate()
    {
        if (Networking.LocalPlayer != null)
        {
            Networking.LocalPlayer.Immobilize(false);
        }

        buttonImage.color = Color.white;
        toggle = false;
    }
}