// Emit events!
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class EventEmissionManager : UdonSharpBehaviour
{
    public EventReceiver receiver;
    private EventEmitter emitter;

    private int clock;

    public void Start()
    {
        clock = 0;
    }

    public void Update()
    {
        if (emitter == null)
        {
            string displayName = "";
            GameObject ownedEmitter = null;

            if (Networking.LocalPlayer != null)
            {
                displayName = Networking.LocalPlayer.displayName;
            }

            ownedEmitter = receiver.GetEmitter(displayName);

            if (ownedEmitter != null)
            {
                Debug.Log("Emitter object has arrived in our care.");
                Networking.SetOwner(Networking.LocalPlayer, ownedEmitter);
                emitter = (EventEmitter)ownedEmitter.GetComponent(typeof(UdonBehaviour));
            }
        }
    }

    public void SendEvent(string eventName, string eventPayload)
    {
        if (emitter == null)
        {
            Debug.Log("emitter was null, could not handle " + eventName + " event");
            return;
        }

        emitter.SetNewEvent(eventName, eventPayload);
    }

    public void SendChatMessage(string message)
    {
        SendEvent("ChatMessage", message);
    }
}