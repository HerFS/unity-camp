using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform itemTr;
    private Transform inventoryTr;
    public static GameObject draggingItem = null;
    Transform itemListTr;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        itemTr = GetComponent<Transform>();
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryTr = GameObject.Find("Inventory").GetComponent<Transform>();
        itemListTr = GameObject.Find("ItemList").GetComponent<Transform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();

        // 드래그 이벤트가 발생하면 아이템의 위치를 마우스 커서의 위치로 변경
        itemTr.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        this.transform.SetParent(inventoryTr);
        draggingItem = this.gameObject;
        canvasGroup.blocksRaycasts = false;
        // 드래그가 시작되면 다른 UI 이벤트를 받지 않겠다.
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();

        // OnEndDrag is called once end drag
        draggingItem = null;
        canvasGroup.blocksRaycasts = true;

        if (itemTr.parent == inventoryTr)
        {
            itemTr.SetParent(itemListTr.transform);
        }
    }
}
