using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager Instance;

    [SerializeField] private EquipInventoryUI equipInventoryUI;
    [SerializeField] private GameObject equipSelectPopup;

    [SerializeField] EquipSlotCtrl[] equipSlotUIs; // 장착 슬롯 UI 목록
    int[] equippedItemIDs = new int[(int)EquipSlot.end]; // 현재 장착 중인 아이템 ID 목록

    private EquipSlot currentSlot;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void OpenEquipSelectUI(EquipSlot slot)
    {
        currentSlot = slot;

        equipInventoryUI.SetFilter(ItemType.Armor); // or Weapon
        equipSelectPopup.SetActive(true);
    }

    public void SelectItem(int itemID)
    {
        var item = ItemManager.Instance.GetItem(itemID);
        // 장착
        InvenToryCtrl.Instance.EquippedInventoryUI.EquipItem(item);

        // UI 닫기
        equipSelectPopup.SetActive(false);
    }

    public void CloseSelect()
    {
        equipSelectPopup.SetActive(false);
    }

    public void EquipItemByID(int id)
    {
        BaseItem item = ItemManager.Instance.GetItem(id);
        if (item == null || item.GetEquipSlot() == null)
        {
            Debug.LogWarning("장착할 수 없는 아이템입니다.");
            return;
        }

        EquipItemToSlot(item);
    }

    public void EquipItemToSlot(BaseItem item)
    {
        EquipSlot? slotType = item.GetEquipSlot();
        if (slotType == null)
        {
            Debug.LogWarning("장착할 수 없는 아이템입니다.");
            return;
        }

        int slotIndex = (int)slotType.Value;

        // 기존 장비 해제
        equippedItemIDs[slotIndex] = item.id;

        // UI 반영
        if (equipSlotUIs.Length > slotIndex)
        {
            equipSlotUIs[slotIndex].SlotListSetting(item);
        }

        Debug.Log($"{slotType.Value} 슬롯에 {item.name} 장착됨");
    }


    public void UnEquipItemBySlot(EquipSlot slot)
    {
        int index = (int)slot;

        // 이미 빈 슬롯이라면 무시
        if (equippedItemIDs[index] == 0)
        {
            Debug.Log($"{slot} 슬롯은 이미 비어 있음");
            return;
        }

        // 슬롯 초기화
        equippedItemIDs[index] = 0;

        // UI 초기화
        if (equipSlotUIs.Length > index)
        {
            equipSlotUIs[index].SlotListSetting(ItemDataBase.Instance.emptyItem); //  emptyItem
        }

        Debug.Log($"{slot} 슬롯의 장비를 해제했습니다.");
    }
    public BaseItem GetEquippedItem(EquipSlot slot)
    {
        int index = (int)slot;
        int id = equippedItemIDs[index];
        return ItemManager.Instance.GetItem(id);
    }
}
