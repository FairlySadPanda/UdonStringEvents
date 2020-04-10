using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

public class ChatController : UdonSharpBehaviour
{
    public InputField input;
    public EventEmissionManager manager;

    public void SendMessage()
    {
        if (input.text == "" || input.text == null)
        {
            return;
        }
        manager.SendChatMessage(input.text);
        input.text = null;
    }
}
