using System.Collections.Generic;
using UnityEngine;

// 전체 아이템을 ID로 관리하는 싱글톤 DB
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    // 아이템 DB (id → 원본 아이템)
    private Dictionary<int, BaseItem> itemDB = new Dictionary<int, BaseItem>();

    // 빈 아이템 (null 대용)
    public BaseItem EmptyItem { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InitDatabase();
    }

    // 아이템 DB 초기화
    private void InitDatabase()
    {
        // 예: 빈 아이템
        EmptyItem = new BaseItem
        {
            id = 0,
            name = "빈 슬롯",
            type = ItemType.Empty,
            maxCount = 0,
            count = 0,
            image = null,
            tooltip = "",
            price = 0,
            color = new Color(1, 1, 1, 0)
        };
        itemDB[0] = EmptyItem;

        // 예: 무기
        itemDB[1001] = new Weapon
        {
            id = 1001,
            name = "헌터 나이프",
            type = ItemType.Weapon,
            maxCount = 1,
            count = 1,
            damage = 80,
            attribute = Attribute.Fire,
            tooltip = "초보자가 쓰는 전통의 검",
            price = 150
        };

        // 예: 방어구
        itemDB[2001] = new Armor
        {
            id = 2001,
            name = "헌터 헬름",
            type = ItemType.Armor,
            equipType = EquipSlot.Head,
            maxCount = 1,
            count = 1,
            defense = 25,
            tooltip = "헌터용 머리 방어구",
            price = 120
        };

        // 이후 itemDB[XXXX] = new Potion { ... } 형태로 계속 확장
    }

    // 아이템 복사본 반환
    public BaseItem GetItem(int id)
    {
        if (itemDB.TryGetValue(id, out var item))
        {
            return item.Clone(); // 항상 복사본 반환
        }
        return EmptyItem;
    }

    // 아이템 원본 직접 접근 (툴팁 전용 등)
    public BaseItem GetOriginal(int id)
    {
        return itemDB.ContainsKey(id) ? itemDB[id] : EmptyItem;
    }
}
