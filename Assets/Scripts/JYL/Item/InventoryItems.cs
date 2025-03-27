using UnityEngine;

public class InventoryItems : BaseInventory
{
    private void Start()
    {
        invenType = InvenType.Inven;

        // 기본 슬롯 설정
        SlotSetting(slotParent);

        // 데이터 초기화
        InvenInit();

        // 테스트용 아이템 추가
        TryAddItem(1001); // 헌터 나이프
        TryAddItem(2001); // 헌터 헬름
        TryAddItem(2002); // 헌터 체스트
        TryAddItem(2003); // 헌터 암

        // UI 갱신
        RefreshUI();
    }

    private void OnEnable()
    {
        RefreshUI(); // 활성화 시 갱신
    }
}
