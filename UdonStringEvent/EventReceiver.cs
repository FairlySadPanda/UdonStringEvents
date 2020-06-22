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
    private EventEmitter emitter;

    public void Start()
    {
        clock = 0;
    }

    public void Update()
    {
        if (Networking.LocalPlayer == null)
        {
            return;
        }

        if (emitter == null)
        {
            emitter = GetEmitter(Networking.LocalPlayer.playerId);
        }

        foreach (var emitter in emitters)
        {
            string newEv = emitter.GetNewEvent();
            // GetNewEvent returns either a nil event (empty string) or an event.
            if (newEv != "")
            {
                HandleUpdate(Networking.GetOwner(emitter.gameObject).playerId, newEv);
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

    private EventEmitter GetEmitter(int id)
    {
        foreach (var possibleEmitter in emitters)
        {
            // Don't re-allocate our own emitter away.
            if (Networking.GetOwner(possibleEmitter.gameObject).playerId == id && possibleEmitter != emitter)
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
            Networking.SetOwner(player, GetEmitter(Networking.LocalPlayer.playerId).gameObject);
        }
    }


    private void HandleUpdate(int playerID, string eventString)
    {
        handler.SetProgramVariable("playerID", playerID);
        handler.SetProgramVariable("newEvent", eventString);
        handler.SendCustomEvent("Handle");
    }
}