using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<BaseItem> onClick;

    [SerializeField] private InvenType invenType;
    [SerializeField] private Sprite[] sprites = new Sprite[2];
    [SerializeField] private GameObject itemImage;
    [SerializeField] private Text countText;

    private Image slotImage;
    private Image itemIcon;

    private Coroutine fadeCoroutine;

    private BaseItem currentItem = null;
    public BaseItem Item => currentItem;

    private void Start()
    {
        slotImage = GetComponent<Image>();
        itemIcon = itemImage.GetComponent<Image>();
        SetItem(ItemDataBase.Instance.emptyItem);
    }

    public void SetInvenType(InvenType type) => invenType = type;

    public void SetItem(BaseItem item)
    {
        currentItem = item;

        itemIcon.sprite = item.image;
        itemIcon.color = item.color;
        countText.text = item.count > 0 ? item.count.ToString() : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InvenToryCtrl.Instance.IsEquipSelectOpen)
        {
            EquipManager.Instance.SelectItem(currentItem.id);
            return;
        }

        switch (invenType)
        {
            case InvenType.Inven:
            case InvenType.Box:
                InvenToryCtrl.Instance.ChangeItemByKey(invenType, currentItem.id);
                if (currentItem.count <= 0)
                    SetItem(ItemDataBase.Instance.emptyItem);
                break;

            case InvenType.EquipBox:
                EquipManager.Instance.EquipItemByID(currentItem.id);
                break;

            case InvenType.Equipped:
                int slotIndex = (int)currentItem.GetEquipSlot();
                var item = ItemDataBase.Instance.itemDB[slotIndex];
                
                EquipSlot temp = EquipSlot.end;

                if (item.type == ItemType.Armor)
                {
                    var i = item as Armor;
                    temp = i.equipType;

                }
                else if (item.type == ItemType.Weapon)
                {
                    var i = item as Weapon;
                    temp = i.equipType;
                }

                EquipManager.Instance.UnEquipItemBySlot(temp);
                break;
        }

        onClick?.Invoke(currentItem);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        slotImage.sprite = sprites[1];

        if (invenType == InvenType.Inven || invenType == InvenType.Box)
            InvenToryCtrl.Instance.ItemToolTipCtrl.ToolTipSetItem(currentItem);
        else
            InvenToryCtrl.Instance.EquipItemToolTipCtrl.ShowTooltip(currentItem);

        fadeCoroutine = StartCoroutine(FadeAlphaLoop(slotImage));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (invenType == InvenType.Inven || invenType == InvenType.Box)
            InvenToryCtrl.Instance.ItemToolTipCtrl.TooltipClear(false);
        else
            InvenToryCtrl.Instance.EquipItemToolTipCtrl.Clear();

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        ClearSlot();
    }

    public void ClearSlot()
    {
        slotImage.color = Color.white;
        slotImage.sprite = sprites[0];
    }

    private IEnumerator FadeAlphaLoop(Image targetImage)
    {
        bool fadingOut = true;
        float alpha = 1f;
        const float alphaSpeed = 2f;
        const float minAlpha = 0.3f;

        while (true)
        {
            alpha += (fadingOut ? -1 : 1) * alphaSpeed * Time.deltaTime;

            if (alpha <= minAlpha) { alpha = minAlpha; fadingOut = false; }
            else if (alpha >= 1f) { alpha = 1f; fadingOut = true; }

            targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, alpha);
            yield return null;
        }
    }
}
