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
        handler.SetProgramVariable("characterName", characterName);
        handler.SetProgramVariable("newEvent", eventString);
        handler.SendCustomEvent("Handle");
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