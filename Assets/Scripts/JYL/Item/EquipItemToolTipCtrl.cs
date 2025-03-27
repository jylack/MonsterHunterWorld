using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EquipItemToolTipCtrl : MonoBehaviour
{
    public enum TooltipPage { Page1, Page2 }
    private TooltipPage currentPage;

    [Header("공통 UI")]
    [SerializeField] private GameObject page1Obj;
    [SerializeField] private GameObject page2Obj;

    [SerializeField] private Image image;
    [SerializeField] private Text itemName;
    [SerializeField] private Text toolTip;
    [SerializeField] private Text sellGold;

    [Header("무기 전용 (1페이지)")]
    [SerializeField] private GameObject weaponObj;
    [SerializeField] private Text weaponRarity;
    [SerializeField] private Text damage;
    [SerializeField] private Text attribute;

    [Header("방어구 전용 (1페이지)")]
    [SerializeField] private GameObject armorObj;
    [SerializeField] private Text armorRarity;
    [SerializeField] private Text level;
    [SerializeField] private Text defense;
    [SerializeField] private Text fireDef;
    [SerializeField] private Text waterDef;
    [SerializeField] private Text lightningDef;
    [SerializeField] private Text iceDef;
    [SerializeField] private Text dragonDef;

    [Header("2페이지 전용")]
    [SerializeField] private Image page2Image;
    [SerializeField] private Text page2Name;
    [SerializeField] private Text page2Description;
    [SerializeField] private Text[] skillTexts;

    private BaseItem currentItem;

    private void Awake()
    {
        HideTooltip();
    }

    public void ShowTooltip(BaseItem item)
    {
        currentItem = item;
        ShowPage(TooltipPage.Page1);
    }

    public void ShowPage(TooltipPage page)
    {
        if (currentItem == null || currentItem.type == ItemType.Empty)
        {
            HideTooltip();
            return;
        }

        page1Obj.SetActive(page == TooltipPage.Page1);
        page2Obj.SetActive(page == TooltipPage.Page2);
        currentPage = page;

        if (page == TooltipPage.Page1) ShowPage1(currentItem);
        else ShowPage2(currentItem);
    }

    private void ShowPage1(BaseItem item)
    {
        // 공통
        image.sprite = item.image;
        itemName.text = item.name;
        toolTip.text = item.tooltip;
        sellGold.text = item.price.ToString();

        if (item is Weapon weapon)
        {
            armorObj.SetActive(false);
            weaponObj.SetActive(true);

            weaponRarity.text = weapon.rarity;
            damage.text = weapon.damage.ToString();
            attribute.text = weapon.attribute.ToString();
        }
        else if (item is Armor armor)
        {
            weaponObj.SetActive(false);
            armorObj.SetActive(true);

            armorRarity.text = armor.rarity;
            level.text = armor.level.ToString();
            defense.text = armor.defense.ToString();
            fireDef.text = armor.fireDef.ToString();
            waterDef.text = armor.waterDef.ToString();
            lightningDef.text = armor.LightningDef.ToString();
            iceDef.text = armor.IceDef.ToString();
            dragonDef.text = armor.DragonDef.ToString();
        }
    }
private void ShowPage2(BaseItem item)
{
    page2Image.sprite = item.image;
    page2Name.text = item.name;
    page2Description.text = item.tooltip;

    //for (int i = 0; i < skillTexts.Length; i++)
    //{
    //    if (item.skills != null && i < item.skills.Count)
    //    {
    //        skillTexts[i].text = item.skills[i];
    //    }
    //    else
    //    {
    //        skillTexts[i].text = "";
    //    }
    //}
}

    public void TogglePage()
    {
        if (currentPage == TooltipPage.Page1)
            ShowPage(TooltipPage.Page2);
        else
            ShowPage(TooltipPage.Page1);
    }

    public void HideTooltip()
    {
        page1Obj.SetActive(false);
        page2Obj.SetActive(false);
    }

    public void Clear()
    {

    }
}
