using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.tutorial //must set this so the classes appear
{
    public class PlayerNameInput : MonoBehaviour
    {
        public GameObject inputFieldObj;

        [Header("UI")]
        [SerializeField] private TMP_InputField nameInputField = null;
        [SerializeField] private Button continueButton = null;

        public static string DisplayName { get; private set; }

        private const string PlayerPrefsNameKey = "123";

        private void Start() => SetUpInputField();

        private void SetUpInputField()
        {

            if (PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

            //Debug.Log("set up input field, player prefs does have 'PlayerName' as a key");
            string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

            nameInputField.text = defaultName;

            SetPlayerName();
        }

        public void SetPlayerName()
        {
            //Debug.Log("set player name" +);
            TMP_InputField inputField = inputFieldObj.GetComponent<TMP_InputField>();
            string value = inputField.text;
            continueButton.interactable = !string.IsNullOrEmpty(value);
        }

        public void SavePlayerName()
        {
            DisplayName = nameInputField.text;

            PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
        }
    }
}

