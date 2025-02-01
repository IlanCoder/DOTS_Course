using System;
using ECS.Systems;
using UnityEngine;
using UnitSelectionSystem = ECS.Systems.Selection.UnitSelectionSystem;

namespace UI {
    public class UIUnitSelectionManager : MonoBehaviour {
        [SerializeField] GameObject selectionAreaRect;

        void Start() {
            selectionAreaRect.SetActive(false);
        }

        void OnEnable() {
            UnitSelectionSystem.Instance.OnSelectionStart += Handle_OnSelectAreaStart;
            UnitSelectionSystem.Instance.OnSelectionEnd += Handle_OnSelectAreaEnd;
        }

        void OnDisable() {
            UnitSelectionSystem.Instance.OnSelectionStart -= Handle_OnSelectAreaStart;
            UnitSelectionSystem.Instance.OnSelectionEnd -= Handle_OnSelectAreaEnd;
        }

        void Handle_OnSelectAreaStart(object sender, EventArgs e) {
            selectionAreaRect.SetActive(true);
        }

        void Handle_OnSelectAreaEnd(object sender, EventArgs e) {
            selectionAreaRect.SetActive(false);
        }
    }
}