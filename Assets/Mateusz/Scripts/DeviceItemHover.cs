using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeviceItemHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public DeviceConnection manager;

    private Outline outline;
    private bool isSelected = false;

    void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (outline != null && !isSelected)
        {
            outline.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSelected) { Deselect(); } else { Select(); }

        if (manager != null)
        {
            manager.ShowConnectButton(this);
        }
    }

    public void Deselect()
    {
        isSelected = false;
        if (outline)
        {
            outline.enabled = false;
        }
    }

    public void Select()
    {
        isSelected = true;
        if (outline)
        {
            outline.enabled = true;
        }
    }
}
