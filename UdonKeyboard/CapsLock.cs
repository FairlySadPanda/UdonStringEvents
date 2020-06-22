using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

public class CapsLockKey : UdonSharpBehaviour
{
    public KeyboardManager manager;
    public Image buttonImage;
    private bool activated;

    public void PressKey()
    {
        if (activated)
        {
            manager.CapsOff();
            activated = false;
            buttonImage.color = Color.white;
            return;
        }

        manager.CapsOn();
        activated = true;
        buttonImage.color = Color.blue;
    }

    public void Deactivate()
    {
        manager.CapsOff();
        activated = false;
        buttonImage.color = Color.white;
    }
}