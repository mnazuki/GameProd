using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableTicket : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // Make semi-transparent
        canvasGroup.blocksRaycasts = false; // Allow raycast passing
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true; // Restore raycasts

        // Check if dropped on a passenger
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Passenger"))
        {
            Passenger passenger = hit.collider.GetComponent<Passenger>();
            if (passenger != null)
            {
                passenger.ReceiveTicket(); // Passenger takes the ticket
                Destroy(gameObject); // Destroy ticket
            }
        }
    }
}
