using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Selectable))]
public class ButtonAdvanced : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent clickMethod;

    public void OnPointerClick(PointerEventData eventData) => clickMethod?.Invoke();
}