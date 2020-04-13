using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

///<Summary>A controller for a button that will convert the product of an InputField to a ChatMessage event.</Summary>
public class ChatController : UdonSharpBehaviour
{
    ///<Summary>The text input field for this controller.</Summary>
    public InputField input;
    ///<Summary>The manager we want to handle chat events.</Summary>
    public EventEmissionManager manager;

    ///<Summary>Send a ChatMessage using the product of an InputField.</Summary>
    public void SendMessage()
    {
        if (input.text == "" || input.text == null)
        {
            return;
        }

        if (input.text.Length > 100)
        {
            input.text = input.text.Substring(0, 100);
        }

        string message = input.text;
        manager.SendEvent("ChatMessage", message.Replace(",", "|"));
        input.text = null;
    }
}
