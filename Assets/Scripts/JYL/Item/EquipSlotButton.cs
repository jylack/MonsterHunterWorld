using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlotButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EquipSlot slotType;

    public void OnPointerClick(PointerEventData eventData)
    {
        EquipManager.Instance.OpenEquipSelectUI(slotType);
    }
}
