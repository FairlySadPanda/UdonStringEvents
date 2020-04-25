using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[AddComponentMenu("")]
public class PlayerTracker : UdonSharpBehaviour
{
    public int maxPlayersInWorld;

    private VRCPlayerApi[] players;

    public void Start()
    {
        players = new VRCPlayerApi[maxPlayersInWorld];
        players[0] = Networking.LocalPlayer;
    }

    public VRCPlayerApi GetPlayerByName(string name)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null && players[i].displayName == name)
            {
                Debug.Log("Getting player API for player " + players[i].displayName);
                return players[i];
            }
        }

        return null;
    }

    public VRCPlayerApi[] GetPlayers()
    {
        return players;
    }

    public VRCPlayerApi GetPlayerById(int id)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null && players[i].playerId == id)
            {
                Debug.Log("Getting player API for player " + players[i].displayName);
                return players[i];
            }
        }

        return null;
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        for (int i = 1; i < players.Length; i++)
        {
            if (players[i] == null)
            {
                Debug.Log("Storing player API for player " + player.displayName + " at index " + i);
                players[i] = player;
                return;
            }
        }
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        for (int i = 1; i < players.Length; i++)
        {
            if (players[i] != null && players[i].displayName == player.displayName)
            {
                players[i] = null;
                return;
            }
        }
    }
}