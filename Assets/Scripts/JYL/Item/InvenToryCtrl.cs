using UnityEngine;

public enum InvenType
{
    Inven,
    Box,
    Equipped,
    EquipBox
}

public class InvenToryCtrl : MonoBehaviour
{
    public static InvenToryCtrl Instance;

    [Header("인벤토리들")]
    [SerializeField] EquippedInventoryUI equippedInventoryUI;
    [SerializeField] EquipInventoryUI equipInventoryUI;
    [SerializeField] InventoryItems inventoryItems;
    [SerializeField] BoxInvenTory boxInvenTory;

    [Header("툴팁")]
    [SerializeField] ItemToolTipCtrl itemToolTipCtrl;
    [SerializeField] EquipItemToolTipCtrl equipItemToolTipCtrl;

    public EquippedInventoryUI EquippedInventoryUI => equippedInventoryUI;
    public EquipInventoryUI EquipInventoryUI => equipInventoryUI;
    public InventoryItems InventoryItems => inventoryItems;
    public BoxInvenTory BoxInvenTory => boxInvenTory;
    public ItemToolTipCtrl ItemToolTipCtrl => itemToolTipCtrl;
    public EquipItemToolTipCtrl EquipItemToolTipCtrl => equipItemToolTipCtrl;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // 인벤 ↔ 박스 아이템 이동
    public void ChangeItemByKey(InvenType fromType, int itemID)
    {
        if (fromType == InvenType.Equipped || fromType == InvenType.EquipBox)
        {
            Debug.LogError("장비 인벤토리에서는 아이템을 교환할 수 없습니다.");
            return;
        }

        BaseInventory from = (fromType == InvenType.Inven) ? inventoryItems : boxInvenTory;
        BaseInventory to = (fromType == InvenType.Inven) ? boxInvenTory : inventoryItems;

        int fromIndex = from.ItemIDs.FindIndex(id => id == itemID);
        if (fromIndex < 0)
        {
            Debug.LogWarning($"[ChangeItemByKey] from 인벤토리에 ID {itemID} 없음");
            return;
        }

        // from에서 제거
        from.ItemIDs[fromIndex] = 0;

        // to에 추가 시도
        to.ChangeItem(to.ItemIDs, itemID);

        inventoryItems.CompactItemList();
        boxInvenTory.CompactItemList();

        inventoryItems.RefreshUI();
        boxInvenTory.RefreshUI();
    }


    // 특정 장비 부위에 아이템 장착
    public void EquipItemToSlot(EquipSlot slot, int itemID)
    {
        BaseItem item = ItemDataBase.Instance.GetItem(itemID);
        if (item == null || item.GetEquipSlot() != slot)
        {
            Debug.LogWarning("장착 불가능한 슬롯입니다.");
            return;
        }

        equippedInventoryUI.EquipItem(item);
        equipInventoryUI.RemoveItem(itemID);

        equippedInventoryUI.RefreshUI();
        equipInventoryUI.RefreshUI();
    }

    // 장착 해제 → 장비 인벤토리에 반환
    public void UnEquipItemFromSlot(EquipSlot slot)
    {
        int itemID = equippedInventoryUI.GetEquippedItemID(slot);
        if (itemID <= 0)
        {
            Debug.LogWarning("해당 부위에 장착된 아이템이 없음.");
            return;
        }

        equippedInventoryUI.UnequipItem(slot);
        equipInventoryUI.TryAddItem(itemID);

        equippedInventoryUI.RefreshUI();
        equipInventoryUI.RefreshUI();
    }

    public bool IsEquipSelectOpen => equipInventoryUI.gameObject.activeSelf;
}
