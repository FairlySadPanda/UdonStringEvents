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
    public UdonBehaviour handler;

    void Update()
    {
        foreach (var emitter in emitters)
        {
            string newEv = emitter.GetNewEvent();
            // GetNewEvent returns either a nil event (empty string) or an event.
            if (newEv != "")
            {
                HandleUpdate(emitter.GetCharacterName(), newEv);
            }
        }
    }

    /// <Summary>Get an empty emitter and assign it to the new player.</Summary>
    override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (Networking.IsOwner(gameObject))
        {
            GetEmitter("").SetCharacterName(player.displayName);
        }
    }

    /// <Summary>Get the player's emitter and assign it to nobody.</Summary>
    override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (Networking.IsOwner(gameObject))
        {
            GetEmitter(player.displayName).SetCharacterName("");
        }
    }

    private void HandleUpdate(string characterName, string eventString)
    {
        handler.SetProgramVariable("characterName", characterName);
        handler.SetProgramVariable("newEvent", eventString);
        handler.SendCustomEvent("Handle");
    }


    /// <Summary>Retrieve an Emitter if one is found that belongs to the provied character name, or null if one isn't.</Summary>
    private EventEmitter GetEmitter(string characterName)
    {
        foreach (var emitter in emitters)
        {
            if (emitter.GetCharacterName() == characterName)
            {
                return emitter;
            }
        }

        return null;
    }
}