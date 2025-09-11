using UnityEngine;
using TMPro;
using Items;
using PlayerController;

namespace HUD
{
    public class BerriesAmount : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentBerries;
        [SerializeField] private ItemSO _berryItem;

        private Inventory _inventory;

        private void Start()
        {
            var playerGO = GameObject.FindWithTag("Player");
            var _player = playerGO.GetComponent<PlayerController.Player>();
            _inventory = _player.Inventory;

            UpdateBerryText();
        }

        private void Update()
        {
            UpdateBerryText();
        }

        private void UpdateBerryText()
        {
            int berryCount = _inventory.GetItemCount(_berryItem);
            _currentBerries.text = $"{berryCount}/{_berryItem.maxAmount}";
        }
    }
}