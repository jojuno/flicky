using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace Mirror.tutorial
{
    public class JoinLobbyMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby networkManager = null;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel = null;
        [SerializeField] private TMP_InputField ipAddressInputField = null;
        [SerializeField] private Button joinButton = null;

        private void HandleClientConnected()
        {
            joinButton.interactable = true;

            gameObject.SetActive(false);
            landingPagePanel.SetActive(false);
        }

        private void HandleClientDisconnected()
        {
            joinButton.interactable = true;
        }

        private void OnEnable() {
            NetworkManagerLobby.OnClientConnected += HandleClientConnected;
            NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
        }

        private void OnDisable()
        {
            NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
            NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
        }

        //hosts and joins the lobby
        /*public void HostLobby() //rejects joining at port 7777: no response from localhost
        {
            string ipAddress = ipAddressInputField.text;

            networkManager.networkAddress = ipAddress;
            Debug.Log("network address in join lobby menu: " + networkManager.networkAddress);
            
            networkManager.StartHost();

            joinButton.interactable = false;
        }*/

        //only joins; cannot host
        public void JoinLobby() //rejects joining at port 7777: no response from localhost
        {
            string ipAddress = ipAddressInputField.text;

            networkManager.networkAddress = ipAddress;
            Debug.Log("network address in join lobby menu: " + networkManager.networkAddress);
            
            networkManager.StartClient();

            joinButton.interactable = false;
        }


    }
}

