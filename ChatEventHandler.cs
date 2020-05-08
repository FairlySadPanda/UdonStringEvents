// Example Handler
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

/// <Summary>An example handler for events.</Summary>
public class ChatEventHandler : UdonSharpBehaviour
{
    public UdonLogger logger;

    private string characterName;
    private string newEvent;

    public void Handle()
    {
        // As it stands, hard-code your events in this function.
        // This is pretty basic. Once maps and lists exist in Udon, this can be improved.
        string[] e = newEvent.Split(',');
        Debug.Log("Got an event named " + e[0] + " with payload " + newEvent);
        switch (e[0])
        {
            case "ChatMessage":
                string message = characterName + ": " + e[1];
                message = message.Replace("|", ",");
                logger.Notice(message);
                break;
            default:
                logger.Notice("Got an event named " + e[0] + " but didn't know what to do with it.");
                break;
        }
    }
}