using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

///<Summary>A generic button that calls a UdonBehaviour's event when a collider hits it.</Summary>
public class GenericButton : UdonSharpBehaviour
{
    public Collider triggerer;
    public UdonBehaviour behaviour;
    public string eventName;
    private float activatedTimer = 1;
    private bool activatedCountdown;
    private float updateTimer = 0.25f;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (activatedCountdown)
        {
            activatedTimer -= Time.deltaTime;
            if (activatedTimer < 0)
            {
                activatedCountdown = false;
                activatedTimer = 1;
            }

            return;
        }

        if (updateTimer < 0)
        {
            updateTimer -= Time.deltaTime;
            return;
        }

        if (Vector3.Distance(triggerer.transform.position, transform.position) < 0.05 && image != null) // Distance is quite expensive so let's avoid doing it too often.
        {
            image.color = Color.cyan;
        }
        else
        {
            image.color = Color.white;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == triggerer.name && !activatedCountdown)
        {
            activatedCountdown = true;
            behaviour.SendCustomEvent(eventName);
        }
    }
}
