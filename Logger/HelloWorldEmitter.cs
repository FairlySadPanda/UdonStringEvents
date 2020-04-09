// On activation, emit a Hello World event to the Logger.
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class HelloWorldEmitter : UdonSharpBehaviour
{
    public DiceRoller roller;

    public void ActivateButton()
    {
        Roll1D20();
    }

    public void Roll1D20()
    {
        roller.SendCustomNetworkEvent(NetworkEventTarget.Owner, "Roll1D20");
    }
}
