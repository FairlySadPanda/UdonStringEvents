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
    ///<Summary>The receiver we want to handle chat events.</Summary>
    public EventReceiver receiver;

    public string[] badWords;
    private int maxMessageLength;

    public void Start()
    {
        maxMessageLength = 50;
    }

    ///<Summary>Send a ChatMessage using the product of an InputField.</Summary>
    public void SendMessage()
    {
        if (input.text == "" || input.text == null)
        {
            return;
        }

        if (input.text.Length > maxMessageLength)
        {
            input.text = input.text.Substring(0, maxMessageLength);
        }

        string message = input.text;
        if (badWordsFound(message) == false)
        {
            receiver.SendEvent("ChatMessage", message.Replace(",", "|"));
        }

        input.text = null;
    }

    public bool badWordsFound(string input)
    {
        bool found = false;

        string[] inputs = input.Replace("1", "i")
                          .Replace("!", "i")
                          .Replace("3", "e")
                          .Replace("4", "a")
                          .Replace("@", "a")
                          .Replace("5", "s")
                          .Replace("7", "t")
                          .Replace("0", "o")
                          .Replace("9", "g")
                          .Replace("\"", "")
                          .Replace("£", "e")
                          .Replace("$", "s")
                          .Replace("€", "e")
                          .Replace("%", "")
                          .Replace("^", "")
                          .Replace("*", "")
                          .Replace("(", "")
                          .Replace(")", "")
                          .Replace("-", "")
                          .Replace("_", "")
                          .Replace("=", "")
                          .Replace("+", "")
                          .Replace("{", "")
                          .Replace("]", "")
                          .Replace("}", "")
                          .Replace(";", "")
                          .Replace("'", "")
                          .Replace("#", "")
                          .Replace("~", "")
                          .Replace("\\", "")
                          .Replace(",", "")
                          .Replace("?", "")
                          .Replace("/", "")
                          .ToLower()
                          .Split(' ');

        for (int inputsIndex = 0; inputsIndex < inputs.Length; inputsIndex++)
        {
            input = inputs[inputsIndex];

            for (int badWordsIndex = 0; badWordsIndex < badWords.Length; badWordsIndex++)
            {
                if (input.Contains(badWords[badWordsIndex]))
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                break;
            }
        }

        return found;
    }
}


