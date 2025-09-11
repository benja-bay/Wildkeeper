using UnityEngine;
using HUD;

public class InteractableHighlighter : MonoBehaviour
{
    [SerializeField] private string message = "Presiona Click Derecho para interactuar";

    private void OnMouseEnter()
    {
        InteractionUIManager.Instance?.ShowPrompt(message);
    }

    private void OnMouseExit()
    {
        InteractionUIManager.Instance?.HidePrompt();
    }
}
