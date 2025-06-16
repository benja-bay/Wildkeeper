using UnityEngine;
using TMPro;

namespace HUD
{
    public class InteractionUIManager : MonoBehaviour
    {
        public static InteractionUIManager Instance { get; private set; }

        [SerializeField] private GameObject promptPanel;
        [SerializeField] private TextMeshProUGUI promptText;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            HidePrompt();
        }

        public void ShowPrompt(string message)
        {
            promptText.text = message;
            promptPanel.SetActive(true);
        }

        public void HidePrompt()
        {
            promptPanel.SetActive(false);
        }
    }
}
