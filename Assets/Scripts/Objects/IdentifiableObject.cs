using UnityEngine;

namespace Objects
{
    public class IdentifiableObject : MonoBehaviour
    {
        [SerializeField] private string objectID;

        public string ID => objectID;
        
        void Start()
        {
            if (GameManager.Instance != null)
            {
                var state = GameManager.Instance.GetRemoteObjectState(objectID);
                if (state.HasValue)
                {
                    gameObject.SetActive(state.Value);
                }
            }
        }
    }
}