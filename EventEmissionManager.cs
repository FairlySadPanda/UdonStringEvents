// Emit events!
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

class EventEmissionManager : UdonSharpBehaviour
{
    public EventReceiver receiver;
    private EventEmitter emitter;
    private bool awaitClaim;

    private int clock;

    public void Start()
    {
        clock = 0;
        awaitClaim = true;
    }

    public void FixedUpdate()
    {
        if (awaitClaim)
        {
            GameObject ownedEmitter = receiver.GetEmitter(Networking.LocalPlayer.displayName);
            if (ownedEmitter != null)
            {
                Debug.Log("Emitter object has arrived in our care.");
                Networking.SetOwner(Networking.LocalPlayer, ownedEmitter);
                emitter = (EventEmitter)ownedEmitter.GetComponent(typeof(UdonBehaviour));
                awaitClaim = false;
            }
        }
    }

    public void SendEvent(string eventName, string eventPayload)
    {
        if (emitter == null)
        {
            Debug.Log("emitter was null, could not handle " + eventName + "event");
            return;
        }

        emitter.SetNewEvent(eventName, eventPayload);
    }

    public void SendHelloWorld()
    {
        SendEvent("DebugLog", "Hello World! " + clock++);
    }
}