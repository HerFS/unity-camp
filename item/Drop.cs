using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        if (transform.childCount == 0)
        {
            Drag.draggingItem.transform.SetParent(this.transform);
            Drag.draggingItem.transform.position = this.transform.position;
        }
    }
}
