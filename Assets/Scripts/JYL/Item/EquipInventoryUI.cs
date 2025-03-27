using UnityEngine;

public class EquipInventoryUI : BaseInventory
{
    [SerializeField] private ItemType filterType = ItemType.All;

    private void Start()
    {
        invenType = InvenType.EquipBox;

        SlotSetting(slotParent);
        InvenInit();

        // øπΩ√∑Œ æ∆¿Ã≈€ √ﬂ∞°
        TryAddItem(1001); // «Â≈Õ ≥™¿Ã«¡
        TryAddItem(2001); // «Â≈Õ «Ô∏ß
        TryAddItem(2002); // «Â≈Õ √ºΩ∫∆Æ
        TryAddItem(3001); // ∫ª «Ô∏ß

        RefreshUI();
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    public void SetFilter(ItemType type)
    {
        filterType = type;
        RefreshUI();
    }

    public override void RefreshUI()
    {
        for (int i = 0; i < slotObjs.Count; i++)
        {
            int id = (i < itemIDs.Count) ? itemIDs[i] : 0;
            var item = ItemManager.Instance.GetItem(id);

            bool isValid = item != null && 
                           item.type != ItemType.Empty &&
                           (filterType == ItemType.All || item.type == filterType);

            slotObjs[i].GetComponent<ItemSlot>().SetItem(isValid ? id : 0);
        }
    }
}
