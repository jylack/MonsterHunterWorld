using UnityEngine;
using UnityEngine.UI;

public class BoxInvenTory : BaseInventory, IClosableUI
{
    [SerializeField] private Text boxIndexText;

    private int boxIndex = 1;
    private int boxMaxIndex = 10;
    private ItemType selectBoxTag = ItemType.All;

    public bool IsOpen => gameObject.activeSelf;

    private const int pageSize = 100;

    private void Start()
    {
        invenType = InvenType.Box;

        SlotSetting(slotParent);
        InvenInit();

        // 예시: 아이템 추가 (id 기준)
        TryAddItem(1001);
        TryAddItem(2001);
        TryAddItem(2002);
        TryAddItem(3001);

        RefreshUI();
    }

    private void Update()
    {
        if (IsOpen)
        {
            BoxInput();
        }
    }

    public void OpenBox()
    {
        boxIndex = 1;
        InvenOpen();
        RefreshUI();
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        UIManager.Instance.RegisterUI(this);
    }

    private void OnDisable()
    {
        UIManager.Instance.UnregisterUI(this);
    }

    public void SelectTag(ItemType tag)
    {
        selectBoxTag = tag;
        boxIndex = 1;
        RefreshUI();
    }

    public override void RefreshUI()
    {
        for (int i = 0; i < slotObjs.Count; i++)
        {
            int globalIndex = (boxIndex - 1) * pageSize + i;

            int id = (globalIndex < itemIDs.Count) ? itemIDs[globalIndex] : 0;

            // 태그 필터 적용
            var item = ItemManager.Instance.GetItem(id);
            if (selectBoxTag != ItemType.All && item.type != selectBoxTag)
            {
                slotObjs[i].GetComponent<ItemSlot>().SetItem(0); // 빈칸 처리
            }
            else
            {
                slotObjs[i].GetComponent<ItemSlot>().SetItem(id);
            }
        }

        boxIndexText.text = $"{boxIndex} / {boxMaxIndex}";
    }

    private void BoxInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            boxIndex = Mathf.Max(1, boxIndex - 1);
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            boxIndex = Mathf.Min(boxMaxIndex, boxIndex + 1);
            RefreshUI();
        }
    }
}
