// On activation, emit a Hello World event to the Logger.
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class EnableLoggerButton : UdonSharpBehaviour
{
    public UdonLogger logger;

    private void OnMouseDown()
    {
        Debug.Log("Hit!");
        if (Networking.LocalPlayer == null)
        {
            logger.Toggle();
        }
    }

    public override void Interact()
    {
        logger.Toggle();
    }

    public void Toggle()
    {
        Debug.Log("Toggle!");
        logger.Toggle();
    }
}
