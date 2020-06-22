// Emit events!
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

/// <Summary>A Behaviour that can emit events. An Emitter will check if this Emitter has emitted each update.</Summary>
public class EventEmitter : UdonSharpBehaviour
{
    // If newEvent doesn't match oldEvent, then a new event has been emitted.
    [UdonSynced]
    private string newEvent;
    private string oldEvent;
    private int ownerID;

    private int clock;

    public void Start()
    {
        newEvent = "";
        oldEvent = "";
        clock = 0;
    }

    /// <Summary>If a new event has been emitted, return the event. Otherwise return an empty string.</Summary>
    public string GetNewEvent()
    {
        if (newEvent == oldEvent)
        {
            return "";
        }

        if (newEvent.IndexOf(",") == -1)
        {
            return "";
        }

        ownerID = int.Parse(newEvent.Substring(0, newEvent.IndexOf(",")));
        oldEvent = newEvent;

        return newEvent.Substring(newEvent.IndexOf(",") + 1);
    }

    /// <Summary>Set a new event to be emitted.</Summary>
    public void SetNewEvent(string eventName, string payload)
    {
        newEvent = $"{ownerID},{eventName},{payload},{clock}";
        clock++;
    }
}