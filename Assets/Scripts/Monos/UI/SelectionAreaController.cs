using ECS.Systems;
using UnityEngine;

namespace UI {
    [RequireComponent (typeof(RectTransform))]
    public class SelectionAreaController : MonoBehaviour {
        RectTransform _rectTransform;
        Canvas _canvas;

        void Awake() {
            _rectTransform = GetComponent<RectTransform> ();
            _canvas = GetComponentInParent<Canvas>();
        }
        
        void Update() {
            UpdateVisual();
        }

        void UpdateVisual() {
            Rect selectionRect = UnitSelectionSystem.Instance.GetSelectionRect();
            float canvasScale = _canvas.scaleFactor;
            _rectTransform.anchoredPosition = selectionRect.position / canvasScale;
            _rectTransform.sizeDelta = selectionRect.size / canvasScale;
        }

        void OnDisable() {
            _rectTransform.sizeDelta= Vector2.zero;
        }
    }
}