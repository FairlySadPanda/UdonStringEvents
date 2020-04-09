using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

class EventReceiver : UdonSharpBehaviour
{
    public GameObject[] emitters;

    public void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            EventEmitter emitter = ((EventEmitter)emitters[i].GetComponent(typeof(UdonBehaviour)));
            string newEv = emitter.GetNewEvent();
            if (newEv != "")
            {
                HandleUpdate(newEv);
            }
        }
    }

    public GameObject GetEmitter(string characterName)
    {
        for (int i = 0; i < 4; i++)
        {
            if (emitters[i].GetComponent<EventEmitter>().GetCharacterName() == characterName)
            {
                Debug.Log("Returning emitter with index " + i + " to requester");
                return emitters[i];
            }
        }

        return null;
    }

    private void HandleUpdate(string eventString)
    {
        string[] e = eventString.Split(',');
        switch (e[0])
        {
            case "DebugLog":
                Debug.Log(e[1]);
                break;
        }
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (Networking.IsOwner(gameObject))
        {
            GameObject emitter = GetEmitter("");
            ((EventEmitter)emitter.GetComponent(typeof(UdonBehaviour))).SetCharacterName(player.displayName);
        }

    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (Networking.IsOwner(gameObject))
        {
            GameObject emitter = GetEmitter(player.displayName);
            ((EventEmitter)emitter.GetComponent(typeof(UdonBehaviour))).SetCharacterName("");
        }
    }
}