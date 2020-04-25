using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

///<Summary>EventReceiver is the core of the UdonStringEvent system. One must exist per system.</Summary>
public class EventReceiver : UdonSharpBehaviour
{
    /// <Summary>An array of all Emitters in the system.</Summary>
    public GameObject[] emitters;

    /// <Summary>A logger that events can be output to. This is optional.</Summary>
    public UdonLogger logger;

    public GameObject master;

    [UdonSynced]
    private bool gameIsNotInProgress;
    private bool lateJoiner;

    public void Start()
    {
        if (Networking.LocalPlayer != null && Networking.LocalPlayer.IsOwner(gameObject))
        {
            gameIsNotInProgress = true;
        }
    }

    public void Update()
    {
        for (int i = 0; i < emitters.Length; i++)
        {
            // UdonSharp limitation - this can be refactored once the generic is handled correctly.
            EventEmitter emitter = ((EventEmitter)emitters[i].GetComponent(typeof(UdonBehaviour)));
            string newEv = emitter.GetNewEvent();

            // GetNewEvent returns either a nil event (empty string) or an event.
            if (newEv != "")
            {
                HandleUpdate(emitter.GetCharacterName(), newEv);
            }
        }
    }

    /// <Summary>Retrieve an Emitter if one is found that belongs to the provied character name, or null if one isn't.</Summary>
    public GameObject GetEmitter(string characterName)
    {
        for (int i = 0; i < emitters.Length; i++)
        {
            if (emitters[i].GetComponent<EventEmitter>().GetCharacterName() == characterName)
            {
                Debug.Log("Returning emitter with index " + i + " to requester");
                return emitters[i];
            }
        }

        return null;
    }

    private void HandleUpdate(string characterName, string eventString)
    {
        bool ownerOfGame = true;
        if (Networking.LocalPlayer != null)
        {
            ownerOfGame = Networking.LocalPlayer.IsOwner(master);
        }

        // As it stands, hard-code your events in this function.
        // This is pretty basic. Once maps and lists exist in Udon, this can be improved.
        string[] e = eventString.Split(',');
        Debug.Log("Got an event named " + e[0] + " with payload " + eventString);
        switch (e[0])
        {
            case "ChatMessage":
                string message = characterName + ": " + e[1];
                message = message.Replace("|", ",");
                Debug.Log("Notice: " + message);
                logger.Notice(message);
                break;
            default:
                logger.Notice("Got an event named " + e[0] + " but didn't know what to do with it.");
                break;
        }
    }

    /// <Summary>Get an empty emitter and assign it to the new player.</Summary>
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (Networking.IsOwner(gameObject))
        {
            GameObject emitter = GetEmitter("");
            ((EventEmitter)emitter.GetComponent(typeof(UdonBehaviour))).SetCharacterName(player.displayName);
        }
    }

    /// <Summary>Get the player's emitter and assign it to nobody.</Summary>
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (Networking.IsOwner(gameObject))
        {
            GameObject emitter = GetEmitter(player.displayName);
            ((EventEmitter)emitter.GetComponent(typeof(UdonBehaviour))).SetCharacterName("");
        }
    }
}