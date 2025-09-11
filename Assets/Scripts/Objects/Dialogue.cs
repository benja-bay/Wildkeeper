using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Dialogues;

namespace Objects
{
    public class Dialogue : MonoBehaviour, IInteractable
    {
        [Header("UI References")]
        [SerializeField] private NPCDialogue dialogueData;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private Image portraitImage;

        private int _dialogueIndex;
        private bool _isTyping;
        private bool isDialogueActive;
        private Coroutine _typingCoroutine;
        private PlayerController.Player _currentPlayer;

        public bool CanInteract()
        {
            return !isDialogueActive;
        }

        public void Interact(PlayerController.Player player)
        {
            if (dialogueData == null || _isTyping) 
                return;

            _currentPlayer = player;
            
            if (!isDialogueActive)
            {
                StartDialogue();
            }
            else if (_dialogueIndex < dialogueData.dialogueLines.Length && 
                    !dialogueData.autoProgressLines[_dialogueIndex])
            {
                NextLine();
            }
        }

        void StartDialogue()
        {
            if (dialogueData == null || dialogueText == null || nameText == null || dialoguePanel == null)
            {
                return;
            }

            if (_currentPlayer != null)
            {
                _currentPlayer.ChangeToIdleState();
                _currentPlayer.rb2D.velocity = Vector2.zero;
            }

            isDialogueActive = true;
            _dialogueIndex = 0;
            nameText.text = dialogueData.npcName;
            
            if (portraitImage != null && dialogueData.npcPortrait != null)
            {
                portraitImage.sprite = dialogueData.npcPortrait;
            }

            dialoguePanel.SetActive(true);
            _typingCoroutine = StartCoroutine(TypeLine());
        }

        void NextLine()
        {
            if (_isTyping)
            {
                if (_typingCoroutine != null)
                    StopCoroutine(_typingCoroutine);
                    
                dialogueText.text = dialogueData.dialogueLines[_dialogueIndex];
                _isTyping = false;
                return;
            }

            _dialogueIndex++;
            
            if (_dialogueIndex < dialogueData.dialogueLines.Length)
            {
                _typingCoroutine = StartCoroutine(TypeLine());
            }
            else
            {
                EndDialogue();
            }
        }

        IEnumerator TypeLine()
        {
            _isTyping = true;
            dialogueText.text = "";

            string currentLine = dialogueData.dialogueLines[_dialogueIndex];
            foreach (char letter in currentLine)
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(dialogueData.typingSpeed);
            }

            _isTyping = false;

            if (_dialogueIndex < dialogueData.autoProgressLines.Length && 
                dialogueData.autoProgressLines[_dialogueIndex])
            {
                yield return new WaitForSeconds(dialogueData.autoProgressDelay);
                NextLine();
            }
        }

        public void EndDialogue()
        {
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);

            if (_currentPlayer != null)
            {
                _currentPlayer.ChangeToIdleState();
            }

            isDialogueActive = false;
            _dialogueIndex = 0;
            dialogueText.text = "";
            dialoguePanel.SetActive(false);
        }

        void OnValidate()
        {
            if (dialogueData != null && dialogueData.autoProgressLines.Length != dialogueData.dialogueLines.Length)
            {
                System.Array.Resize(ref dialogueData.autoProgressLines, dialogueData.dialogueLines.Length);
            }
        }
        
        public string ObjectID => null;
    }
}