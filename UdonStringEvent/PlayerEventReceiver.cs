using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

///<Summary>PlayerEventReceiver receives events from network players to the host.</Summary>
public class PlayerEventReceiver : UdonSharpBehaviour
{
    /// <Summary>An array of all Emitters in the system.</Summary>
    public PlayerEventEmitter[] emitters;

    private string characterName;

    public void Start()
    {
        var player = Networking.LocalPlayer;
        if (player == null)
        {
            characterName = "bob0";
            return;
        }

        characterName = player.displayName;
    }

    public void Update()
    {
        for (int i = 0; i < emitters.Length; i++)
        {
            // UdonSharp limitation - this can be refactored once the generic is handled correctly.
            PlayerEventEmitter emitter = emitters[i];
            if (emitter.GetCharacterName() == characterName)
            {
                string newEv = emitter.GetNewEvent();
                // GetNewEvent returns either a nil event (empty string) or an event.
                if (newEv != "")
                {
                    HandleUpdate(emitter.GetCharacterName(), newEv);
                }
                break;
            }
        }
    }

    /// <Summary>Retrieve an Emitter if one is found that belongs to the provied character name, or null if one isn't.</Summary>
    public PlayerEventEmitter GetEmitter(string characterName)
    {
        for (int i = 0; i < emitters.Length; i++)
        {
            if (emitters[i].GetCharacterName() == characterName)
            {
                Debug.Log("Returning emitter with index " + i + " to requester");
                return emitters[i];
            }
        }

        Debug.Log("WARNING: No player event emitter found for character " + characterName);
        return null;
    }

    public void SetEmitter(string characterName, int index)
    {
        Debug.Log("Setting " + characterName + " to emitter at index " + index);
        emitters[index].SetCharacterName(characterName);
    }

    public void FreeAllEmitters()
    {
        for (int i = 0; i < emitters.Length; i++)
        {
            var emitter = emitters[i];
            emitter.Clear();
        }
    }

    private void HandleUpdate(string characterName, string eventString)
    {
        // As it stands, hard-code your events in this function.
        // This is pretty basic. Once maps and lists exist in Udon, this can be improved.
        string[] e = eventString.Split(',');
        Debug.Log("Got an event for player " + characterName + " named " + e[0] + " with payload " + eventString);
        switch (e[0])
        {
            default:
                Debug.Log("Got an event called " + e[0] + " but I don't know what to do with it.");
                break;
        }
    }
}