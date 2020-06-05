using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Transform itemTr;
    Transform inventoryTr;
    Transform itemListTr;
    CanvasGroup canvasGroup;

    public static GameObject draggingItem = null;

    // Start is called before the first frame update
    void Start()
    {
        itemTr = GetComponent<Transform>();
        inventoryTr = GameObject.Find("Inventory").GetComponent<Transform>();
        itemListTr = GameObject.Find("ItemList").GetComponent<Transform>();

        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.transform.SetParent(inventoryTr);
        draggingItem = this.gameObject;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null;
        canvasGroup.blocksRaycasts = true;

        if(itemTr.parent == inventoryTr)
        {
            itemTr.SetParent(itemListTr.transform);
        }
    }
}
