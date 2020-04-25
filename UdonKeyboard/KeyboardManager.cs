using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

///<Summary>The manager for a keyboard.</Summary>
public class KeyboardManager : UdonSharpBehaviour
{
    ///<Summary>The anchor on this keyboard for putting a Logger on it.</Summary>
    public GameObject keyboardAnchor;
    ///<Summary>The root of a Logger, used for anchoring it to the keyboard.</Summary>
    public UdonLogger logScreen;
    ///<Summary>The default anchor location for the Logger.</Summary>
    public GameObject logScreenAnchor;
    ///<Summary>The input field for this keyboard.</Summary>
    public InputField input;
    ///<Summary>All the keyboard keys this keyboard has.</Summary>
    public KeyboardKey[] keys;
    ///<Summary>The shift key for this keyboard.</Summary>
    public ShiftKey shiftKey;
    ///<Summary>The Character Freeze button for this keyboard.</Summary>
    public ImmobilizeToggleKey immobilizeToggle;

    private bool caps;
    private bool shift;
    private bool visible;

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void Update()
    {
        VRCPlayerApi player = Networking.LocalPlayer;
        if (player != null)
        {
            gameObject.transform.position = player.GetPosition();

            // If the keyboard is active, the log screen should sit above the keyboard.
            if (gameObject.activeSelf)
            {
                logScreen.transform.position = keyboardAnchor.transform.position;
                logScreen.transform.rotation = keyboardAnchor.transform.rotation;
                logScreen.transform.localScale = new Vector3(2, 2, 2);
            }
        }
    }

    ///<Summary>Send a key's character to the keyboard. Deactivate shift if it's on.</Summary>
    public void SendKey(string character)
    {
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

    ///<Summary>Toggle the keyboard on or off.</Summary>
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            if (Networking.LocalPlayer != null)
            {
                gameObject.transform.rotation = Networking.LocalPlayer.GetRotation();
            }

            logScreen.Anchor();
            return;
        }

        logScreen.Unanchor();
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