// Example Handler
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

/// <Summary>The handler of events.</Summary>
public class EventHandler : UdonSharpBehaviour
{
    // The player who sent the event.
    public int playerID;

    // The event that was sent.
    public string newEvent;

    public void Handle()
    {
        string[] e = newEvent.Split(',');
        Debug.Log("Got an event named " + e[0] + " with payload " + newEvent);

        // Event type switch. Contains both state and view events. (Too many synced strings currently will break eventually.)
        // TODO: Replace with a true event bus when network event payloads are added.
        switch (e[0])
        {
            default:
                Debug.Log("Got an event named " + e[0] + " but didn't know what to do with it.");
                break;
        }
    }
}