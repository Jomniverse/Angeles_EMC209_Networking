using UnityEngine;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using System.Collections.Generic;

public class LobbyReadyManager : NetworkBehaviour
{
    [SerializeField] TMP_Text playerListText;
    [SerializeField] TMP_Text playerStatusText;
    [SerializeField] GameObject startGameButton;

    private Dictionary<ulong,bool> playerReadyStates = new Dictionary<ulong,bool>();
    // unassign long integer(player), bool (Ready or Not)

    public override void OnNetworkSpawn()
    {
        if (IsServer) 
        {
            NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;
        }
    }

    private void Singleton_OnClientDisconnectCallback(ulong obj)
    {
        throw new System.NotImplementedException();
    }

    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
        throw new System.NotImplementedException();
    }
}
