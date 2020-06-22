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
    ///<Summary>The caps key for this keyboard.</Summary>
    public CapsLockKey capsLockKey;
    ///<Summary>The Character Freeze button for this keyboard.</Summary>
    public ImmobilizeToggleKey immobilizeToggle;

    private bool caps;
    private bool shift;
    private bool visible;

    public void Start()
    {
        SetActive(false);
    }

    public void Update()
    {
        VRCPlayerApi player = Networking.LocalPlayer;
        if (player != null)
        {
            // Be aware that if you're using GotoUdon -- which you should, as it's ace -- it will break here if you have avatar emulation on.
            var lowestPossibleYPos = Vector3.Lerp(player.GetBonePosition(HumanBodyBones.LeftFoot), player.GetBonePosition(HumanBodyBones.RightFoot), 0.5f).y - 1f;
            var newPos = player.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position.y - 1.7F;
            if (lowestPossibleYPos > newPos)
            {
                newPos = lowestPossibleYPos;
            }

            var pos = player.GetPosition();
            pos.y = newPos;
            transform.position = pos;

            // If the keyboard is active, the log screen should sit above the keyboard.
            if (activeSelf)
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
            shiftKey.PressKey();
        }
    }

    ///<Summary>Set caps to ON. Shift the keys to upper case if shift isn't on.</Summary>
    public void CapsOn()
    {
        caps = true;
        SetKeysUpper();
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
        SetKeysUpper();
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
        if (input.isFocused)
        {
            return;
        }

        if (Networking.LocalPlayer != null && !Networking.LocalPlayer.IsUserInVR())
        {
            logScreen.Toggle();
        }

        SetActive(!activeSelf);

        if (activeSelf)
        {
            if (Networking.LocalPlayer != null)
            {
                transform.rotation = Networking.LocalPlayer.GetRotation();
            }

            logScreen.Anchor();
            return;
        }

        logScreen.Unanchor();
        immobilizeToggle.Deactivate();
        capsLockKey.Deactivate();
        shiftKey.Deactivate();
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