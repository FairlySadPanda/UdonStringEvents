using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

///<Summary>A Logger for Udon. Attaches to a player's hand if they're in VR, or sits at some point in space if they're in desktop mode.</Summary>
public class UdonLogger : UdonSharpBehaviour
{
    ///<Summary>The prefab for a line in the logger.</Summary>
    public GameObject logLinePrefab;
    ///<Summary>The canvas that log lines should be printed on.</Summary>
    public Canvas logScreenCanvas;
    ///<Summary>The root object of the buttons that manipulate the logger. Used to anchor the logger to things.</Summary>
    public GameObject buttons;

    private bool isActive;
    private int maxNumberOfLogLines;
    private GameObject[] logs;
    private bool anchored;

    private float update;

    private int count;

    void Start()
    {
        gameObject.SetActive(false);
        count = 0;
        maxNumberOfLogLines = 10;
        logs = new GameObject[maxNumberOfLogLines];

        VRCPlayerApi player = Networking.LocalPlayer;
        if (player != null && !player.IsUserInVR())
        {
            buttons.SetActive(false);
        }
    }

    public void Update()
    {
        VRCPlayerApi player = Networking.LocalPlayer;
        if (player != null && player.IsUserInVR() && !anchored)
        {
            gameObject.transform.position = player.GetBonePosition(HumanBodyBones.RightHand);
            gameObject.transform.rotation = player.GetBoneRotation(HumanBodyBones.RightHand);
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    ///<Summary>Show or hide the Logger.</Summary>
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    ///<Summary>Update the logger to output something.</Summary>
    public void Notice(string log)
    {
        // This was written with zero knowledge of how Unity's UI features can be better leveraged.
        // If any experts want to improve this code, put a PR in!

        // Kill the top log line if it's gone off screen.
        // An improvement to this system would be a scroll ability!
        if (logs[maxNumberOfLogLines - 1] != null)
        {
            Destroy(logs[maxNumberOfLogLines - 1]);
            logs[maxNumberOfLogLines - 1] = null;
        }

        // For each log, shift it up by 100.
        // An improvement would be to shift it up by its height, rather than using 100 as a magic number.
        for (int i = maxNumberOfLogLines - 2; i > -1; i--)
        {
            if (logs[i] != null)
            {
                RectTransform logTrans = logs[i].GetComponent<RectTransform>();
                logTrans.anchoredPosition3D = new Vector3(0, logTrans.anchoredPosition3D.y + 100, -1);
                logs[i + 1] = logs[i];
            }
        }

        GameObject newLog = VRCInstantiate(logLinePrefab);
        Text text = newLog.GetComponent<Text>();
        text.text = log;

        logs[0] = newLog;
        newLog.transform.SetParent(logScreenCanvas.transform);

        RectTransform transform = newLog.GetComponent<RectTransform>();
        transform.anchoredPosition3D = new Vector3(0, 100, -1);
        transform.localRotation = new Quaternion();
        transform.localScale = new Vector3(1, 1, 1);
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        Notice(player.displayName + " joined.");
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        Notice(player.displayName + " left.");
    }

    public void Anchor()
    {
        anchored = true;
    }

    public void Unanchor()
    {
        anchored = false;
    }
}
