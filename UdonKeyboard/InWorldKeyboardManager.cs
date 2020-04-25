using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

///<Summary>The manager for a keyboard.</Summary>
public class InWorldKeyboardManager : UdonSharpBehaviour
{
    public string kName;
    ///<Summary>The input field for this keyboard.</Summary>
    public InputField input;
    ///<Summary>All the keyboard keys this keyboard has.</Summary>
    public KeyboardKey[] keys;
    ///<Summary>The shift key for this keyboard.</Summary>
    public ShiftKey shiftKey;
    ///<Summary>The Character Freeze button for this keyboard.</Summary>
    public InWorldImmobilizeToggleKey immobilizeToggle;

    public GameObject root;

    public int maxStringLength;

    private bool caps;
    private bool shift;
    private bool visible;

    ///<Summary>Send a key's character to the keyboard. Deactivate shift if it's on.</Summary>
    public void SendKey(string character)
    {
        if (input.text.Length >= maxStringLength)
        {
            return;
        }

        input.text += character;

        if (shift)
        {
            shift = false;
            shiftKey.PressKey();
        }
    }

    ///<Summary>Set caps to ON. Shift the keys to upper case if shift isn't on.</Summary>
    public void CapsOn()
    {
        caps = true;

        if (!shift)
        {
            SetKeysUpper();
        }
    }

    ///<Summary>Set caps to off. Shift the keys to lower case if shift isn't on.</Summary>
    public void CapsOff()
    {
        caps = false;

        if (!shift)
        {
            SetKeysLower();
        }
    }

    ///<Summary>Set shift to ON. Shift the keys to upper case if caps isn't on.</Summary>
    public void ShiftOn()
    {
        shift = true;

        if (!caps)
        {
            SetKeysUpper();
        }
    }

    ///<Summary>Set shift to off. Shift the keys to lower case if caps isn't on.</Summary>
    public void ShiftOff()
    {
        shift = false;

        if (!caps)
        {
            SetKeysLower();
        }
    }

    ///<Summary>Delete the last character in the input field.</Summary>
    public void Backspace()
    {
        if (input.text.Length > 0)
        {
            input.text = input.text.Remove(input.text.Length - 1);
        }
    }

    public void EnableKeyboard()
    {
        Debug.Log(kName + " keyboard enabled");
        root.SetActive(true);
        immobilizeToggle.Deactivate();
    }

    ///<Summary>Toggle the keyboard on or off.</Summary>
    public void DisableKeyboard()
    {
        Debug.Log(kName + " keyboard disabled");
        root.SetActive(false);
        immobilizeToggle.Deactivate();
    }

    private void SetKeysUpper()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].SetUpper();
        }
    }

    private void SetKeysLower()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].SetLower();
        }
    }
}