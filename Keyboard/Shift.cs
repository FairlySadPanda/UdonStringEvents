using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

public class ShiftKey : UdonSharpBehaviour
{
    public KeyboardManager manager;
    public Image buttonImage;
    private bool toggle;

    public void PressKey()
    {
        if (toggle)
        {
            manager.ShiftOff();
            toggle = false;
            buttonImage.color = Color.white;
            return;
        }

        manager.ShiftOn();
        toggle = true;
        buttonImage.color = Color.blue;
    }
}