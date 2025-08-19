using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CSTI_ZLib.UI.Com;

public class EventOnDropOn : MonoBehaviour, IDropHandler
{
    public delegate void OnCardDropOnDelegate(IReadOnlyList<InGameCardBase> draggedCards);

    public event OnCardDropOnDelegate? CardDropOnEvent;

    public void OnDrop(PointerEventData eventData)
    {
        var gameManager = GameManager.Instance;
        if (gameManager == null) return;
        if (GameManager.DraggedCardStack is { Count: > 0 })
        {
            var cards = GameManager.DraggedCardStack.ToList();
            var inGameDraggableCard = GameManager.DraggedCard;
            var slot = inGameDraggableCard.CurrentSlot;
            GameManager.ClearDraggedStack();
            GameManager.EndDragItem(inGameDraggableCard);

            CardDropOnEvent?.Invoke(cards);
            slot.SortCardPile();
        }
    }
}