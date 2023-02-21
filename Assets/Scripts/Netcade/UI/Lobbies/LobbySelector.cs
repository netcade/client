using Netcade.Networking;
using Netcade.Objects;
using Netcade.UI.Interfaces;
using UnityEngine;

public class LobbySelector : MonoBehaviour, ILobbyListUpdated
{
    public GameObject LobbyPanel;
    public void GamesListUpdated()
    {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Lobby game in ServerData.Lobbies)
        {
            GameObject panel = Instantiate(LobbyPanel, this.transform);
            panel.GetComponent<LobbyPanel>().FillData(game.Name, game.Owner.Username);
        }
    }
}
