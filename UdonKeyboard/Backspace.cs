using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

public class BackspaceKey : UdonSharpBehaviour
{
    public KeyboardManager manager;

    public void PressKey()
    {
        manager.Backspace();
    }
}