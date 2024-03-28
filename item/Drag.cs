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

        // �巡�� �̺�Ʈ�� �߻��ϸ� �������� ��ġ�� ���콺 Ŀ���� ��ġ�� ����
        itemTr.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        this.transform.SetParent(inventoryTr);
        draggingItem = this.gameObject;
        canvasGroup.blocksRaycasts = false;
        // �巡�װ� ���۵Ǹ� �ٸ� UI �̺�Ʈ�� ���� �ʰڴ�.
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
