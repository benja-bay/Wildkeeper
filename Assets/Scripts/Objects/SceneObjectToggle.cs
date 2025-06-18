using UnityEngine;
using System.Collections.Generic;

namespace Objects
{
    public class SceneObjectToggle : MonoBehaviour, IInteractable
    {
        [Header("Configuración")]
        [Tooltip("¿Este objeto puede usarse más de una vez?")]
        [SerializeField] private bool reusable = false;
        
        [Header("Cambio de sprite al usar")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite activatedSprite;

        [Tooltip("¿Activar o desactivar los objetos?")]
        [SerializeField] private bool activateObjects = true;

        [Tooltip("Identificador único para guardar si ya fue usado")]
        [SerializeField] private string objectID;

        [Header("Objetos a activar/desactivar")]
        [SerializeField] private List<GameObject> localObjects;
        
        [Header("Objetos remotos por ID")]
        [SerializeField] private List<string> remoteObjectIDs;

        // Futuro: referencias a objetos en otras escenas si se amplía el sistema

        private bool _hasBeenUsed;

        private void Start()
        {
            if (!string.IsNullOrEmpty(objectID) && GameManager.Instance != null)
            {
                _hasBeenUsed = GameManager.Instance.IsObjectUsed(objectID);

                if (_hasBeenUsed && !reusable)
                {
                    ToggleObjects(); // Aplicar una vez al inicio si ya fue usado
                }
            }
        }

        public void Interact(Player.Player player)
        {
            if (_hasBeenUsed && !reusable)
            {
                Debug.Log("Este objeto ya fue usado.");
                return;
            }

            Debug.Log($"OBJETO ACTIVADO");

            ToggleObjects();

            if (!string.IsNullOrEmpty(objectID))
            {
                GameManager.Instance.MarkObjectAsUsed(objectID);
                _hasBeenUsed = true;
            }
        }

        private void ToggleObjects()
        {
            foreach (var obj in localObjects)
            {
                if (obj != null)
                    obj.SetActive(activateObjects);
            }

            Debug.Log($"Objetos {(activateObjects ? "activados" : "desactivados")}.");
            
            foreach (var remoteID in remoteObjectIDs)
            {
                if (!string.IsNullOrEmpty(remoteID))
                {
                    GameManager.Instance.SetRemoteObjectState(remoteID, activateObjects);
                    Debug.Log($"Se registró cambio de estado para objeto remoto: {remoteID}");
                }
            }
            
            if (spriteRenderer != null && activatedSprite != null)
            {
                spriteRenderer.sprite = activatedSprite;
            }
        }
    }
}