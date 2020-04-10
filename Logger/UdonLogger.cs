// Logger for Udon

// This system maintains a log event system in Udon, similar to normal logging systems.
// It has four levels: error, warning, info and notice.
// Error: something has gone fatally wrong and the behaviour needs to terminate.
// Warning: something is probably wrong and the developer might want to know about it.
// Info: A view's state has changed and a deloper might want to know about it.
// Notice: A view's state has changed and anyone - including a player - might want to know about it. 
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class UdonLogger : UdonSharpBehaviour
{
    public GameObject logLinePrefab;
    public GameObject testObj;
    public Canvas logScreen;
    public GameObject playerUI;

    public bool isDev;

    private bool isActive;

    // [UdonSynced]
    // private string latestError;
    // private string lastError;

    // [UdonSynced]
    // private string latestWarning;
    // private string lastWarning;

    // [UdonSynced]
    // private string latestInfo;
    // private string lastInfo;

    private int maxNumberOfLogLines;
    private GameObject[] logs;

    private float update;

    private int count;

    void Start()
    {
        //Hide();
        count = 0;
        maxNumberOfLogLines = 5;
        logs = new GameObject[maxNumberOfLogLines];
    }

    public void Update()
    {
        // if (Networking.LocalPlayer != null)
        // {
        //     playerUI.transform.position = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.RightHand);
        //     playerUI.transform.rotation = Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.RightHand);
        // }
    }

    public void Toggle()
    {
        if (isActive == true)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Hide()
    {
        logScreen.gameObject.SetActive(false);
        isActive = false;
    }

    private void Show()
    {
        logScreen.gameObject.SetActive(true);
        isActive = true;
    }

    public void Notice(string log)
    {
        Debug.Log("Updating log to say: " + log);
        UpdateLog(log);
    }


    private void UpdateLog(string logString)
    {
        if (logs[4] != null)
        {
            Destroy(logs[4]);
            logs[4] = null;
        }

        for (int i = maxNumberOfLogLines - 2; i > -1; i--)
        {
            if (logs[i] != null)
            {
                RectTransform logTrans = logs[i].GetComponent<RectTransform>();
                logTrans.anchoredPosition3D = new Vector3(0, logTrans.anchoredPosition3D.y + 200, -1);
                logs[i + 1] = logs[i];
            }
        }

        GameObject newLog = VRCInstantiate(logLinePrefab);
        Text text = newLog.GetComponent<Text>();
        text.text = logString;

        logs[0] = newLog;
        newLog.transform.SetParent(logScreen.transform);

        RectTransform transform = newLog.GetComponent<RectTransform>();
        transform.anchoredPosition3D = new Vector3(0, 300, -1);
        transform.localRotation = new Quaternion();
        transform.localScale = new Vector3(1, 1, 1);
    }
}
