using UnityEngine;
using UnityEngine.EventSystems;

namespace Objects
{
    public class NoteCloser: MonoBehaviour
    {
        [SerializeField] private NoteObject note;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            note?.CloseNote();
        }
        
        private void Update()
        {
            if (Input.GetButtonDown("Attack"))
            {
                note?.CloseNote();
            }
        }
    }
}