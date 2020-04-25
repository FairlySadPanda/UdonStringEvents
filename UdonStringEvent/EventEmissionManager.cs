// Emit events!
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

///<Summary>The point-of-contact for all users of the UdonStringEvent system.</Summary>
public class EventEmissionManager : UdonSharpBehaviour
{
    ///<Summary>The receiver of events in this event system.</Summary>
    public EventReceiver receiver;

    private int clock;
    private string displayName;
    private bool gotEmitter;

    public void Start()
    {
        clock = 0;
        displayName = "";
        gotEmitter = false;

        if (Networking.LocalPlayer != null)
        {
            displayName = Networking.LocalPlayer.displayName;
        }

    }

    public void Update()
    {
        if (gotEmitter == false)
        {
            GetEmitter();
            //TODO: If gotemitter is false for a long time (5 seconds?) panic and alert the receiver that a reallocation has to happen.
        }
    }

    ///<Summary>Send a string event to all other players in the world.</Summary>
    public void SendEvent(string eventName, string eventPayload)
    {
        // An extension to this system might queue up events in the instance that we want to make sure they're sent on world load.
        // For the time being a good modification might be to change SendEvent to return a bool.

        // This can be bad in two ways: either the return is null or the return is not owned.
        GameObject emitter = receiver.GetEmitter(displayName);
        if (emitter == null)
        {
            Debug.Log("emitter was null: could not handle " + eventName + " event");
            gotEmitter = false;
            return;
        }

        if (!Networking.IsOwner(emitter.gameObject))
        {
            Debug.Log("emitter not owned by player: could not handle " + eventName + " event");
            gotEmitter = false;
            return;
        }

        emitter.GetComponent<EventEmitter>().SetNewEvent(eventName, eventPayload);
    }

    private void GetEmitter()
    {
        GameObject emitter = receiver.GetEmitter(displayName);

        if (emitter != null)
        {
            Debug.Log("Emitter object has arrived in our care.");
            Networking.SetOwner(Networking.LocalPlayer, emitter);
            gotEmitter = true;
        }
    }
}