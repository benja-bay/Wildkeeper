using UnityEngine;
using PlayerController;

namespace Objects
{
    public class NoteObject : MonoBehaviour, IInteractable
    {
        [Header("Note data")]
        [SerializeField] private GameObject notePrefab;

        public void Interact(PlayerController.Player player)
        {
            notePrefab?.SetActive(true);
            Time.timeScale = 0;
        }

        public void CloseNote()
        {
            notePrefab?.SetActive(false);
            Time.timeScale = 1;
        }

        public string ObjectID => null;
    }
}
