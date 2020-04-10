// Emit events!
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class EventEmitter : UdonSharpBehaviour
{
    [UdonSynced]
    private string characterName;

    [UdonSynced]
    private string newEvent;
    private string oldEvent;

    public void Start()
    {
        characterName = "";
        newEvent = "";
        oldEvent = "";
    }

    public string GetNewEvent()
    {
        if (newEvent == oldEvent)
        {
            return "";
        }

        oldEvent = newEvent;
        return newEvent;
    }

    public void SetNewEvent(string eventName, string payload)
    {
        Debug.Log("Sending event: " + eventName + ": " + payload);
        newEvent = eventName + "," + payload;
    }

    public string GetCharacterName()
    {
        return characterName;
    }

    public void SetCharacterName(string c)
    {
        characterName = c;
        Debug.Log("Emitter's Character Name has been set to " + c);
    }
}