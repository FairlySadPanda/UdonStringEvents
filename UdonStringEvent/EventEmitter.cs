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
    // Designates who owns this Emitter.
    [UdonSynced]
    private string characterName;

    // If newEvent doesn't match oldEvent, then a new event has been emitted.
    [UdonSynced]
    private string newEvent;
    private string oldEvent;

    private int clock;

    public void Start()
    {
        characterName = "";
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

        oldEvent = newEvent;
        return newEvent;
    }

    /// <Summary>Set a new event to be emitted.</Summary>
    public void SetNewEvent(string eventName, string payload)
    {
        // Leave this debug log on unless you have a lot of spam. It'll help with debugging.
        Debug.Log("Sending event: " + eventName + ": " + payload);
        newEvent = eventName + "," + payload + "," + clock;

        clock++;
    }

    /// <Summary>Get the name of the character that owns this emitter.</Summary>
    public string GetCharacterName()
    {
        // Note that for Udon, even though we *can* get variables directly, don't. It's cleaner to rely on functions.
        return characterName;
    }

    /// <Summary>Set the owner of the emitter to the provided name.</Summary>
    public void SetCharacterName(string c)
    {
        if (Networking.IsOwner(gameObject))
        {
            characterName = c;
        }
    }
}