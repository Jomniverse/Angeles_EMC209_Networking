using Unity.Netcode;
using UnityEngine;

public class MultiplayerMenu : MonoBehaviour
{
    public void StartHost() // STARTS THE GAME AS BOTH SERVER AND CLIENT
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient() // STARTS THE GAME AS CLIENT THAT CONNECTS TO A  HOST OR SERVER
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StartServer() // STARTS THE GAME AS SERVER ONLY
    {
        NetworkManager.Singleton.StartHost();
    }
}
