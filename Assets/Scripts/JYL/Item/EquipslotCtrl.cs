using UnityEngine;
using UnityEngine.UI;

public class EquipSlotCtrl : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text itemName;

    public void SlotListSetting(BaseItem item)
    {
        icon.sprite = item.image;
        icon.color = item.color;
        itemName.text = item.name;
    }
}
