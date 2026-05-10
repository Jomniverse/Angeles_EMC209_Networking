using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class MultiplayerMenu : NetworkBehaviour
{
    public static MultiplayerMenu instance;
    [SerializeField] TMP_Text playerCountUI;

    public NetworkVariable<int> playerCounts = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public int playerCount = 0;

    private void Awake() 
    { 
        if (instance == null) 
        { 
            instance = this; 
        } 
        else 
        { 
            Destroy(gameObject); 
        } 
    }


    private void Update()
    {
            playerCountUI.text = "Player Count: " + playerCounts.Value;
    }

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
        NetworkManager.Singleton.StartServer();
    }

   
}
