using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.tutorial
{
    public class NetworkGamePlayerLobby : NetworkBehaviour
    {

        [SyncVar]
        private string displayName = "Loading...";
        

        private NetworkManagerLobby room;
        private NetworkManagerLobby Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }
        

        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);

            Room.GamePlayers.Add(this);
        }

        public override void OnStopClient()
        {
            Room.GamePlayers.Remove(this);
        }
        
        [Server]
        public void SetDisplayname(string displayName)
        {
            this.displayName = displayName;
        }


    }
}