using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

public class KeyboardKey : UdonSharpBehaviour
{
    public string lowCharacter;
    public string upCharacter;
    private string character;
    public KeyboardManager manager;
    public Text buttonText;

    public void Start()
    {
        SetLower();
    }

    public void PressKey()
    {
        manager.SendKey(character);
    }

    public void SetLower()
    {
        character = lowCharacter;
        if (character == "space")
        {
            buttonText.text = "Space";
            character = " ";
            return;
        }

        buttonText.text = character.ToUpper();
    }

    public void SetUpper()
    {
        character = upCharacter;
        if (character == "space")
        {
            buttonText.text = "Space";
            character = " ";
            return;
        }

        buttonText.text = character.ToUpper();
    }
}