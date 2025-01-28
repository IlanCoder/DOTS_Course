using System;
using ECS.Systems;
using Unity.Mathematics;
using UnityEngine;

namespace UI {
    [RequireComponent (typeof(RectTransform))]
    public class SelectionAreaController : MonoBehaviour {
        RectTransform _rectTransform;
        Canvas _canvas;
        Vector2 _selectionStartPos;
        Vector2 _lowerLeftCorner;
        Vector2 _upperRightCorner;
        Rect _selectionRect;

        void Awake() {
            _rectTransform = GetComponent<RectTransform> ();
            _canvas = GetComponentInParent<Canvas>();
        }
        
        void Update() {
            UpdateVisual();
        }

        void OnEnable() {
            _selectionStartPos = Input.mousePosition;
        }

        void OnDisable() {
            _rectTransform.sizeDelta = Vector2.zero;
        }

        void UpdateVisual() {
            UpdateSelectionRect();
            float canvasScale = _canvas.scaleFactor;
            _rectTransform.anchoredPosition = _selectionRect.position / canvasScale;
            _rectTransform.sizeDelta = _selectionRect.size / canvasScale;
        }

        void UpdateSelectionRect() {
            Vector2 mousePos = Input.mousePosition;
            _lowerLeftCorner.x = math.min(mousePos.x, _selectionStartPos.x);
            _lowerLeftCorner.y= math.min(mousePos.y, _selectionStartPos.y);
            _upperRightCorner.x = math.max(mousePos.x, _selectionStartPos.x);
            _upperRightCorner.y = math.max(mousePos.y, _selectionStartPos.y);
            _selectionRect.position = _lowerLeftCorner;
            _selectionRect.size = _upperRightCorner - _lowerLeftCorner;
        }
    }
}