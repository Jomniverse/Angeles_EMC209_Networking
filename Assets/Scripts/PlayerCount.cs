using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerCount : MonoBehaviour
{
    public static PlayerCount instance;
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
}