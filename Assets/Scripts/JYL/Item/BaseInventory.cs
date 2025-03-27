using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventory : MonoBehaviour
{
    [SerializeField] protected InvenType invenType;

    [SerializeField] protected int invenSize = 20;

    [SerializeField] protected GameObject slotParent;

    protected List<int> itemIDs = new List<int>(); // 아이템 ID 저장
    protected List<GameObject> slotObjs = new List<GameObject>();

    public List<int> ItemIDs => itemIDs;

    // 초기화
    protected virtual void Awake()
    {
        SlotSetting(slotParent);
        InvenInit();
    }

    // 슬롯 UI 연결
    public void SlotSetting(GameObject parent)
    {
        slotObjs.Clear();
        var slots = parent.GetComponentsInChildren<ItemSlot>();

        foreach (var s in slots)
        {
            s.SetInvenType(invenType);
            slotObjs.Add(s.gameObject);
        }
    }

    // 인벤토리 내부 데이터 초기화
    public virtual void InvenInit()
    {
        itemIDs.Clear();
        for (int i = 0; i < invenSize; i++)
        {
            itemIDs.Add(0); // 0 = 빈 슬롯
        }
    }

    // 아이템 추가 (빈칸 찾아서)
    public bool TryAddItem(int itemID)
    {
        int index = itemIDs.FindIndex(id => id == 0);
        if (index >= 0)
        {
            itemIDs[index] = itemID;
            return true;
        }
        return false;
    }

    // 아이템 삭제
    public void RemoveItem(int index)
    {
        if (index >= 0 && index < itemIDs.Count)
        {
            itemIDs[index] = 0;
        }
    }

    // 아이템 교환 또는 스택 증가
    public void ChangeItem(List<int> currentList, int itemID)
    {
        var item = ItemManager.Instance.GetItem(itemID);
        int index = currentList.FindIndex(id =>
        {
            var target = ItemManager.Instance.GetItem(id);
            return target.name == item.name && target.count < target.maxCount;
        });

        if (index >= 0)
        {
            // 같은 아이템이면 수량 증가
            var copy = ItemManager.Instance.GetItem(currentList[index]);
            copy.count++;
            currentList[index] = copy.id;
        }
        else
        {
            // 빈칸에 새로 추가
            TryAddItem(itemID);
        }
    }

    // 빈칸 정리
    public void CompactItemList()
    {
        var validIDs = new List<int>();
        foreach (var id in itemIDs)
        {
            if (id != 0) validIDs.Add(id);
        }

        while (validIDs.Count < invenSize)
            validIDs.Add(0);

        itemIDs = validIDs;
    }

    // UI 갱신
    public virtual void RefreshUI()
    {
        for (int i = 0; i < slotObjs.Count; i++)
        {
            int id = (i < itemIDs.Count) ? itemIDs[i] : 0;
            var slot = slotObjs[i].GetComponent<ItemSlot>();
            slot.SetItem(id);
        }
    }

    public void InvenOpen()
    {
        RefreshUI();
        gameObject.SetActive(true);
    }

    public void InvenClose()
    {
        gameObject.SetActive(false);
    }
}
