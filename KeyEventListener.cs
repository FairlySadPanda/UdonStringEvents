using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using System;

public class DesktopKeyboardToggle : UdonSharpBehaviour
{
    public PoolingKeyListenerEntity keyListener;
    public KeyboardManager keyboard;
    
    void Awake() 
    {
        if(Networking.LocalPlayer.IsUserInVR()) 
            keyListener.OnPressed += keyboard.Toggle;    
    }
}


public class PoolingKeyListenerEntity : UdonSharpBehaviour
{
    public PoolingKeyListenerSystem keyListenerSystem;
    public KeyCode key;
    public event Action OnPressed;

    void Awake() => keyListenerSystem.AddEntityToSystem(this);
    
    void Check()
    {
        if(Input.GetKeyDown(key))
            OnPresed?.Invoke();
    }
}


public class PoolingKeyListenerSystem : UdonSharpBehaviour
{
    public PoolingKeyListenerEntity[] listeners;
    int entityCount;
    
    void Update()
    {
        for(int i = 0; i < entityCount; ++i)
            listeners[i].Check();
    }
    
    public void AddEntityToSystem(PoolingKeyListenerEntity entity)
    {
        int currentLength = listeners.Length;
        Array.Resize(ref listeners, currentLength + 1);
        listeners[currentLength - 1] = entity;
        entityCount++;
    }
}
