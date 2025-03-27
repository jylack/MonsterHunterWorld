using System.Collections.Generic;
using UnityEngine;

public class EquippedInventoryUI : BaseInventory
{
    [SerializeField] GameObject slotParent;

    private List<EquipSlotCtrl> slotUIs = new List<EquipSlotCtrl>();

    private void Start()
    {
        invenType = InvenType.Equipped;

        // 슬롯 설정: 자식 오브젝트 기준
        var slots = slotParent.GetComponentsInChildren<EquipSlotCtrl>();
        slotUIs.AddRange(slots);

        // 슬롯 초기화: 개수는 EquipSlot.end 기준
        itemIDs = new List<int>();
        for (int i = 0; i < (int)EquipSlot.end; i++)
        {
            itemIDs.Add(0); // 0은 빈 슬롯
        }

        RefreshUI();
    }

    // 장비 장착 - 장착한 아이템은 정보가 필요하므로 세팅
    public void EquipItem(BaseItem item)
    {
        EquipSlot? slot = item.GetEquipSlot();
        if (slot == null) return;

        int index = (int)slot.Value;
        itemIDs[index] = item.id;
    }

    // 장비 해제
    public void UnequipItem(EquipSlot slot)
    {
        int index = (int)slot;
        itemIDs[index] = 0;
    }

    // 슬롯별 아이템 ID 확인
    public int GetEquippedItemID(EquipSlot slot)
    {
        return itemIDs[(int)slot];
    }

    public override void RefreshUI()
    {
        for (int i = 0; i < itemIDs.Count && i < slotUIs.Count; i++)
        {
            int id = itemIDs[i];
            BaseItem item = (id > 0) ? ItemDataBase.Instance.GetItem(id) : ItemDataBase.Instance.emptyItem;
            slotUIs[i].SlotListSetting(item);
        }
    }
}
