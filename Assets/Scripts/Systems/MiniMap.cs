using System;
using UnityEngine;

namespace Systems
{
    public class MiniMap: MonoBehaviour
    {
        public static MiniMap Instance {get; private set;}
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}