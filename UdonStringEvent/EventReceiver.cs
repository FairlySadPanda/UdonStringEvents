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
    public EventEmitter[] emitters;
    public UdonBehaviour handler;

    private int clock;
    private string displayName;
    private EventEmitter emitter;

    public void Start()
    {
        clock = 0;
        displayName = "";

        if (Networking.LocalPlayer != null)
        {
            displayName = Networking.LocalPlayer.displayName;
        }
    }

    void Update()
    {
        if (Networking.LocalPlayer == null)
        {
            return;
        }

        if (emitter == null)
        {
            emitter = GetEmitter(displayName);

            if (emitter != null)
            {
                Debug.Log("Emitter object has arrived in our care.");
                Networking.SetOwner(Networking.LocalPlayer, emitter.gameObject);
            }

            //TODO: If gotemitter is false for a long time (5 seconds?) panic and alert the receiver that a reallocation has to happen.
        }

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

    ///<Summary>Send a string event to all other players in the world.</Summary>
    public void SendEvent(string eventName, string eventPayload)
    {
        // An extension to this system might queue up events in the instance that we want to make sure they're sent on world load.
        // For the time being a good modification might be to change SendEvent to return a bool.

        // This can be bad in two ways: either the return is null or the return is not owned.
        if (emitter == null)
        {
            Debug.LogError("emitter was null: could not handle " + eventName + " event");
            return;
        }

        if (!Networking.IsOwner(emitter.gameObject))
        {
            Debug.LogError("emitter not owned by player: could not handle " + eventName + " event");
            return;
        }

        emitter.SetNewEvent(eventName, eventPayload);
    }

    private EventEmitter GetEmitter(string name)
    {
        foreach (var possibleEmitter in emitters)
        {
            if (possibleEmitter.GetCharacterName() == name)
            {
                return possibleEmitter;
            }
        }

        return null;
    }

    /// <Summary>Get an empty emitter and assign it to the new player.</Summary>
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (Networking.IsOwner(gameObject))
        {
            GetEmitter("").SetCharacter(player.playerId);
        }
    }

    /// <Summary>Get the player's emitter and assign it to nobody.</Summary>
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (Networking.IsOwner(gameObject))
        {
            var emitter = GetEmitter(player.displayName);
            if (emitter != null)
            {
                emitter.SetCharacter(-1);
            }
        }
    }

    private void HandleUpdate(string characterName, string eventString)
    {
        handler.SetProgramVariable("characterName", characterName);
        handler.SetProgramVariable("newEvent", eventString);
        handler.SendCustomEvent("Handle");
    }
}